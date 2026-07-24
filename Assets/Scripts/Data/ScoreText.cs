using System;
using TMPro;
using UnityEngine;

namespace Data
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private HeightManager heightManager;
        
        private void Update()
        {
            scoreText.text = "Height: " + heightManager.Score;
        }
    }
}