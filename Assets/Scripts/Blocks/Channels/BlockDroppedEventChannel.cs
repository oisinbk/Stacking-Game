using UnityEngine;
using System;

namespace Blocks.Channels
{
    [CreateAssetMenu(fileName = "BlockPlacingChannel", menuName = "Events/Block Placing Event Channel")]
    public class BlockDroppedEventChannel : ScriptableObject
    {
        public static event Action<GameObject> DroppedBlock;
        
        public void RaiseEvent(GameObject prevBlock)
        {
            DroppedBlock?.Invoke(prevBlock);
        }
    }
}