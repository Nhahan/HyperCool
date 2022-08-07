using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Experimental.Animations;
using SlicePlaneSerializable = DinoFracture.FractureGeometry.SlicePlaneSerializable;

namespace DinoFracture.Editor
{
    public struct SlicePlaneData
    {
        public SerializedProperty Property;
        public SlicePlaneSerializable Plane;
    }

    public struct FractureGeometryTargetData
    {
        public FractureGeometry[] Targets;
        public SlicePlaneData[] SlicePlanes;
        public SerializedProperty SlicePlanesProp;
    }

    public class FractureGeometryEditor : UnityEditor.Editor
    {
        protected struct PropertyName : IEquatable<PropertyName>
        {
            public static readonly PropertyName cInvalid = new PropertyName(null, null, null);

            private string _memberName;
            private string _displayName;
            private string _tooltip;

            public string MemberName => _memberName;

            public string DisplayName => _displayName;

            public string Tooltip => _tooltip;

            public PropertyName(string memberName, string displayName = "", string tooltip = "")
            {
                _memberName = memberName;
                _displayName = displayName;
                _tooltip = tooltip;
            }

            public PropertyName(SerializedProperty prop)
            {
                _memberName = prop.name;
                _displayName = prop.displayName;
                _tooltip = prop.tooltip;
            }

            public static implicit operator PropertyName(string memberName)
            {
                return new PropertyName(memberName);
            }

            public static implicit operator PropertyName(SerializedProperty prop)
            {
                return new PropertyName(prop);
            }

            public bool Equals(PropertyName other)
            {
                return (_memberName == other._memberName);
            }

            public override int GetHashCode()
            {
                return _memberName.GetHashCode();
            }

            public override string ToString()
            {
                return _memberName;
            }
        }

        private bool _editingPlanes = false;

        private AnimBool _shatterGroupAnim = new AnimBool();
        private AnimBool _slicesGroupAnim = new AnimBool();

        private int _selectedSlicePlaneIdx = -1;
        private FractureGeometryTargetData _targetData = new FractureGeometryTargetData();

        private static Dictionary<string, PropertyName> _sCommonPropertyNames;
        private static Dictionary<string, PropertyName> _sShatterFracturePropertyNames;
        private static Dictionary<string, PropertyName> _sFractureEventPropertyNames;

        protected static GUIStyle _cHeaderTextStyle;
        protected static GUIStyle _cItemBackgroundStyle;
        protected static GUIStyle _cSelectedItemBackgroundStyle;
        protected static GUIStyle _cNoticeTextStyle;

        protected static readonly Color _cEditModeButtonBackgroundColor = new Color(1.0f, 0.6f, 0.6f, 0.7f);
        protected static readonly Color _cSelectedBackgroundColor = new Color(0.6f, 1.0f, 0.6f, 0.5f);

        static FractureGeometryEditor()
        {
            _sCommonPropertyNames = new Dictionary<string, PropertyName>();
            AddPropertyName(_sCommonPropertyNames, "InsideMaterial");
            AddPropertyName(_sCommonPropertyNames, "FractureTemplate");
            AddPropertyName(_sCommonPropertyNames, "PiecesParent");
            AddPropertyName(_sCommonPropertyNames, "UVScale");
            AddPropertyName(_sCommonPropertyNames, "DistributeMass");
            AddPropertyName(_sCommonPropertyNames, "SeparateDisjointPieces");
            AddPropertyName(_sCommonPropertyNames, "RandomSeed");
            AddPropertyName(_sCommonPropertyNames, "NumGenerations");

            _sShatterFracturePropertyNames = new Dictionary<string, PropertyName>();
            AddPropertyName(_sShatterFracturePropertyNames, "NumFracturePieces");
            AddPropertyName(_sShatterFracturePropertyNames, "NumIterations");
            AddPropertyName(_sShatterFracturePropertyNames, new PropertyName("EvenlySizedPieces", "Evenly Sized Pieces (EXPERIMENTAL)"));
            AddPropertyName(_sShatterFracturePropertyNames, "FractureRadius");

            _sFractureEventPropertyNames = new Dictionary<string, PropertyName>();
            AddPropertyName(_sFractureEventPropertyNames, "OnFractureCompleted");
        }

