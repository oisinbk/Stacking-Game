using System.Numerics;
using Pooling;
using UnityEngine;

namespace Sound
{
    public class AudioObjectPool : SimplePool<AudioObject>
    {
        //TODO: same here SMH...
        public AudioObject GetAudioObject(AudioClip clip, Transform t)
        {
            AudioObject obj = Get();
            obj.audioSource.clip = clip;
            obj.transform.position = t.position;
            obj.transform.rotation = t.rotation;
            return obj;
        }
    }
}