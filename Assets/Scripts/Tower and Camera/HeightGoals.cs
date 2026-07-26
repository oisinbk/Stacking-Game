using System;
using UI;
using UnityEngine;

namespace Tower_and_Camera
{
    public class HeightGoals : MonoBehaviour
    {
        [SerializeField] private HeightManager heightManager;
        [SerializeField] private TimerManager timerManager;
        [SerializeField] private GameObject goalPrefab;
        
        private int _currentGoalIndex = 1;
        private GameObject _currentGoal;

        private void Update()
        {
            if (_currentGoal == null) return;
            if (heightManager.TowerTop.y >= _currentGoal.transform.position.y - heightManager.TowerBottom.y)
            {
                timerManager.IncrementTimer();
                GenerateNextGoal();
            }
        }

        public void GenerateNextGoal()
        {
            int height = GenerateNextGoalHeight(_currentGoalIndex);
            GameObject nextGoal = Instantiate(goalPrefab, transform);
            Vector3 prevPos = _currentGoal == null ? heightManager.TowerBottom : _currentGoal.transform.position;
            
            nextGoal.transform.position = prevPos + Vector3.up * height;
            _currentGoal = nextGoal;
            _currentGoalIndex++;
        }
        
        private int GenerateNextGoalHeight(int x)
        {
            //just a function that made sense to me
            //x is the previous goal index
            int height = x * x / 10 + 4 * x + 3;
            return height;
        }

        public void ResetGoals()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            _currentGoal = null;
            _currentGoalIndex = 1;
        }
    }
}