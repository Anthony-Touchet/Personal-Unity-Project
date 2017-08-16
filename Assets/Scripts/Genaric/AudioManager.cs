using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Genaric
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [Serializable]
        public class AudioSample
        {
            public string sampleName;
            public AudioClip sampleSource;
        }

        private List<AudioSource> m_AudioSources = new List<AudioSource>();

        public class StingEvent : UnityEvent<string> { }
        public List<AudioSample> audioSamples = new List<AudioSample>();

        public AudioSource AvaliableAudioSource
        {
            get
            {
                foreach (var audioSource in m_AudioSources)
                {
                    if(audioSource.isPlaying) continue;

                    return audioSource;
                }

                throw new Exception("No avaliabe Audio Sources!!!");
            }
        }

        public override void SubAwake()
        {
            m_AudioSources = GetComponents<AudioSource>().ToList();
        }

        public void PlaySound(string soundName)
        {
            foreach (var audioSample in audioSamples)
            {
                if (audioSample.sampleName != soundName) continue;

                var audioSource = AvaliableAudioSource;

                audioSource.clip = audioSample.sampleSource;
                audioSource.Play();
                return;
            }

            Debug.LogError("Could not find sound!");
        }
    }
}
