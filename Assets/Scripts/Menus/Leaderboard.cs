using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Menus
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private LeaderBoardEntry entryPrefab;
        [SerializeField] private int length = 10;

        private List<LeaderBoardEntry> _leaderboard = new  List<LeaderBoardEntry>();

        private void Start()
        {
            for (int i = 0; i < length; i++)
            {
                LeaderBoardEntry listEntry = Instantiate(entryPrefab, transform);
                listEntry.Setup(i, "Player-Name", 0);
                
                _leaderboard.Add(listEntry);
            }
        }

        public void InsertScoreAtPosition(int targetPosition, string newName, int newScore)
        {
            LeaderBoardEntry newEntry = Instantiate(entryPrefab, transform);
            newEntry.transform.SetSiblingIndex(targetPosition);
            newEntry.Setup(targetPosition, newName, newScore);
            
            _leaderboard.Insert(targetPosition, newEntry);

            int last = _leaderboard.Count - 1;
            LeaderBoardEntry lastEntry = _leaderboard[last];
            _leaderboard.RemoveAt(last);
            Destroy(lastEntry.gameObject);
            
            // Update the indices
            for (int i = targetPosition + 1; i < _leaderboard.Count; i++)
            {
                _leaderboard[i].IncrementIndex();
            }
        }
        
        public void DeleteScoreAtPosition(int targetPosition)
        {
            // Remove the entry at the given position
            LeaderBoardEntry entryToDelete = _leaderboard[targetPosition];
            _leaderboard.RemoveAt(targetPosition);
            Destroy(entryToDelete.gameObject);

            // Update the indices for everything that shifted up
            for (int i = targetPosition; i < _leaderboard.Count; i++)
            {
                _leaderboard[i].DecrementIndex();
            }
        }
    }
}