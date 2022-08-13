using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject next;
        [SerializeField] private GameObject wall;
        
        public static GameManager I;
        public bool pause;
        public bool gameOver;
        public bool clear;
        private List<GameObject> enemies = new();

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
                gameObject => gameObject.GetComponentInChildren(typeof(Enemy))));
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
            while (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }

            if (GetEnemiesCount() == 0)
            {
                StartCoroutine(WallToNext());
            }
        }

        private IEnumerator WallToNext()
        {
            var material = wall.GetComponent<MeshRenderer>().material;
            Debug.Log("[+] " + material.GetColor("_EmissionColor"));
            var i = 0;
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                material.SetColor("_EmissionColor", material.GetColor("_EmissionColor") * 0.975f);
                i++;
                if (i == 30) break;
            }
            Debug.Log("[-] " + material.GetColor("_EmissionColor"));
            
            wall.SetActive(false);
            next.SetActive(true);
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
    }
}
