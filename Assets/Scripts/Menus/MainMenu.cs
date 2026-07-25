using System;
using GameStates;
using UnityEngine;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private GameObject allMenu;
        [SerializeField] private GameObject mainMenuVisuals;
        [SerializeField] private GameObject settingsVisuals;
        [SerializeField] private GameObject leaderboardVisuals;

        public void PlayButton()
        {
            Bootstrapper.Instance.StartGame();
            allMenu.SetActive(false);
        }

        public void SettingsButton()
        {
            settingsVisuals.SetActive(true);
            mainMenuVisuals.SetActive(false);
        }

        public void LeaderboardButton()
        {
            leaderboardVisuals.SetActive(true);
            mainMenuVisuals.SetActive(false);
        }

        public void QuitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        
#else
            Application.Quit();
#endif
        }
    }
}