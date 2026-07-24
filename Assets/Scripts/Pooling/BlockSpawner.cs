using System;
using UnityEngine;
using System.Collections.Generic;
using Blocks;
using Blocks.Channels;

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
        [SerializeField] private Transform mainCamera;
        [SerializeField] private float cameraYOffset = 1f;
        [SerializeField] private BlockIsStableEventChannel blockIsStableEventChannel;
        
        [SerializeField] private List<BlockData> legalObjects;
        [SerializeField] private BlockPool blockPool;

        private bool _firstBlock = true;
        
        private void Start()
        {
            _firstBlock = true;
            GenerateNewBlock();
            _firstBlock = false;
        }

        private void GenerateNewBlock()
        {
            if (legalObjects == null || legalObjects.Count == 0) return;
            int randomIndex = UnityEngine.Random.Range(0, legalObjects.Count);
            BlockData selectedBlock = legalObjects[randomIndex];
            
            BlockPlacement currentBlockPlacement = blockPool.Get();
            currentBlockPlacement.transform.position = new Vector3(0, mainCamera.position.y + cameraYOffset, 0);

            AddComponents(selectedBlock, currentBlockPlacement);
        }

        private void AddComponents(BlockData selectedBlock, BlockPlacement currentBlockPlacement)
        {
            if (!currentBlockPlacement.TryGetComponent<MeshFilter>(out var meshFilter))
            {
                meshFilter = currentBlockPlacement.gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.sharedMesh = selectedBlock.blockMesh;

            if (!currentBlockPlacement.TryGetComponent<MeshRenderer>(out var meshRenderer))
            {
                meshRenderer = currentBlockPlacement.gameObject.AddComponent<MeshRenderer>();
            }
            meshRenderer.sharedMaterial = selectedBlock.blockMaterial;
            
            if (!currentBlockPlacement.TryGetComponent<MeshCollider>(out var meshCollider))
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
            BlockPlacingChannel.SpawnNewBlock += GenerateNewBlock;
        }

        private void OnDisable()
        {
            BlockPlacingChannel.SpawnNewBlock -= GenerateNewBlock;
        }

        private void OnDrawGizmos()
        {
            Vector3 spawnPoint = new Vector3(0, mainCamera.position.y + cameraYOffset, 0);
            
            Color originalColor = Gizmos.color;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint, 0.5f);
            
            //reset for additional gizmo drawing
            Gizmos.color = originalColor;
        }
    }
}