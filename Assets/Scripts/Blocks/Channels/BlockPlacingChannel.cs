using UnityEngine;
using System;

namespace Blocks.Channels
{
    [CreateAssetMenu(fileName = "BlockPlacingChannel", menuName = "Events/Block Placing Event Channel")]
    public class BlockPlacingChannel : ScriptableObject
    {
        private const float CoolDownTime = 1f;
        public static event Action SpawnNewBlock;
        
        private float _lastSpawnTime = -1f;
        
        
        public void RaiseEvent()
        {
            if (Time.unscaledTime - _lastSpawnTime < CoolDownTime)
            {
                return; 
            }
            
            _lastSpawnTime = Time.unscaledTime;
            SpawnNewBlock?.Invoke();
        }
    }
}