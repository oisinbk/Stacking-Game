using UnityEngine;
using Blocks.Channels;

namespace Tower_and_Camera
{
    public class HeightManager : MonoBehaviour
    {
        [SerializeField] private Vector3 towerBottom = new Vector3(0, 0.5f, 0);
        public float Score {get; private set;}
        
        public Vector3 TowerTop { get; private set;}
        private float _maxTowerHeight;
        private float _towerHeight;

        private void AddToScore(float newHeight)
        {
            if (newHeight > _towerHeight)
            {
                _towerHeight = newHeight;
                Score = (int)(_towerHeight - towerBottom.y);
                TowerTop = Vector3.up * (_towerHeight - towerBottom.y);
            }
        }

        public void ResetScore()
        {
            _towerHeight = 0;
        }
        
        private void OnEnable()
        {
            BlockIsStableEventChannel.UpdateHeight += AddToScore;
        }

        private void OnDisable()
        {
            BlockIsStableEventChannel.UpdateHeight -= AddToScore;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(towerBottom, new Vector3(0.5f, 0.5f, 0));
        }
    }
}