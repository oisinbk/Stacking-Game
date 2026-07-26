using System.Collections.Generic;
using Blocks;
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
        [SerializeField] private BlockPool blockPool;
        [SerializeField] private TimerManager gameTimer;
        [SerializeField] private HeightManager scoreManager;
        [SerializeField] private MusicManager musicManager;
        [SerializeField] private HeightGoals heightGoals;

        [SerializeField] private GameObject bigMenu;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private List<GameObject> otherMenus;
        [SerializeField] private List<GameObject> otherSubMenus;
        private void Awake()
        {
            pauseManager.PauseGame();
            
            ReturnToMainMenu();
            //TODO: start the music here
        }

        public void ReturnToMainMenu()
        {
            bigMenu.SetActive(true);
            mainMenu.SetActive(true);
            foreach (var menu in otherMenus) menu.SetActive(false);
            foreach (var subMenu in otherSubMenus) subMenu.SetActive(false);
            // while (true)
            // {
            //     GameObject existingBlock = blockPool.chil<BlockPlacement>().gameObject;
            //     if (existingBlock == null) break;
            //     Destroy(existingBlock);
            // }
        }
        public void StartGame()
        {
            pauseManager.ResumeGame();
            blockSpawner.StartGame();
            gameTimer.ResetTimer();
            scoreManager.ResetScore();
            heightGoals.GenerateNextGoal();
        }
    }
}