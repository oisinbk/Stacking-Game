using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameStates
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] InputActionReference pause;
        [SerializeField] List<InputActionReference> allPlayerInputs;
        [SerializeField] private List<GameObject> inGameUIElements;
        [SerializeField] private GameObject pauseMenuVisuals;
        private bool _gameIsPaused;

        private void Update()
        {
            if (pause.action.triggered)
            {
                _gameIsPaused = !_gameIsPaused;
                pauseMenuVisuals.SetActive(_gameIsPaused);
                PauseUnpauseGame(_gameIsPaused);
            }
        }
        
        public void PauseGame()
        {
            _gameIsPaused = true;
            PauseUnpauseGame(true);
        }

        public void ResumeGame()
        {
            _gameIsPaused = false;
            PauseUnpauseGame(false);
        }

        private void PauseUnpauseGame(bool paused)
        {
            Time.timeScale = paused ? 0 : 1;
            foreach (var input in allPlayerInputs)
            {
                if (paused)
                {
                    input.action.Disable();
                }
                else
                {
                    input.action.Enable();
                }
            }
            foreach (var element in inGameUIElements)
            {
                element.SetActive(!paused);
            }
        }
        
        private void OnEnable()
        {
            pause.action.Enable();
        }

        private void OnDisable()
        {
            pause.action.Disable();
        }
    }
}