using System;
using Pooling;
using Tower_and_Camera;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blocks.Channels
{
    public class BlockMover : MonoBehaviour
    {
        [SerializeField] private BlockDroppedEventChannel blockDroppedEventChannel;
        
        [Header("Input Actions")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference rotateAction;
        [SerializeField] private InputActionReference dropAction;

        [Header("Movement Settings")]
        [SerializeField] private float maxMoveSpeed = 10f;
        [Tooltip("How fast the block reaches max speed and stops. Higher = less sliding.")]
        [SerializeField] private float moveSnappiness = 15f; 

        [Header("Rotation Settings")]
        [SerializeField] private float maxRotationSpeed = 200f;
        [Tooltip("How fast the block reaches max rotation and stops.")]
        [SerializeField] private float rotationSnappiness = 15f;

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
            // Apply X/Y movement
            Vector3 targetVelocity = new Vector3(_moveInput.x, _moveInput.y, 0f) * maxMoveSpeed;
            Vector3 velocityChange = targetVelocity - _rb.linearVelocity;
            
            float moveRate = Time.fixedDeltaTime / Mathf.Max(Time.fixedDeltaTime, moveSnappiness);

            _rb.AddForce(velocityChange * moveRate, ForceMode.VelocityChange);
        }

        private void RotateBLock()
        {
            // Apply Z rotation
            Vector3 targetAngularVelocity = new Vector3(0f, 0f, _rotationInput * maxRotationSpeed);
            Vector3 angularVelocityDifference = targetAngularVelocity - _rb.angularVelocity;
            
            float rotRate = Time.fixedDeltaTime / Mathf.Max(Time.fixedDeltaTime, rotationSnappiness);

            _rb.AddTorque(angularVelocityDifference * rotRate, ForceMode.VelocityChange);
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