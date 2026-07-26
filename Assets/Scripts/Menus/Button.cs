using Sound;
using UnityEngine;

namespace Menus
{
    public class Button : MonoBehaviour
    {
        [SerializeField] AudioClip buttonSound;
        
        public void PlayButton()
        {
            SoundFXManager.Instance.PlaySoundFXClip(buttonSound, transform, 1f);
        }
    }
}