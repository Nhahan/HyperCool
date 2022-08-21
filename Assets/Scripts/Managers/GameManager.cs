using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject nextStage;
        [SerializeField] private GameObject gameOverUI;

        public static GameManager I;
        public bool pause;
        public bool gameOver;
        public bool clear;
        private readonly List<GameObject> enemies = new();

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
            enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy").ToList().FindAll(
                o => o.GetComponentInChildren(typeof(Enemy))));
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
                pause = false;
                Debug.Log(pause + " / " + gameOver + " / " + clear);
            }
        }

        public void RemoveEnemyFromList(GameObject enemy)
        {
            while (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }

            if (GetEnemiesCount() == 0)
            {
                StartCoroutine(NextStage());
            }
        }

        private IEnumerator NextStage()
        {
            yield return new WaitForSeconds(1.5f);
            clear = true;
            nextStage.SetActive(true);
        }

        private IEnumerator Pause(bool b)
        {
            yield return new WaitForSeconds(0.001f);
            pause = b;
        }

        public int GetEnemiesCount()
        {
            return enemies.Count;
        }

        public List<GameObject> GetEnemies()
        {
            return enemies;
        }

        public void GameOver()
        {
            gameOver = true;
            gameOverUI.SetActive(true);
        }
    }
}
