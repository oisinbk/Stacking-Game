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
        private Bootstrapper _bootstrapper;

        private void Awake()
        {
            _bootstrapper = GetComponent<Bootstrapper>();
        }

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
            _bootstrapper.ReturnToMainMenu();
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