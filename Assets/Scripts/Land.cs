using System.Collections;
using UnityEngine;

public class Land : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Untagged")) StartCoroutine(ToKinematic(collision.gameObject)); 
    }

    private static IEnumerator ToKinematic(GameObject col)
    {
        var rb = col.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(1.5f);
        try
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
        }
        catch
        {
            // ignored
        }
    }
}
