using System.Collections;
using System.Collections.Generic;
using Pooling;
using UnityEngine;
using System.Threading;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;

namespace Sound
{
    public class SoundFXManager : MonoSingleton<SoundFXManager>
    {
        [SerializeField] private AudioObjectPool audioPool;

        private float _masterVolume;
        
        //playing FX- no loops
        //playing randomized FX- no loops
        
        public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float vol)
        {
            //AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity, this.transform);
            AudioObject audioObject = audioPool.Get();
            audioObject.audioSource.clip = audioClip;
            audioObject.transform.position = spawnTransform.position;
            
            audioObject.audioSource.volume = vol * _masterVolume;
            float randomPitch = Random.Range(0.9f, 1.1f);
            audioObject.audioSource.pitch = randomPitch;
            
            audioObject.audioSource.Play();
            
            float actualLength = audioClip.length/randomPitch;
            ReturnToPoolAsync(audioObject, actualLength).Forget();
        }
    
        private async UniTaskVoid ReturnToPoolAsync(AudioObject obj, float delay)
        {
            // SuppressCancellationThrow prevents errors if the manager is destroyed while waiting
            bool canceled = await UniTask.Delay(
                System.TimeSpan.FromSeconds(delay), 
                cancellationToken: this.GetCancellationTokenOnDestroy()
            ).SuppressCancellationThrow();
        
            // Only return to pool if the task wasn't cancelled (e.g., scene didn't unload)
            if (!canceled && obj != null)
            {
                audioPool.Return(obj);
            }
        }
        
        public void PlayRandomSoundFXClip(List<AudioClip> audioClips, Transform spawnTransform, float volume)
        {
            if (audioClips == null || audioClips.Count == 0) return;

            int rand = Random.Range(0, audioClips.Count);
            PlaySoundFXClip(audioClips[rand], spawnTransform, volume);
        }

        public void AdjustMasterVolume(float volume)
        {
            _masterVolume = volume;
        }
    }
}