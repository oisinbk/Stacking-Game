using System;
using Blocks.Channels;
using Tower_and_Camera;
using UnityEngine;

namespace Blocks
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private HeightManager heightManager;
        [SerializeField] private float heightBuffer = 2f;
        private Vector3 _originalPosition;

        private void Start()
        {
            _originalPosition = transform.position;
        }

        private void UpdateHeight(GameObject prevBlock)
        {
            //Vector3.Lerp(transform.position, _originalPosition + heightManager.TowerTop, 1);
            Vector3 option1 = prevBlock.transform.position + Vector3.up * heightBuffer;
            Vector3 option2 = _originalPosition + heightManager.TowerTop;
            Vector3 newPos = Vector3.Max(option1, option2);
            
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
            BlockDroppedEventChannel.DroppedBlock += UpdateHeight;
        }

        private void OnDisable()
        {
            BlockDroppedEventChannel.DroppedBlock -= UpdateHeight;
        }
    }
}