using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class MuteButton : BaseButton
    {
        [SerializeField] private Sprite _offState;
        [SerializeField] private Sprite _onState;
        [SerializeField] private Image _image;

        private AudioContoller _audioContoller;

        protected override void Start()
        {
            base.Start();

            _audioContoller = AudioContoller.Instance;
            ChangeSprite();
        }

        protected override void OnClick()
        {
            base.OnClick();

            _audioContoller.ChangeMute(!_audioContoller.Mute);
            ChangeSprite();
        }

        private void ChangeSprite()
        {
            if (!_audioContoller.Mute)
            {
                _image.sprite = _onState;
            }
            else
            {
                _image.sprite = _offState;
            }
        }
    }
}