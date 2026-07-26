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
            //Vector3.Lerp(transform.position, _originalPosition + heightManager.TowerTop, 1);
            Vector3 newPos = _originalPosition + heightManager.TowerTop;
            if (newPos.y > _originalPosition.y)
            {
                transform.position = newPos;
            }
        }

        public void ResetPosition()
        {
            transform.position = _originalPosition;
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