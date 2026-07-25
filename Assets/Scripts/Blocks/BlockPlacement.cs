using UnityEngine;
using System;
using Pooling;
using Blocks.Channels;

namespace Blocks
{
    [RequireComponent(typeof(MeshCollider), typeof(Rigidbody), typeof(GroundCollisionDetection))]
    public class BlockPlacement : MonoBehaviour, IPoolable
    {
        [SerializeField] private BlockIsStableEventChannel blockIsStableEventChannel;

        private bool _blockIsStationary;
        public bool BlockIsStationary => _blockIsStationary;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Block") && !_blockIsStationary)
            {
                BlockPlaced();
                _blockIsStationary = true;
            }
        }

        private void Update()
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }

        private void BlockPlaced()
        {
            //if player is still holding the mouse return
            //if the block is moving too much return

            blockIsStableEventChannel.RaiseEvent(transform.position.y);
            //TODO: update the most top point of the block instead of the center of it
        }

        public void Reset()
        {
            transform.position = Vector3.zero;
        }
    }
}
