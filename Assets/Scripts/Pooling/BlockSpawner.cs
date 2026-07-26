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
    [Serializable]
    public class BlockData
    {
        public Mesh blockMesh;
        public Vector3 blockScale = Vector3.one;
        public Material blockMaterial;
    }
    
    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] private SpawnEventChannel spawnEventChannel;
        [SerializeField] private InputActionReference dropAction;
        
        [SerializeField] private Transform spawnLocation;
        
        [SerializeField] private List<BlockData> legalObjects;
        [SerializeField] private BlockPool blockPool;
        
        [SerializeField] private float coolDownTime = 1f;
        private float _lastSpawnTime = -1f;
        
        private bool _firstBlock = true;
        private bool _isWaitingToSpawn = false;
        
        public void StartGame()
        {
            _firstBlock = true;
            GenerateNewBlock();
        }

        private void RequestGenerateNewBlock()
        {
            if(_isWaitingToSpawn) return;

            SpawnAfterCooldownAsync().Forget();
        }
        
        //TODO: the following function is from gemini, it might be gobbldygook
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
            BlockData dataForBlock = legalObjects[randomIndex];
            
            BlockPlacement currentBlock = blockPool.Get();
            currentBlock.transform.position = spawnLocation.transform.position;

            AddComponents(dataForBlock, currentBlock);
            spawnEventChannel.RaiseEvent(currentBlock.gameObject);
            _firstBlock = false;
        }

        private void AddComponents(BlockData selectedBlock, BlockPlacement currentBlockPlacement)
        {
            if (!currentBlockPlacement.TryGetComponent(out GroundCollisionDetection groundCollision))
            {
                groundCollision = currentBlockPlacement.gameObject.AddComponent<GroundCollisionDetection>();
            }
            groundCollision.SetProperties(_firstBlock);
            
            if (!currentBlockPlacement.TryGetComponent(out MeshFilter meshFilter))
            {
                meshFilter = currentBlockPlacement.gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.sharedMesh = selectedBlock.blockMesh;

            if (!currentBlockPlacement.TryGetComponent(out MeshRenderer meshRenderer))
            {
                meshRenderer = currentBlockPlacement.gameObject.AddComponent<MeshRenderer>();
            }
            meshRenderer.sharedMaterial = selectedBlock.blockMaterial;
            
            if (!currentBlockPlacement.TryGetComponent(out MeshCollider meshCollider))
            {
                meshCollider = currentBlockPlacement.gameObject.AddComponent<MeshCollider>();
            }
            
            Vector3 appliedScale = selectedBlock.blockScale;
            if (appliedScale.x == 0) appliedScale.x = 1f;
            if (appliedScale.y == 0) appliedScale.y = 1f;
            if (appliedScale.z == 0) appliedScale.z = 1f;
            
            currentBlockPlacement.transform.localScale = appliedScale;
            meshCollider.sharedMesh = selectedBlock.blockMesh;
            meshCollider.convex = true;
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