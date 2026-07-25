using System;
using Pooling;
using UnityEngine;
using Unity.Cinemachine;

namespace Tower_and_Camera
{
    [RequireComponent(typeof(HeightManager))]
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera mainCamera;

        private void ChangeFollowTarget(GameObject target)
        {
            mainCamera.Target.TrackingTarget = target.transform;
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