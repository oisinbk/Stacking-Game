using System;
using Pooling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blocks.Channels
{
    public class BlockMover : MonoBehaviour
    {
        [SerializeField] private BlockPlacingChannel blockPlacingChannel;
        
        [Header("Input Actions")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference rotateAction;
        [SerializeField] private InputActionReference dropAction;

        [Header("Physics Settings")]
        [SerializeField] private float moveForce = 10f;
        [SerializeField] private float torqueForce = 5f;

        [Tooltip("lower the center of mass of the block by this factor")]
        [SerializeField] private float stabilizeFactor = 0.5f;

        private GameObject _currentBlock;
        private Rigidbody _rb;
        private bool _droppedBlock;

        //inputs
        private Vector2 _moveInput;
        private float _rotationInput;

        private void Awake()
        {
            _rb = null;
        }

        private void AssignNewBlock(GameObject newBlock)
        {
            _currentBlock = newBlock;
            _rb = _currentBlock.GetComponent<Rigidbody>();
            
            _rb.useGravity = false;
            _rb.centerOfMass = _currentBlock.transform.position;
            
            _droppedBlock = false;
        }

        private void Update()
        {
            if (_rb == null) return;
            if (dropAction.action.WasPressedThisFrame())
            {
                // Turn off the input listening for this specific block
                blockPlacingChannel.RaiseEvent();
                
                _rb.useGravity = true;
                _rb.centerOfMass = _currentBlock.transform.position + Vector3.down * stabilizeFactor;
                
                _moveInput = Vector2.zero;
                _rotationInput = 0f;
                _droppedBlock = true;
            }
            if (_droppedBlock) return;

            // Read directly from the Action Map reference
            _moveInput = moveAction.action.ReadValue<Vector2>();
            _rotationInput = rotateAction.action.ReadValue<float>();
        }

        private void FixedUpdate()
        {
            if (_rb == null) return;
            // Apply Y/Z movement
            if (_moveInput != Vector2.zero)
            {
                Vector3 force = new Vector3(_moveInput.x, _moveInput.y, 0) * moveForce;
                _rb.AddForce(force); 
            }

            // Apply X rotation
            if (_rotationInput != 0)
            {
                Vector3 torque = new Vector3(0, 0, _rotationInput) * torqueForce;
                _rb.AddTorque(torque);
            }
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
    }
}