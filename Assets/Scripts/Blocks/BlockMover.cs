using Pooling;
using UnityEngine;
using UnityEngine.InputSystem;
using Blocks.Channels;
using DebugTools;

namespace Blocks
{
    public class BlockMover : MonoBehaviour
    {
        [SerializeField] private BlockDroppedEventChannel blockDroppedEventChannel;
        
        [Header("Input Actions")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference rotateAction;
        [SerializeField] private InputActionReference dropAction;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float moveStartDelay = 0.2f;
        [SerializeField] private float moveEndDelay = 0.2f;

        [Header("Rotation Settings")]
        [SerializeField] private float rotSpeed = 5f;
        [SerializeField] private float dropOffFactor = 0.5f;

        [Header("Tower Stabilizers")]
        [Tooltip("lower the center of mass of the block by this factor")]
        [SerializeField] private float stabilizeFactor = 0.5f;
        [SerializeField] private float newMass = 20f;
        
        [Header("Block Movement Limiters")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float maxXValues = 5f;
        [SerializeField] private float maxHeight = 10f;
        
        private GameObject _currentBlock;
        private Rigidbody _rb;
        private bool _droppedBlock;

        //inputs
        private Vector2 _moveInput;
        private float _rotationInput;

        //movement limiter
        private float _maxActualHeight;
        
        private void Awake()
        {
            _rb = null;
        }

        private void AssignNewBlock(GameObject newBlock)
        {
            _currentBlock = newBlock;
            _rb = _currentBlock.GetComponent<Rigidbody>();
            
            _rb.useGravity = false;
            _rb.centerOfMass = Vector3.zero; //this is calculated in local space
            
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            
            _droppedBlock = false;
        }

        private void Update()
        {
            if (_rb == null || _droppedBlock) return;
            if (dropAction.action.WasPressedThisFrame())
            {
                DropBlock();
            }

            _maxActualHeight = spawnPoint.position.y + maxHeight;
            
            // Read directly from the Action Map reference
            _moveInput = moveAction.action.ReadValue<Vector2>();
            _rotationInput = rotateAction.action.ReadValue<float>();
        }

        private void FixedUpdate()
        {
            if (_rb == null) return;
            MoveBlock();
            RotateBLock();
            
            ClampBlockPosition();
        }
        
        private void DropBlock()
        {
            // #region agent log
            AgentDebugLog.Write(
                "BlockMover.cs:DropBlock",
                "Player dropped block (Space)",
                "D",
                dataJson: $"{{\"blockName\":\"{_currentBlock?.name ?? "null"}\"}}");
            // #endregion

            _rb.useGravity = true;
            _rb.centerOfMass = Vector3.down * stabilizeFactor; //this is calculated in local space
            _rb.mass = newMass;
            
            _moveInput = Vector2.zero; 
            _rotationInput = 0f;
            _droppedBlock = true;
                
            // Turn off the input listening for this specific block
            blockDroppedEventChannel.RaiseEvent();
        }
        
        private void MoveBlock()
        {
            Vector3 targetVelocity = new Vector3(_moveInput.x, _moveInput.y, 0f) * moveSpeed;
            // Apply X/Y movement
            if (_moveInput != Vector2.zero)
            {
                _rb.linearVelocity = 
                    Vector3.Lerp(_rb.linearVelocity, targetVelocity, moveStartDelay * Time.fixedDeltaTime); 
            }
            else
            {
                _rb.linearVelocity = 
                    Vector3.Lerp(_rb.linearVelocity, Vector3.zero, moveEndDelay * Time.fixedDeltaTime); 
            }
        }

        private void RotateBLock()
        {
            // Apply Z rotation
            if (_rotationInput != 0)
            {
                _rb.angularVelocity = new Vector3(0, 0, _rotationInput * rotSpeed);
            }
            else
            {
                _rb.angularVelocity =
                    Vector3.Lerp(_rb.angularVelocity, Vector3.zero, dropOffFactor * Time.fixedDeltaTime);
            }
        }

        private void ClampBlockPosition()
        {
            Vector3 newPos = _rb.position;
            Vector3 linVelocity = _rb.linearVelocity;
            
            if (newPos.y > _maxActualHeight)
            {
                newPos.y = _maxActualHeight;
                if (linVelocity.y > 0f) linVelocity.y = 0f;
            }
            if (newPos.x > maxXValues)
            {
                newPos.x = maxXValues;
                if(linVelocity.x > 0f) linVelocity.x = 0f;
            }
            if(newPos.x < -maxXValues)
            {
                newPos.x = -maxXValues;
                if(linVelocity.x < 0f) linVelocity.x = 0f;
            }

            _rb.position = newPos;
            _rb.linearVelocity = linVelocity;
        }

        private void OnEnable()
        {
            SpawnEventChannel.SpawnBlock += AssignNewBlock;
            moveAction.action.Enable();
            rotateAction.action.Enable();
            dropAction.action.Enable();
        }

        private void OnDisable() 
        {
            SpawnEventChannel.SpawnBlock -= AssignNewBlock;
            moveAction.action.Disable();
            rotateAction.action.Disable();
            dropAction.action.Disable();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(new Vector3(0, spawnPoint.position.y, 0), new Vector3(maxXValues, maxHeight, 0));
        }
    }
}