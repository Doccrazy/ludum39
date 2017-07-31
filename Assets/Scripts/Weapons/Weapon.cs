using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons {
    public abstract class Weapon : MonoBehaviour {
        public AudioClip AudioClip;
        protected AudioSource _audioSource;

        public abstract void Init();

        public abstract void Deinit();

        public abstract void Fire();

        public abstract void Release();

        private void OnDestroy() {
            Deinit();
        }

        // sets up and adds new audio source to the gane object
        protected AudioSource SetUpEngineAudioSource(AudioClip clip) {
            // create the new audio source component on the game object and set up its properties
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = 1;
            source.loop = false;
            source.spatialBlend = 1;
            return source;
        }
    }
}
