using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager I;
        public bool pause;
        public bool gameOver;
        public List<GameObject> enemies = new();

        private void Awake()
        {
            Application.targetFrameRate = 60;
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
            StartCoroutine(Pause(true));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("Resume");
                pause = false;
            }
        }

        public void RemoveEnemyFromList(GameObject enemy)
        {
            enemies.Remove(enemy);
        }

        private IEnumerator Pause(bool b)
        {
            yield return new WaitForSeconds(0.05f);
            pause = b;
        }
    }
}
