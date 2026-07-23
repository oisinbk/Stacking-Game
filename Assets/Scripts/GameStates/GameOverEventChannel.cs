using UnityEngine;
using System;

namespace GameStates
{
    [CreateAssetMenu(fileName = "GameOverEventChannel", menuName = "Events/Game Over Event Channel")]
    public class GameOverEventChannel : ScriptableObject
    {
        // Pass the block's transform or Y-position in the action payload
        public event Action GameOver;

        public void RaiseEvent()
        {
            GameOver?.Invoke();
        }
    }
}