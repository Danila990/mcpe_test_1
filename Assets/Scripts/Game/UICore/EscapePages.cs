using UnityEngine;

namespace Code
{
    public class EscapePages : MonoBehaviour
    {
        private StackPages _stackPages;
        private bool _isActive = false;

        private void Start()
        {
            _stackPages = StackPages.Instance;
            var animator = GetComponent<BasePageAnimation>();
            animator.OnShowAnimationEnd += () => _isActive = true;
            animator.OnMoveInAnimationEnd += () => _isActive = true;
            animator.OnHideAnimationStart += () => _isActive = false;
            animator.OnMoveInAnimationStart += () => _isActive = false;
            animator.OnMoveOutAnimationStart += () => _isActive = false;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape) && _isActive)
            {
                _stackPages.PageHide();
            }
        }
    }
}