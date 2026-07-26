using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Pooling;
using Blocks.Channels;
using Sound;
using System.Collections.Generic;

namespace Blocks
{
    [RequireComponent(typeof(Rigidbody), typeof(GroundCollisionDetection))]
    public class BlockPlacement : MonoBehaviour, IPoolable
    {
        [SerializeField] private BlockIsStableEventChannel blockIsStableEventChannel;
        
        [Header("Settle Settings")]
        [SerializeField] float requiredSettleTime = 0.2f;
        [Tooltip("minimum amount of velocity to register movement")]
        [SerializeField] float minVelocityThreshold = 0.01f;
        
        [SerializeField] List<AudioClip> blockSounds;
        
        private bool _blockIsStationary;
        private bool _isAlreadyChecking;
        
        private Collider _collider;
        private Rigidbody _rb;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _rb = GetComponent<Rigidbody>();
            
            //higher precision rb calculations
            _rb.sleepThreshold = 0.01f;
        }
        
        
        private void OnCollisionEnter(Collision other)
        {
            SoundFXManager.Instance.PlayRandomSoundFXClip(blockSounds, transform, 1);
            if ((other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Stage"))
                && !_blockIsStationary
                && !_isAlreadyChecking)
            {
                CheckIfSettledAsync(this.GetCancellationTokenOnDestroy()).Forget();
            }
        }

        // UniTaskVoid is the most optimized return type for a "fire and forget" task
        private async UniTaskVoid CheckIfSettledAsync(CancellationToken token)
        {
            _isAlreadyChecking = true;
            float timeStationary = 0f;

            float sqrThreshold = minVelocityThreshold * minVelocityThreshold;
            
            while (timeStationary < requiredSettleTime)
            {
                bool isMoving = _rb.linearVelocity.sqrMagnitude > sqrThreshold;
                bool isRotating = _rb.angularVelocity.sqrMagnitude > sqrThreshold;

                if (!isMoving && !isRotating)
                {
                    timeStationary += Time.fixedDeltaTime;
                }
                else
                {
                    timeStationary = 0f;
                }

                await UniTask.WaitForFixedUpdate(cancellationToken: token);
            }

            _blockIsStationary = true;
            _isAlreadyChecking = false;
        
            BlockPlaced();
        }

        private void BlockPlaced()
        {
            blockIsStableEventChannel.RaiseEvent(_collider.bounds.max.y);
            
            //lower precision rb calculations
            _rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            _rb.solverIterations = 6;
            _rb.solverVelocityIterations = 1;
        }

        public void Reset()
        {
            transform.position = Vector3.zero;
            
            // Reset state flags
            _blockIsStationary = false;
            _isAlreadyChecking = false;
            
            // Reset velocities so it doesn't carry over old momentum from its previous life
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            
            // Higher precision rb calculations for the fresh drop
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rb.solverIterations = 20;
            _rb.solverVelocityIterations = 10;
        }
    }
}
