using UnityEngine;

namespace Atelier.Audio {

    [AddComponentMenu("Atelier/Audio/Repeated Player")]
    public class RepeatedPlayer : MonoBehaviour {

        [SerializeField]
        private AudioPanel panel;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float volumeScale = 1.0f;

        [SerializeField]
        private float delay;

        [SerializeField]
        private float noise;

        private AudioSource source;
        private AudioClip lastClip;
        private float timer;

        private void Awake() {
            this.source = GetComponentInParent<AudioSource>();
        }

        private void OnEnable() {
            this.PlayNextClip();
        }

        private void FixedUpdate() {
            this.timer -= Time.deltaTime;
            if (this.timer <= 0.0f) {
                this.PlayNextClip();
            }
        }

        private void PlayNextClip() {
            AudioClip clip = this.GetNextClip();
            this.source.PlayOneShot(clip, this.volumeScale);
            this.timer = clip.length + this.delay + Random.Range(-this.noise, this.noise);
            this.lastClip = clip;
        }

        private AudioClip GetNextClip() {
            int index = Random.Range(0, this.panel.Count);
            AudioClip clip = this.panel[index];
            while (this.PanelHasSingleClip() || this.ClipIsRepeat(clip)) {
                index = Random.Range(0, this.panel.Count);
                clip = this.panel[index];
            }
            return clip;
        }

        private bool PanelHasSingleClip() {
            return this.panel.Count == 1;
        }

        private bool ClipIsRepeat(AudioClip clip) {
            return this.lastClip != null && this.lastClip == clip;
        }
    }

}

