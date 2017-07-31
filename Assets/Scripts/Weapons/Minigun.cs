using System;
using UnityEngine;

namespace Weapons {
    public class Minigun : Weapon {
        public GameObject Attachment;
        public GameObject Bullet;
        private GameObject _attachment;
        private bool _firing;

        private void Start() {
            InvokeRepeating("FireBullet", 0, 0.05f);
        }

        public override void Init() {
            _audioSource = SetUpEngineAudioSource(AudioClip);
            _attachment = Instantiate(Attachment, transform);
            _attachment.transform.localPosition = Vector3.up * 2f;
        }

        public override void Deinit() {
            if (_attachment != null) {
                Destroy(_attachment);
                Destroy(_audioSource);
            }
            _firing = false;
        }

        public override void Fire() {
            _firing = true;
        }

        public override void Release() {
            _firing = false;
        }

        public void Remove() {
            Destroy(_attachment);
        }

        private void FireBullet() {
            if (_firing) {
                var dir = new Vector3(0, -0.05f, 1).normalized;
                dir = transform.TransformDirection(dir);
                var b = Instantiate(Bullet);
                b.transform.position = _attachment.transform.position + dir*0.5f;
                var rb = b.GetComponent<Rigidbody>();
                rb.velocity = GetComponent<Rigidbody>().velocity + dir * 20f;

                if (GetComponent<Fuel>()) {
                    GetComponent<Fuel>().Use(0.001f);
                }
                _audioSource.Play();
            }
        }
    }
}
