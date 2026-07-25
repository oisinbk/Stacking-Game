using TMPro;
using Tower_and_Camera;
using UnityEngine;

namespace Menus
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private HeightManager heightManager;
        [SerializeField] private TMP_Text scoreText;
        
        private void Update()
        {
            scoreText.text = "SCORE: " + heightManager.Score.ToString("D4");
        }
    }
}