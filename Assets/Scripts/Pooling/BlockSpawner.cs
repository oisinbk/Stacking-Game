using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Pooling
{
    [Serializable]
    public struct BlockData
    {
        public Mesh mesh;
        public Material material;
    }
    
    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] private Transform mainCamera;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0, 1f, 1f);
        [SerializeField] private BlockPlacingChannel blockPlacingChannel;
        
        [SerializeField] private List<BlockData> legalObjects;
        [SerializeField] private BlockPool blockPool;

        private void GenerateNewBlock(float topPoint)
        {
            if (legalObjects == null || legalObjects.Count == 0) return;
            int randomIndex = UnityEngine.Random.Range(0, legalObjects.Count);
            BlockData selectedBlock = legalObjects[randomIndex];
            
            Block currentBlock = blockPool.Get();
            currentBlock.transform.position = mainCamera.position + cameraOffset;
            
            if (currentBlock.TryGetComponent<MeshFilter>(out var filter))
            {
                filter.sharedMesh = selectedBlock.mesh;
            }

            if (currentBlock.TryGetComponent<MeshRenderer>(out var renderer))
            {
                renderer.sharedMaterial = selectedBlock.material;
            }

            if (!currentBlock.TryGetComponent<MeshCollider>(out var collider))
            {
                collider = currentBlock.gameObject.AddComponent<MeshCollider>();
            }
            
            collider.sharedMesh = selectedBlock.mesh;
            collider.convex = true;
        }
        
        private void OnEnable()
        {
            blockPlacingChannel.BlockPlaced += GenerateNewBlock;
        }

        private void OnDisable()
        {
            blockPlacingChannel.BlockPlaced -= GenerateNewBlock;
        }

        private void OnDrawGizmos()
        {
            Vector3 spawnPoint = mainCamera.position + cameraOffset;
            
            Color originalColor = Gizmos.color;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint, 0.5f);
            
            //reset for additional gizmo drawing
            Gizmos.color = originalColor;
        }
    }
}