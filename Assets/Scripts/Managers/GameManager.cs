using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager I;
        public bool gameOver;

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
    }
}
