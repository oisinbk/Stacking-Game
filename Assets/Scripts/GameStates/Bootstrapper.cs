using System.Collections.Generic;
using Blocks;
using Cysharp.Threading.Tasks;
using Pooling;
using Tower_and_Camera;
using UI;
using UnityEngine;
using Sound;

namespace GameStates
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private BlockSpawner blockSpawner;
        [SerializeField] private TimerManager gameTimer;
        [SerializeField] private HeightManager scoreManager;
        [SerializeField] private MusicManager musicManager;
        [SerializeField] private HeightGoals heightGoals;
        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private CameraMover cameraMover;

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
        public async UniTaskVoid StartGame()
        {
            pauseManager.ResumeGame();
            await UniTask.Yield(); 
            
            blockSpawner.DestroyAllBlocks();
            await UniTask.Yield();
            
            blockSpawner.StartGame();
            await UniTask.Yield();
            
            gameTimer.ResetTimer();
            await UniTask.Yield();
            
            scoreManager.ResetScore();
            await UniTask.Yield();
            
            spawnPoint.ResetPosition();
            cameraMover.ResetPosition();
            await UniTask.Yield();
            
            heightGoals.ResetGoals();
            heightGoals.GenerateNextGoal();
        }
    }
}