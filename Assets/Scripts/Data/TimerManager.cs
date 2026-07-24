using System;
using UnityEngine;

namespace Data
{
    public class TimerManager : MonoBehaviour, ITimer
    {
        [SerializeField] private int startTime = 20;
        
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
            _timer -= Time.deltaTime;
        }
    }
}