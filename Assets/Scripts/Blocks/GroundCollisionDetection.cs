using System;
using UnityEngine;
using GameStates;

namespace Blocks
{
    public class GroundCollisionDetection: MonoBehaviour
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