using System;
using GameStates;
using UnityEngine;

namespace UI
{
    public class TimerManager : MonoBehaviour, ITimer
    {
        [SerializeField] private int startTime = 20;
        [SerializeField] private int incrementedTime = 5;
        [SerializeField] private GameOverEventChannel gameOverEventChannel;
        
        public float Timer => _timer;
        private float _timer;

        public void ResetTimer()
        {
            Time.timeScale = 1;
            _timer = startTime;
        }

        public void PauseTimer()
        {
            Time.timeScale = 0;
        }

        public void ResumeTimer()
        {
            Time.timeScale = 1;
        }

        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                gameOverEventChannel.RaiseEvent();
            }
        }

        public void IncrementTimer()
        {
            _timer += incrementedTime;
        }
    }
}