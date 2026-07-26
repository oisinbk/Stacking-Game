using System;
using Blocks.Channels;
using Tower_and_Camera;
using UnityEngine;

namespace Blocks
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private HeightManager heightManager;
        private Vector3 _originalPosition;

        private void Start()
        {
            _originalPosition = transform.position;
        }

        private void UpdateHeight(float height)
        {
            Vector3.Lerp(transform.position, _originalPosition + heightManager.TowerTop, 1);
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