        protected static void AddPropertyName(Dictionary<string, PropertyName> names, in PropertyName nameInfo)
        {
            names.Add(nameInfo.MemberName, nameInfo);
        }

        protected virtual void OnEnable()
        {
            RefreshTargetData();

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        protected virtual void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedo;

            EndPlaneEditMode();
        }

        protected virtual void OnSceneGUI()
        {
            for (int t = 0; t < _targetData.Targets.Length; t++)
            {
                DrawTargetsSlicePlanes(_targetData.Targets[t]);
            }

            if (_editingPlanes && _selectedSlicePlaneIdx >= 0)
            {
                RefreshPlaneTransforms();

                ref SlicePlaneSerializable plane = ref _targetData.SlicePlanes[_selectedSlicePlaneIdx].Plane;

                for (int i = 0; i < _targetData.Targets.Length; i++)
                {
                    Matrix4x4 worldTrans;
                    GetPlaneWorldSpaceTransform(ref plane, _targetData.Targets[i], out worldTrans);

                    Vector3 pos = worldTrans.GetPosition();
                    Quaternion rot = worldTrans.rotation;
                    float scale = worldTrans.lossyScale.x;

#if UNITY_2019_1_OR_NEWER
                    EditorGUI.BeginChangeCheck();
                    Handles.TransformHandle(ref pos, ref rot, ref scale);
                    bool changed = EditorGUI.EndChangeCheck();
#else                  
                    EditorGUI.BeginChangeCheck();
                    pos = Handles.PositionHandle(pos, rot);
                    rot = Handles.RotationHandle(rot, pos);
                    scale = Handles.ScaleValueHandle(scale, pos, rot, HandleUtility.GetHandleSize(pos), Handles.CubeHandleCap, 0.0f);
                    bool changed = EditorGUI.EndChangeCheck();
#endif

                    if (changed)
                    {
                        var trans = _targetData.Targets[i].transform.worldToLocalMatrix;
                        Matrix4x4 localMat = trans * Matrix4x4.TRS(pos, rot, Vector3.one);

                        var slicePlaneProp = _targetData.SlicePlanes[_selectedSlicePlaneIdx].Property;

                        slicePlaneProp.FindPropertyRelative("Position").vector3Value = localMat.GetPosition();
                        slicePlaneProp.FindPropertyRelative("Rotation").quaternionValue = localMat.rotation;
                        slicePlaneProp.FindPropertyRelative("Scale").floatValue = scale;

                        slicePlaneProp.serializedObject.ApplyModifiedProperties();

                        plane.Position = localMat.GetPosition();
                        plane.Rotation = localMat.rotation;
                        plane.Scale = scale;

                        break;
                    }
                }
            }
        }

        private void RefreshTargetData()
        {
            _targetData.Targets = new FractureGeometry[targets.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                _targetData.Targets[i] = (FractureGeometry)targets[i];
            }

            _targetData.SlicePlanesProp = serializedObject.FindProperty("SlicePlanes");
            _targetData.SlicePlanes = new SlicePlaneData[_targetData.SlicePlanesProp.arraySize];
            for (int i = 0; i < _targetData.SlicePlanesProp.arraySize; i++)
            {
                _targetData.SlicePlanes[i].Property = _targetData.SlicePlanesProp.GetArrayElementAtIndex(i);
            }
            RefreshPlaneTransforms();

            if (_targetData.SlicePlanes == null || _targetData.SlicePlanes.Length == 0)
            {
                _selectedSlicePlaneIdx = -1;
            }
        }

