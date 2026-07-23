using UnityEngine;
using System;
using Pooling;

namespace Blocks
{
    [RequireComponent(typeof(MeshCollider), typeof(Rigidbody))]
    public class BlockPlacement : MonoBehaviour, IPoolable
    {
        [SerializeField] private BlockPlacingChannel blockPlacingChannel;
        
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

        private void BlockPlaced()
        {
            //if player is still holding the mouse return
            //if the block is moving too much return

            blockPlacingChannel.RaiseEvent(transform.position.y);
            //TODO: update the most top point of the block instead of the center of it
        }

        public void Reset()
        {
            transform.position = Vector3.zero;
        }
    }
}
