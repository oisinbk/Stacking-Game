using UnityEngine;
using System;

namespace Blocks.Channels
{
    [CreateAssetMenu(fileName = "BlockIsStableEventChannel", menuName = "Events/Block Is Stable Event Channel")]
    public class BlockIsStableEventChannel : ScriptableObject
    {
        // Pass the block's transform or Y-position in the action payload
        public static event Action<float> UpdateHeight;

        public void RaiseEvent(float topPoint)
        {
            UpdateHeight?.Invoke(topPoint);
        }
    }
}