        private void RefreshPlaneTransforms()
        {
            for (int i = 0; i < _targetData.SlicePlanesProp.arraySize; i++)
            {
                _targetData.SlicePlanes[i].Plane = _targetData.Targets[0].SlicePlanes[i];
            }
        }

        private void DrawTargetsSlicePlanes(FractureGeometry fractureGeometry)
        {
            if (fractureGeometry.FractureType != FractureType.Slice)
            {
                return;
            }

            for (int i = 0; i < fractureGeometry.SlicePlanes.Length; i++)
            {
                bool isSelected = (_editingPlanes && _selectedSlicePlaneIdx == i);
                DrawSlicePlane(ref fractureGeometry.SlicePlanes[i], fractureGeometry, isSelected);
            }
        }

        private void DrawSlicePlane(ref SlicePlaneSerializable plane, UnityEngine.Object owner, bool isSelected = false)
        {
            Matrix4x4 worldTrans;
            GetPlaneWorldSpaceTransform(ref plane, owner, out worldTrans);

            Vector3 up = worldTrans.GetColumn(1);
            Vector3 right = worldTrans.GetColumn(0);

            float size = worldTrans.lossyScale.x;
            Vector3 pos = worldTrans.GetPosition();

            Vector3[] corners = new[] { pos + (up + right) * size, pos + (up - right) * size, pos + (-up - right) * size, pos + (-up + right) * size, pos + (up + right) * size };

            Color planeColor = Color.green * 0.75f;
            planeColor.a = 0.5f;

            Color borderColor = Color.green;

            if (!isSelected)
            {
                planeColor *= 0.6f;
                borderColor *= 0.6f;
            }

            var saved = Handles.zTest;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            Handles.DrawSolidRectangleWithOutline(corners, planeColor, borderColor);
            Handles.zTest = saved;
        }

        protected virtual void EnsureStyles()
        {
            if (_cHeaderTextStyle == null)
            {
                _cHeaderTextStyle = new GUIStyle(EditorStyles.boldLabel);
                _cHeaderTextStyle.alignment = TextAnchor.MiddleCenter;
            }

            if (_cItemBackgroundStyle == null)
            {
                _cItemBackgroundStyle = new GUIStyle(GUIStyle.none);
                _cItemBackgroundStyle.normal.background = MakeColorBackground(new Color(1.0f, 1.0f, 1.0f, 0.05f));
            }

            if (_cSelectedItemBackgroundStyle == null)
            {
                _cSelectedItemBackgroundStyle = new GUIStyle(GUIStyle.none);

                Color color = _cSelectedBackgroundColor;
                color.a = 0.2f;
                _cSelectedItemBackgroundStyle.normal.background = MakeColorBackground(color);
            }

            if (_cNoticeTextStyle == null)
            {
                _cNoticeTextStyle = new GUIStyle(EditorStyles.boldLabel);
                _cNoticeTextStyle.alignment = TextAnchor.MiddleCenter;
                _cNoticeTextStyle.wordWrap = true;
                _cNoticeTextStyle.fontSize = 12;
                _cNoticeTextStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                _cNoticeTextStyle.normal.background = MakeColorBackground(new Color(0.75f, 0.2f, 0.2f, 0.6f));
            }
        }

        private Texture2D MakeColorBackground(Color32 color)
        {
            Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);

            var colors = tex.GetPixels32();
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }
            tex.SetPixels32(colors);

            tex.Apply();

