

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTrailRenderer : MonoBehaviour {

    [HideInInspector]public Mesh BakedMeshResult; //출력하고 싶은 타겟 스킨 렌더 (캐릭터)
    [HideInInspector]public string ValueName;
    [HideInInspector]public float ValueDetail;
    [HideInInspector]public float ValueTimeDelay;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine("MaterialColorAnimation");
    }

    IEnumerator MaterialColorAnimation()
    {
        for (float e = 0; e < 1.1; e += ValueDetail) //X번 반복한다.
        {
            this.GetComponent<MeshRenderer>().material.SetFloat(ValueName, e);
            yield return new WaitForSeconds(ValueTimeDelay); //X초마다 반복
        }

        //this.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.SetActive(false);
    }
}
