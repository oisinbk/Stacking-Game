using UnityEngine;
using System;

namespace Blocks.Channels
{
    [CreateAssetMenu(fileName = "BlockPlacingChannel", menuName = "Events/Block Placing Event Channel")]
    public class BlockPlacingChannel : ScriptableObject
    {
        public static event Action SpawnNewBlock;
        
        public void RaiseEvent()
        {
            SpawnNewBlock?.Invoke();
        }
    }
}