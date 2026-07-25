using UnityEngine;
using System;

namespace Blocks.Channels
{
    [CreateAssetMenu(fileName = "BlockPlacingChannel", menuName = "Events/Block Placing Event Channel")]
    public class BlockDroppedEventChannel : ScriptableObject
    {
        public static event Action DroppedBlock;
        
        public void RaiseEvent()
        {
            DroppedBlock?.Invoke();
        }
    }
}