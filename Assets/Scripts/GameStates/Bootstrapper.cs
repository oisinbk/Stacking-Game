using System.Collections.Generic;
using Menus;
using Pooling;
using Tower_and_Camera;
using UI;
using UnityEngine;
using Sound;

namespace GameStates
{
    public class Bootstrapper : MonoSingleton<Bootstrapper>
    {
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private BlockSpawner blockSpawner;
        [SerializeField] private TimerManager gameTimer;
        [SerializeField] private HeightManager scoreManager;
        [SerializeField] private MusicManager musicManager;

        [SerializeField] private GameObject bigMenu;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private List<GameObject> otherMenus;
        private void Awake()
        {
            pauseManager.PauseGame();
            
            bigMenu.SetActive(true);
            mainMenu.SetActive(true);
            foreach (var menu in otherMenus) menu.SetActive(false);
            
            //TODO: start the music here
        }

        public void StartGame()
        {
            pauseManager.ResumeGame();
            blockSpawner.StartGame();
            gameTimer.ResetTimer();
            scoreManager.ResetScore();
        }
    }
}