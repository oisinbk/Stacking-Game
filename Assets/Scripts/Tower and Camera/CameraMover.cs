using System;
using Pooling;
using UnityEngine;
using Unity.Cinemachine;

namespace Tower_and_Camera
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class CameraMover : MonoBehaviour
    {
        private CinemachineCamera _mainCamera;

        private void Awake()
        {
            _mainCamera = GetComponent<CinemachineCamera>();
        }

        private void ChangeFollowTarget(GameObject target)
        {
            _mainCamera.Target.TrackingTarget = target.transform;
        }


        private void OnEnable()
        {
            SpawnEventChannel.SpawnBlock += ChangeFollowTarget;
        }

        private void OnDisable()
        {
            SpawnEventChannel.SpawnBlock -= ChangeFollowTarget;
        }
    }
}