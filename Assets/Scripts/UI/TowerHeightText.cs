using System;
using TMPro;
using UnityEngine;
using Tower_and_Camera;

namespace UI
{
    public class TowerHeightText : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        private HeightManager _heightManager;

        private void Awake()
        {
            _heightManager = GetComponent<HeightManager>();
        }

        private void Update()
        {
            scoreText.text = "Height: " + _heightManager.Score;
        }
    }
}