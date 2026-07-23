using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GroundObject: MonoBehaviour
    {
        [SerializeField] private GameOverEventChannel gameOverChannel;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Block") && gameOverChannel != null)
            {
                gameOverChannel.RaiseEvent(); 
            }
        }
    }
}