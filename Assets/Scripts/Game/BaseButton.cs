using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public abstract class BaseButton : MonoBehaviour
    {
        protected Button _button;

        protected virtual void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        protected virtual void OnDestroy()
        {
            _button?.onClick.RemoveListener(OnClick);
        }

        protected virtual void OnClick() { }
    }
}