using System;
using TMPro;
using UnityEngine;
using Tower_and_Camera;

namespace UI
{
    public class TowerHeightText : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private HeightManager heightManager;
        
        private void Update()
        {
            scoreText.text = "Height: " + heightManager.Score;
        }
    }
}