using System;
using UI;
using UnityEngine;

namespace Tower_and_Camera
{
    public class HeightGoals : MonoBehaviour
    {
        [SerializeField] private HeightManager heightManager;
        [SerializeField] private TimerManager timerManager;
        [SerializeField] private Goal goalPrefab;
        
        private int _currentGoalIndex = 1;
        private Goal _currentGoal;

        private void Update()
        {
            if (heightManager.TowerTop.y >= _currentGoal.transform.position.y)
            {
                timerManager.IncrementTimer();
                GenerateNextGoal();
            }
        }

        public void GenerateNextGoal()
        {
            int height = GenerateNextGoalHeight(_currentGoalIndex);
            Goal nextGoal = Instantiate(goalPrefab, transform);
            nextGoal.transform.position = _currentGoal.transform.position + Vector3.up * height;
            _currentGoal = nextGoal;
            _currentGoalIndex++;
        }
        
        private int GenerateNextGoalHeight(int x)
        {
            //just a function that made sense to me
            //x is the previous goal index
            int height = x * x / 10 + 4 * x;
            return height;
        }   
    }
}