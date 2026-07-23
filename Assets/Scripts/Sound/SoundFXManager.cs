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
        //responsible for:
        //  retrieving audio sources from a pool
        //  checking their volume and pitch
        //  playing them
        //  returning them to pool
        
        //TODO: needs lots of work here
        [SerializeField] private float masterVolume;
        [SerializeField] private AudioObjectPool audioPool;
        [SerializeField] private float fadeTime = 2f;
        [SerializeField] private AudioClip buttonEffect;

        public void PlayButtonEffect()
        {
            PlaySoundFXClip(buttonEffect, transform, 1, 1);
        }
        
        public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float vol, float pitch)
        {
            //AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity, this.transform);
            AudioObject audioObject = audioPool.GetAudioObject(audioClip, spawnTransform);
            audioObject.transform.rotation = Quaternion.identity;
            audioObject.audioSource.volume = vol * masterVolume;
            audioObject.audioSource.pitch = Random.Range(0.9f, 1.1f) * pitch;
            audioObject.audioSource.Play();
            StartCoroutine(ReturnToPoolAfterDelay(audioObject, audioClip.length));
        }
        
        private IEnumerator ReturnToPoolAfterDelay(AudioObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            audioPool.Return(obj);
        }
        
        public void PlayRandomSoundFXClip(List<AudioClip> audioClips, Transform spawnTransform, float volume, float pitch)
        {
            if (audioClips == null || audioClips.Count == 0) return;

            int rand = Random.Range(0, audioClips.Count);
            PlaySoundFXClip(audioClips[rand], spawnTransform, volume, pitch);
        }
        
        
        
        public AudioObject FadeInLoopingEffect(AudioClip audioClip, Transform spawnTransform, float targetVol, float pitch)
        {
            AudioObject audioObject = audioPool.GetAudioObject(audioClip, spawnTransform);
            audioObject.transform.rotation = Quaternion.identity;
            
            audioObject.audioSource.loop = true;
            audioObject.audioSource.pitch = pitch;
            
            audioObject.audioSource.volume = 0f;
            audioObject.audioSource.Play();
            
            float finalTargetVolume = targetVol * masterVolume;
            
            // Trigger the UniTask and forget it
            FadeInAsync(audioObject, finalTargetVolume, fadeTime, this.GetCancellationTokenOnDestroy()).Forget();
            
            return audioObject;
        }

        private async UniTaskVoid FadeInAsync(AudioObject obj, float targetVolume, float duration, CancellationToken token)
        {
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                // Safely exit if manager is destroyed or object is somehow lost
                if (token.IsCancellationRequested || obj == null) return;

                obj.audioSource.volume = Mathf.Lerp(0f, targetVolume, timeElapsed / duration);
                timeElapsed += Time.unscaledDeltaTime;
                
                // Yield to the next frame (Update loop)
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
            
            if (obj != null) obj.audioSource.volume = targetVolume;
        }
        
        public void FadeOutLoopingEffect(AudioObject audioObject)
        {
            if (audioObject != null && audioObject.audioSource.isPlaying)
            {
                FadeOutAsync(audioObject, fadeTime, this.GetCancellationTokenOnDestroy()).Forget();
            }
        }

        private async UniTaskVoid FadeOutAsync(AudioObject obj, float duration, CancellationToken token)
        {
            float timeElapsed = 0f;
            float startVolume = obj.audioSource.volume;

            while (timeElapsed < duration)
            {
                if (token.IsCancellationRequested || obj == null) return;

                obj.audioSource.volume = Mathf.Lerp(startVolume, 0f, timeElapsed / duration);
                timeElapsed += Time.unscaledDeltaTime;
                
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (obj != null)
            {
                obj.audioSource.volume = 0f;
                obj.audioSource.Stop();
                obj.audioSource.loop = false;
                
                audioPool.Return(obj);
            }
        }
    }
}