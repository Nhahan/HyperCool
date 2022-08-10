using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager I;
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void RemoveEnemyFromList(GameObject enemy)
        {
            enemies.Remove(enemy);
        }
    }
}
