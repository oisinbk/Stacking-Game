using UnityEngine;
using UnityEngine.InputSystem;
using Blocks.Channels;

namespace Blocks
{
    [RequireComponent(typeof(BlockPlacement),
        typeof(Rigidbody))]

    public class BlockMovement : MonoBehaviour
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
        
        private Rigidbody _rb;

        //inputs
        private Vector2 _moveInput;
        private float _rotationInput;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (dropAction.action.triggered)
            {
                blockPlacingChannel.RaiseEvent();
                
                // Turn off the input listening for this specific block
                moveAction.action.Disable();
                rotateAction.action.Disable();
                _rb.useGravity = true;
                _rb.centerOfMass = transform.position + Vector3.down * stabilizeFactor;
            
                // Shut off the script so it stops taking up CPU cycles
                enabled = false; 
                return;
            }

            // Read directly from the Action Map reference
            _moveInput = moveAction.action.ReadValue<Vector2>();
            _rotationInput = rotateAction.action.ReadValue<float>();
        }

        private void FixedUpdate()
        {
            // Apply Y/Z movement
            if (_moveInput != Vector2.zero)
            {
                Vector3 force = new Vector3(0, _moveInput.y, _moveInput.x) * moveForce;
                _rb.AddForce(force); 
            }

            // Apply X rotation
            if (_rotationInput != 0)
            {
                Vector3 torque = new Vector3(_rotationInput, 0, 0) * torqueForce;
                _rb.AddTorque(torque);
            }
        }

        private void OnEnable()
        {
            moveAction.action.Enable();
            rotateAction.action.Enable();
        }

        private void OnDisable() 
        {
            moveAction.action.Disable();
            rotateAction.action.Disable();
        }
    }
}