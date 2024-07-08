using UnityEngine;

namespace Code
{
    public class AudioContoller : Singleton<AudioContoller>
    {
        [SerializeField] private AudioClip _buttonClip;

        [field: SerializeField] public bool Mute { get; private set; } = false;

        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();

            _audioSource = GetComponent<AudioSource>();
        }

        public void ChangeMute(bool mute)
        {
            Mute = mute;
        }

        public void PlayButtonAudio()
        {
            if (Mute)
            {
                return;
            }

            _audioSource.PlayOneShot(_buttonClip);
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                PlayButtonAudio();
            }
        }
    }
}