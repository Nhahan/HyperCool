using UnityEngine;

namespace Managers
{
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField] public GameObject hitParticles;
    
        public static ParticleManager I;

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
    }
}
