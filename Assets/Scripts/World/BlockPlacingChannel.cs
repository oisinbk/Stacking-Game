using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BlockPlacingChannel", menuName = "Events/Block Placing Channel")]
public class BlockPlacingChannel : ScriptableObject
{
    // Pass the block's transform or Y-position in the action payload
    public event Action<float> BlockPlaced;

    public void RaiseEvent(float topPoint)
    {
        BlockPlaced?.Invoke(topPoint);
    }
}