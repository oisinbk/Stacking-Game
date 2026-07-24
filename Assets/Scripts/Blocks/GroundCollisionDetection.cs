using UnityEngine;
using GameStates;

namespace Blocks
{
    public class GroundCollisionDetection: MonoBehaviour, IBlockProperties
    {
        [SerializeField] private GameOverEventChannel gameOverChannel;
        private bool _isFirst;
        private bool _collidedWithFloor;

        public void SetProperties(bool isFirst)
        {
            _isFirst = isFirst;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (gameOverChannel != null && !_collidedWithFloor)
            {
                if (other.gameObject.CompareTag("Ground"))
                {
                    _collidedWithFloor = true;
                    gameOverChannel.RaiseEvent();
                }
                else if (other.gameObject.CompareTag("Stage") && !_isFirst)
                {
                    _collidedWithFloor = true;
                    gameOverChannel.RaiseEvent();
                }
            }
        }
    }
}