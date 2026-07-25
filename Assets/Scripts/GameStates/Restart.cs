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
        
        private void OnEnable()
        {
            GameOverEventChannel.GameOver += RestartGame;
        }

        private void OnDisable()
        {
            GameOverEventChannel.GameOver -= RestartGame;
        }
    }
}