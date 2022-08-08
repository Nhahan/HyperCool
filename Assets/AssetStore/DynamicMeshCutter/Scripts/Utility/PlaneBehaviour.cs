
using System.Collections.Generic;
using UnityEngine;

namespace DynamicMeshCutter
{
    public class PlaneBehaviour : CutterBehaviour
    {
        public float DebugPlaneLength = 2;
        public void Cut()
        {
            var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            
            foreach (var root in roots)
            {
                if (!root.activeInHierarchy)
                    continue;
                var targets = root.GetComponentsInChildren<MeshTarget>();
                foreach (var target in targets)
                {
                    if (!target.CompareTag("Enemy")) continue;
                    if (Vector3.Distance(Player.I.transform.position, target.transform.position) < 4)
                    {
                        Instantiate(ParticleManager.I.hitParticles, target.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                        Cut(target, transform.position, transform.forward, null, OnCreated);
                        target.tag = "Dead";
                    }
                }
            }
        }

        private void OnCreated(Info info, MeshCreationData cData)
        {
            var meshes = MeshCreation.TranslateCreatedObjects(info, cData.CreatedObjects, cData.CreatedTargets, Separation);
            meshes.ForEach(mesh =>
            {
                Destroy(mesh, 2f);
            });
        }
    }
}