using System.Linq;
using Managers;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player I;

    private float rotationSpeed;
    
    private Quaternion originalRotation;
    private Quaternion targetRotation;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
        } 
        else if (I != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        SetLookRotation();
    }

    private void SetLookRotation()
    {
        var nearestEnemy = GameManager.I.GetEnemies()
            .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
            .FirstOrDefault();
        
        if ( nearestEnemy != null && Vector3.Distance(transform.position, nearestEnemy.transform.position) < 12)
        {
            var enemyDirection = nearestEnemy.transform.position - transform.position;
            var rot = Quaternion.LookRotation(enemyDirection);
            var rotEuler = rot.eulerAngles;
            var rotEulerY = rotEuler.y < 10 ? rotEuler.y : rotEuler.y - 360;
            if (Mathf.Abs(rotEulerY) < 10)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime / Time.timeScale);;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime / Time.timeScale);
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime / Time.timeScale);
        }
    }
}
    