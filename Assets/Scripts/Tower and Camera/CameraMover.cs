using System;
using UnityEngine;
using UI;

namespace Tower_and_Camera
{
    [RequireComponent(typeof(HeightManager))]
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        
        private HeightManager _heightManager;
        private Vector3 _originalPosition;
        
        private void Awake()
        {
            _heightManager = GetComponent<HeightManager>();
            _originalPosition = mainCamera.transform.position;
        }

        private void Update()
        {
            mainCamera.transform.position = _originalPosition + _heightManager.TowerTop;
        }
    }
}