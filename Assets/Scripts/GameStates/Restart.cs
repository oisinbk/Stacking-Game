using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace GameStates
{
    public class Restart : MonoBehaviour
    {
        [SerializeField] private InputActionReference restartAction;
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private GameObject GameOverMenu;

        private void Update()
        {
            if (restartAction.action.triggered)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void GameOver()
        {
            pauseManager.PauseGame();
            GameOverMenu.SetActive(true);
        }
        
        private void OnEnable()
        {
            GameOverEventChannel.GameOver += GameOver;
        }

        private void OnDisable()
        {
            GameOverEventChannel.GameOver -= GameOver;
        }
    }
}