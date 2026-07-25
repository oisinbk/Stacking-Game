using System;
using Blocks.Channels;
using UnityEngine;

namespace Blocks
{
    public class SpawnPoint : MonoBehaviour
    {
        private void UpdateHeight(float height)
        {
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }

        private void OnEnable()
        {
            BlockIsStableEventChannel.UpdateHeight += UpdateHeight;
        }

        private void OnDisable()
        {
            BlockIsStableEventChannel.UpdateHeight -= UpdateHeight;
        }
    }
}