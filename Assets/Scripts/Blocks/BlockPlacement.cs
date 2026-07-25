using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Pooling;
using Blocks.Channels;

namespace Blocks
{
    [RequireComponent(typeof(MeshCollider), typeof(Rigidbody), typeof(GroundCollisionDetection))]
    public class BlockPlacement : MonoBehaviour, IPoolable
    {
        [SerializeField] private BlockIsStableEventChannel blockIsStableEventChannel;
        
        [Header("Settle Settings")]
        [SerializeField] float requiredSettleTime = 0.5f;
        [Tooltip("minimum amount of velocity to register movement")]
        [SerializeField] float minVelocityThreshold = 0.01f;
        
        private bool _blockIsStationary;
        private bool _isAlreadyChecking;
        
        private Collider _collider;
        private Rigidbody _rb;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _rb = GetComponent<Rigidbody>();
        }
        
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Block") && !_blockIsStationary && !_isAlreadyChecking)
            {
                CheckIfSettledAsync(this.GetCancellationTokenOnDestroy()).Forget();
            }
        }

        // UniTaskVoid is the most optimized return type for a "fire and forget" task
        private async UniTaskVoid CheckIfSettledAsync(CancellationToken token)
        {
            _isAlreadyChecking = true;
            float timeStationary = 0f;

            while (timeStationary < requiredSettleTime)
            {
                bool isMoving = _rb.linearVelocity.sqrMagnitude > minVelocityThreshold;
                bool isRotating = _rb.angularVelocity.sqrMagnitude > minVelocityThreshold;

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
        }

        public void Reset()
        {
            transform.position = Vector3.zero;
        }
    }
}
