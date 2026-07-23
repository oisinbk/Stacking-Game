using Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioObject : MonoBehaviour, IPoolable
    {
        public AudioSource audioSource;
        //TODO: wtf is going on here
        private void Awake()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }
        
        public void Reset()
        {
            transform.position = Vector3.zero;
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
        }
    }
}