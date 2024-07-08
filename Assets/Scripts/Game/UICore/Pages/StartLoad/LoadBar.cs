using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class LoadBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _timeDuration = 4f;
        [SerializeField] private float _delayUpdate = 0.1f;
        [SerializeField] private Image _image;

        private void Awake()
        {
            StartCoroutine(SphereAnimation());
        }

        private IEnumerator SphereAnimation()
        {
            float startTime = Time.time;
            while (true)
            {
                float timeElapsed = Time.time - startTime;
                float timeRemaining = _timeDuration - timeElapsed;
                float percentageComplete = (timeElapsed / _timeDuration) * 100;
                float clampValue = Mathf.Clamp(percentageComplete, 0, 100);
                _image.fillAmount = clampValue * 0.01f;
                _text.text = $"{clampValue.ToString("F0")}%";
                if(timeRemaining <= 0)
                {
                     break;
                }

                yield return new WaitForSeconds(_delayUpdate);
            }

            yield return null;
        }
    }
}