            return tex;
        }

        protected bool DrawCommonFractureProperties()
        {
            EnsureStyles();

            bool changed = false;

            EditorGUILayout.LabelField("Common Properties", _cHeaderTextStyle);
            changed |= DrawFractureProperties(_sCommonPropertyNames);

            Space(10);

            EditorGUILayout.LabelField("Fracture Properties", _cHeaderTextStyle);

            var fractureTypeProp = serializedObject.FindProperty("FractureType");
            FractureType type = (FractureType)fractureTypeProp.enumValueIndex;

#if UNITY_2019_1_OR_NEWER
            if (type == FractureType.Slice && !SceneView.lastActiveSceneView.drawGizmos)
            {
                EditorGUILayout.LabelField("Turn on scene filters to see planes and gizmos in the scene view", _cNoticeTextStyle);
                Space(10);
            }
#endif

            DrawProperty(fractureTypeProp, PropertyName.cInvalid);

            if (changed || !fractureTypeProp.hasMultipleDifferentValues)
            {
                fractureTypeProp = serializedObject.FindProperty("FractureType");
                type = (FractureType)fractureTypeProp.enumValueIndex;
            }

            if (type == FractureType.Shatter && !fractureTypeProp.hasMultipleDifferentValues)
            {
                EndPlaneEditMode();

                _shatterGroupAnim.target = true;
                _slicesGroupAnim.target = false;
            }
            else if (type == FractureType.Slice && !fractureTypeProp.hasMultipleDifferentValues)
            {
                _shatterGroupAnim.target = false;
                _slicesGroupAnim.target = true;
            }
            else
            {
                EndPlaneEditMode();

                _shatterGroupAnim.target = false;
                _slicesGroupAnim.target = false;
            }

            using (var group = new EditorGUILayout.FadeGroupScope(_shatterGroupAnim.faded))
            {
                if (group.visible)
                {
                    changed |= DrawShatterFractureProperties();
                }
            }

            using (var group = new EditorGUILayout.FadeGroupScope(_slicesGroupAnim.faded))
            {
                if (group.visible)
                {
                    changed |= DrawSliceFractureProperties();
                }
            }

            return changed;
        }

        protected bool DrawShatterFractureProperties()
        {
            return DrawFractureProperties(_sShatterFracturePropertyNames);
        }

        protected bool DrawSliceFractureProperties()
        {
            var planesProp = serializedObject.FindProperty("SlicePlanes");

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField($"Plane count: {(planesProp.hasMultipleDifferentValues ? "-" : planesProp.arraySize.ToString())}");

                if (DrawToggleButton(new GUIContent(_editingPlanes ? "End Edit" : "Edit Planes"), _editingPlanes, _cEditModeButtonBackgroundColor, 100.0f))
                {
                    if (_editingPlanes)
                    {
                        EndPlaneEditMode();
                    }
                    else
                    {
                        StartPlaneEditMode();
                    }
                }
            }

            if (_editingPlanes)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    const float cMargin = 6.0f;

                    Space(cMargin, false);

                    using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(true)))
                    {
                        if (!planesProp.hasMultipleDifferentValues)
                        {
                            for (int i = 0; i < _targetData.SlicePlanes.Length; i++)
                            {
                                DrawSlicePlaneProperty(i);

                                Space(10.0f);
                            }

                            if (GUILayout.Button("Add Plane"))
                            {
                                planesProp.InsertArrayElementAtIndex(planesProp.arraySize);
                                serializedObject.ApplyModifiedProperties();

                                for (int i = 0; i < targets.Length; i++)
                                {
                                    SetPropertyValue(planesProp.GetArrayElementAtIndex(planesProp.arraySize - 1), SlicePlaneSerializable.Identity, targets[i]);
                                }
                                serializedObject.ApplyModifiedProperties();

                                RefreshTargetData();

                                ClearSelectedPlane();
                            }
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Multiple values. Clear to reset.");
                        }

                        if (GUILayout.Button("Clear All Planes"))
                        {
                            planesProp.ClearArray();
                            serializedObject.ApplyModifiedProperties();

                            RefreshTargetData();
                            ClearSelectedPlane();
                        }
                    }

                    Space(cMargin, false);
                }

                EditorGUILayout.Space();
            }
            else
            {
                ClearSelectedPlane();
            }

            return false;
        }

        public void DrawFractureEventProperties()
        {
            DrawFractureProperties(_sFractureEventPropertyNames);
        }

        private void DrawSlicePlaneProperty(int idx)
        {
            using (new EditorGUILayout.VerticalScope((_selectedSlicePlaneIdx == idx) ? _cSelectedItemBackgroundStyle : _cItemBackgroundStyle))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (DrawToggleButton(new GUIContent("S", "Select Plane"), (_selectedSlicePlaneIdx == idx), _cSelectedBackgroundColor, 0.0f))
                    {
                        if (_selectedSlicePlaneIdx == idx)
                        {
                            ClearSelectedPlane();
                        }
                        else
                        {
                            SetSelectedPlane(idx);
                        }
                    }

                    if (GUILayout.Button(new GUIContent("X", "Delete Plane"), GUILayout.ExpandWidth(false)))
                    {
                        _targetData.SlicePlanesProp.DeleteArrayElementAtIndex(idx);
                        serializedObject.ApplyModifiedProperties();

                        RefreshTargetData();

                        ClearSelectedPlane();

                        return;
                    }
                }

                DrawProperty(_targetData.SlicePlanesProp.GetArrayElementAtIndex(idx).FindPropertyRelative("Position"), PropertyName.cInvalid);
                DrawQuaternionProperty(_targetData.SlicePlanesProp.GetArrayElementAtIndex(idx).FindPropertyRelative("Rotation"), PropertyName.cInvalid);
                DrawProperty(_targetData.SlicePlanesProp.GetArrayElementAtIndex(idx).FindPropertyRelative("Scale"), PropertyName.cInvalid);
            }
        }

        private void StartPlaneEditMode()
        {
            if (!_editingPlanes)
            {
                _editingPlanes = true;

                Tools.hidden = true;

                ClearSelectedPlane();
            }
        }

        private void EndPlaneEditMode()
        {
            if (_editingPlanes)
            {
                _editingPlanes = false;

                Tools.hidden = false;

                ClearSelectedPlane();
            }
        }

        private void SetSelectedPlane(int idx)
        {
            _selectedSlicePlaneIdx = idx;
            SceneView.RepaintAll();
        }

        private void ClearSelectedPlane()
        {
            SetSelectedPlane(-1);
        }

        protected bool DrawFractureProperties(Dictionary<string, PropertyName> validNames)
        {
            var obj = serializedObject;
            bool changed = false;

            obj.UpdateIfRequiredOrScript();
            SerializedProperty iterator = obj.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                PropertyName nameInfo;
                if (!validNames.TryGetValue(iterator.name, out nameInfo))
                {
                    continue;
                }

                using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    changed |= DrawProperty(iterator, nameInfo);
            }
            obj.ApplyModifiedProperties();

            return changed;
        }

        protected bool DrawProperty(SerializedProperty property, in PropertyName nameInfo)
        {
            bool changed;

            string displayName = (string.IsNullOrEmpty(nameInfo.DisplayName) ? property.displayName : nameInfo.DisplayName);
            string toolTip = (string.IsNullOrEmpty(nameInfo.Tooltip) ? property.tooltip : nameInfo.Tooltip);

            if (property.propertyType == SerializedPropertyType.Enum)
            {
                // Enums have trouble displaying when they have different values
                EditorGUI.BeginChangeCheck();

                EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
                int value = (property.hasMultipleDifferentValues ? -1 : property.enumValueIndex);
                value = EditorGUILayout.Popup(new GUIContent(displayName, toolTip), value, property.enumDisplayNames);

                changed = EditorGUI.EndChangeCheck();

                if (changed)
                {
                    property.enumValueIndex = value;
                }
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(property, new GUIContent(displayName, toolTip), true, Array.Empty<GUILayoutOption>());
                changed = EditorGUI.EndChangeCheck();
            }

            serializedObject.ApplyModifiedProperties();

            return changed;
        }

        protected bool DrawToggleButton(GUIContent content, bool value, Color toggledColor, float width = -1.0f)
        {
            if (value)
            {
                GUI.backgroundColor = toggledColor;
            }
            else
            {
                GUI.backgroundColor = Color.white;
            }

            GUILayoutOption[] options;

            if (width > 0.0f)
            {
                options = new[] { GUILayout.Width(width) };
            }
            else if (width == 0.0f)
            {
                options = new[] { GUILayout.ExpandWidth(false) };
            }
            else
            {
                options = new GUILayoutOption[] { };
            }

            bool clicked = GUILayout.Button(content, options);

            GUI.backgroundColor = Color.white;

            return clicked;
        }

        protected bool DrawQuaternionProperty(SerializedProperty property, in PropertyName nameInfo)
        {
            bool changed = false;

            Quaternion value = property.quaternionValue;

            Vector3 eulerAngles = value.eulerAngles;
            Vector3 newValue = EditorGUILayout.Vector3Field(property.displayName, eulerAngles);

            if (eulerAngles != newValue)
            {
                value.eulerAngles = newValue;

                property.quaternionValue = value;
                serializedObject.ApplyModifiedProperties();
                SceneView.RepaintAll();

                changed = true;
            }

            return changed;
        }

        public T GetPropertyValue<T>(SerializedProperty prop)
        {
            return GetPropertyValue<T>(prop, prop.serializedObject.targetObject);
        }

        public T GetPropertyValue<T>(SerializedProperty prop, UnityEngine.Object target)
        {
            if (prop == null)
            {
                return default;
            }

            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = target;

            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    var arr = (Array)obj.GetType().GetField(elementName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
                    obj = arr.GetValue(index);
                }
                else
                {
                    obj = obj.GetType().GetField(element, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
                }
            }

            return (T)obj;
        }

        public void SetPropertyValue<T>(SerializedProperty prop, T value)
        {
            for (int i = 0; i < _targetData.Targets.Length; i++)
            {
                SetPropertyValue(prop, value, _targetData.Targets[i]);
            }
        }

        public void SetPropertyValue<T>(SerializedProperty prop, T value, UnityEngine.Object target)
        {
            if (prop == null)
            {
                return;
            }

            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = target;

            var elements = path.Split('.');
            for (int i = 0; i < elements.Length; i++)
            {
                var element = elements[i];
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    var arr = (Array)obj.GetType().GetField(elementName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);

                    if (i == elements.Length - 1)
                    {
                        arr.SetValue(value, index);
                    }
                    else
                    {
                        obj = arr.GetValue(index);
                    }
                }
                else
                {
                    var field = obj.GetType().GetField(element, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (i == element.Length - 1)
                    {
                        field.SetValue(obj, value);
                    }
                    else
                    {
                        obj = field.GetValue(obj);
                    }
                }
            }
        }

        private void GetPlaneWorldSpaceTransform(ref SlicePlaneSerializable plane, UnityEngine.Object owner, out Matrix4x4 worldTrans)
        {
            var trans = ((MonoBehaviour)owner).transform.localToWorldMatrix;

            Vector3 worldPos = trans.MultiplyPoint(plane.Position);
            Quaternion worldRot = trans.rotation * plane.Rotation;

            worldTrans = Matrix4x4.TRS(worldPos, worldRot, new Vector3(plane.Scale, plane.Scale, plane.Scale));
        }

        protected void Space(float spacePixels, bool expand = true)
        {
#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(spacePixels);
#else
            GUILayoutUtility.GetRect(spacePixels, spacePixels, GUILayout.ExpandWidth(expand));
#endif
        }

        private void OnUndoRedo()
        {
            serializedObject.UpdateIfRequiredOrScript();
            RefreshTargetData();
        }
    }

#if !UNITY_2021_2_OR_NEWER
    static class Matrix4x4Extensions
    {
        public static Vector3 GetPosition(this Matrix4x4 mat)
        {
            return mat.GetColumn(3);
        }
    }
#endif
}