using UnityEngine;
using System;
using Pooling;

[RequireComponent(typeof(MeshCollider), typeof(Rigidbody))]
public class Block : MonoBehaviour, IPoolable
{
    [Tooltip("lower the center of mass of the block by this factor")]
    [SerializeField] private float stabilizeFactor = 0.5f;
    
    [SerializeField] private BlockPlacingChannel blockPlacingChannel;

    public static event Action GameOver;

    private Rigidbody _rb;
    private bool _stationary;
    
    private void Awake()
    {
        _rb =  GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Block") && !_stationary)
        {
            BlockPlaced();
            _stationary = true;
        }
    }

    private void BlockPlaced()
    {
        //if player is still holding the mouse return
        //if the block is moving too much return
        
        _rb.centerOfMass = transform.position + Vector3.down * stabilizeFactor;
        blockPlacingChannel.RaiseEvent(transform.position.y);
        //TODO: update the most top point of the block instead of the center of it
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
    }
}