using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using Blocks;
using Blocks.Channels;
using UnityEngine.InputSystem;

namespace Pooling
{
    // [Serializable]
    // public class BlockData
    // {
    //     public Mesh blockMesh;
    //     public Material blockMaterial;
    // }
    
    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] private SpawnEventChannel spawnEventChannel;
        [SerializeField] private InputActionReference dropAction;
        
        [SerializeField] private Transform spawnLocation;
        
        [SerializeField] private List<GameObject> legalObjects;
        
        [SerializeField] private float coolDownTime = 1f;
        private float _lastSpawnTime = -1f;
        
        private bool _firstBlock = true;
        private bool _isWaitingToSpawn = false;
        
        public void StartGame()
        {
            _firstBlock = true;
            GenerateNewBlock();
        }

        public void DestroyAllBlocks()
        {
            // Destroys all blocks that we parented to this spawner
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        
        private void RequestGenerateNewBlock(GameObject prevBlock)
        {
            if(_isWaitingToSpawn) return;

            SpawnAfterCooldownAsync().Forget();
        }
        
        private async UniTaskVoid SpawnAfterCooldownAsync()
        {
            _isWaitingToSpawn = true;

            float timeSinceLastSpawn = Time.unscaledTime - _lastSpawnTime;
            
            // If the cooldown hasn't finished, calculate the remaining time and await it.
            if (timeSinceLastSpawn < coolDownTime)
            {
                float waitTime = coolDownTime - timeSinceLastSpawn;
                
                // ignoreTimeScale keeps it aligned with your Time.unscaledTime logic.
                // The cancellation token ensures we don't try to spawn if the object is destroyed while waiting.
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime), 
                    ignoreTimeScale: true, 
                    cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            
            GenerateNewBlock();
            _lastSpawnTime = Time.unscaledTime;
            _isWaitingToSpawn = false;
        }

        private void GenerateNewBlock()
        {
            Debug.Log("Generating new block");
            if (legalObjects == null || legalObjects.Count == 0) return;
            
            int randomIndex = UnityEngine.Random.Range(0, legalObjects.Count);
            
            GameObject currentBlock = Instantiate(legalObjects[randomIndex], transform);
            currentBlock.transform.position = spawnLocation.transform.position;

            AddComponents(currentBlock);
            currentBlock.SetActive(true);
            spawnEventChannel.RaiseEvent(currentBlock.gameObject);
            _firstBlock = false;
        }

        private void AddComponents(GameObject currentBlock)
        {
            if (!currentBlock.TryGetComponent(out GroundCollisionDetection groundCollision))
            {
                groundCollision = currentBlock.gameObject.AddComponent<GroundCollisionDetection>();
            }
            groundCollision.SetProperties(_firstBlock);
        }
        
        private void OnEnable()
        {
            BlockDroppedEventChannel.DroppedBlock += RequestGenerateNewBlock;
        }

        private void OnDisable()
        {
            BlockDroppedEventChannel.DroppedBlock -= RequestGenerateNewBlock;
        }

        private void OnDrawGizmos()
        {
            Vector3 spawnPoint = spawnLocation.position;
            
            Color originalColor = Gizmos.color;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint, 0.5f);
            
            //reset for additional gizmo drawing
            Gizmos.color = originalColor;
        }
    }
}