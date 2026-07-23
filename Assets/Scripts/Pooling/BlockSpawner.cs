using System;
using UnityEngine;
using System.Collections.Generic;
using Blocks;

namespace Pooling
{
    [Serializable]
    public struct BlockData
    {
        public MeshFilter meshFilter;
        public Material material;
    }
    
    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] private Transform mainCamera;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0, 1f, 1f);
        [SerializeField] private BlockPlacingChannel blockPlacingChannel;
        
        [SerializeField] private List<BlockData> legalObjects;
        [SerializeField] private BlockPool blockPool;

        private void Start()
        {
            GenerateNewBlock(0f);
        }

        private void GenerateNewBlock(float topPoint)
        {
            if (legalObjects == null || legalObjects.Count == 0) return;
            int randomIndex = UnityEngine.Random.Range(0, legalObjects.Count);
            BlockData selectedBlock = legalObjects[randomIndex];
            
            BlockPlacement currentBlockPlacement = blockPool.Get();
            currentBlockPlacement.transform.position = mainCamera.position + cameraOffset;
            
            if (currentBlockPlacement.TryGetComponent<MeshFilter>(out var filter))
            {
                filter.sharedMesh = selectedBlock.meshFilter.mesh;
            }

            if (currentBlockPlacement.TryGetComponent<MeshRenderer>(out var renderer))
            {
                renderer.sharedMaterial = selectedBlock.material;
            }

            if (!currentBlockPlacement.TryGetComponent<MeshCollider>(out var collider))
            {
                collider = currentBlockPlacement.gameObject.AddComponent<MeshCollider>();
            }
            
            collider.sharedMesh = selectedBlock.meshFilter.sharedMesh;
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