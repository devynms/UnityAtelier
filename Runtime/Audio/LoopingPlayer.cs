using UnityEngine;

namespace Atelier.Audio {

    [AddComponentMenu("Atelier/Audio/Looping Player")]
    public class LoopingPlayer : MonoBehaviour {

        [SerializeField]
        private AudioPanel panel;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float volumeScale = 1.0f;

        [SerializeField]
        private float interval;

        [SerializeField]
        private float noise;

        [SerializeField]
        private bool playOnStop = false;


        public float timeScale { get; set; } = 1.0f;


        private AudioSource source;

        private AudioClip lastClip;
        private float timer;
        private float lastTime = float.NegativeInfinity;

        private void Awake() {
            this.source = GetComponentInParent<AudioSource>();
        }

        private void OnEnable() {
            this.PlayRandomClip();
            this.KickTimer();
        }

        private void OnDisable() {
            if (this.playOnStop && (Time.time - this.lastTime > 0.25f * this.interval)) {
                this.PlayRandomClip();
            }
        }

        private void FixedUpdate() {
            this.timer -= Time.deltaTime;
            if (this.timer <= 0.0f) {
                this.PlayRandomClip();
                this.KickTimer();
                this.lastTime = Time.time;
            }
        }

        private void KickTimer() {
            this.timer = (this.interval + Random.Range(-this.noise, this.noise)) * this.ScalingFactor();
        }

        private float ScalingFactor() {
            // we want to take a value on the curve magnitude**2, and convert it to a value
            // going linearly to a HIGH for low magnitudes, and a LOW for high magnitudes.
            // (the slower we move, the longer the timer gets...)
            // timeScale = magnitude**2
            // scaling = (1 - hi) * magnitude + hi (line going from (0, hi) to (1, lo)
            // magnitude = sqrt(timeScale)
            // scaling = (1 - hi) * sqrt(timeScale) + hi
            return -1.0f * Mathf.Sqrt(this.timeScale) + 2.0f;
        }

        private void PlayRandomClip() {
            int index = Random.Range(0, this.panel.Count);
            AudioClip clip = this.panel[index];
            while (this.PanelHasSingleClip() || this.ClipIsRepeat(clip)) {
                index = Random.Range(0, this.panel.Count);
                clip = this.panel[index];
            }
            this.source.PlayOneShot(clip, this.volumeScale);
            this.lastClip = clip;
        }

        private bool PanelHasSingleClip() {
            return this.panel.Count == 1;
        }

        private bool ClipIsRepeat(AudioClip clip) {
            return this.lastClip != null && this.lastClip == clip;
        }


    }

}

