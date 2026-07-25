using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace Menus
{
    public class LeaderBoardEntry : MonoBehaviour
    {
        public void Setup(int index, string playerName, int score)
        {
            indexText.text = index.ToString();
            playerNameText.text = playerName;
            scoreText.text = score.ToString("D4");
        }

        public void IncrementIndex()
        {
            indexText.text = (int.Parse(indexText.text) + 1).ToString();
        }

        public void DecrementIndex()
        {
            indexText.text = (int.Parse(indexText.text) - 1).ToString();
        }
        
        [SerializeField] private TMP_Text indexText;
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text scoreText;
    }
}