using UnityEngine;
using System;

namespace Pooling
{
    [CreateAssetMenu(fileName = "SpawnEventChannel", menuName = "Events/Spawn Block Event Channel")]
    public class SpawnEventChannel : ScriptableObject
    {
        public static event Action<GameObject> SpawnBlock;
        
        public void RaiseEvent(GameObject block)
        {
            SpawnBlock?.Invoke(block);
        }
    }
}