using UnityEngine;

namespace Weapons {
    public class BarrelLauncher : Weapon {
        public GameObject Attachment;
        public GameObject Grenade;
        private GameObject _attachment;

        public override void Init() {
            _audioSource = SetUpEngineAudioSource(AudioClip);
            _attachment = Instantiate(Attachment, transform);
            _attachment.transform.localPosition = Vector3.up * 1.7f;
        }

        public override void Deinit() {
            if (_attachment != null) {
                Destroy(_attachment);
                Destroy(_audioSource);
            }
        }

        public override void Fire() {
            var dir = new Vector3(0, 1, 1).normalized;
            dir = transform.TransformDirection(dir);
            var g = Instantiate(Grenade);
            g.transform.localScale = transform.localScale;
            g.transform.position = transform.position + dir*1.5f;
            var rb = g.GetComponent<Rigidbody>();
            rb.velocity = GetComponent<Rigidbody>().velocity;
            rb.AddForce(dir * 400f);

            if (GetComponent<Fuel>()) {
                GetComponent<Fuel>().Use(0.03f);
            }
            _audioSource.Play();
        }

        public override void Release() {
        }

        public void Remove() {
            Destroy(_attachment);
        }
    }
}
