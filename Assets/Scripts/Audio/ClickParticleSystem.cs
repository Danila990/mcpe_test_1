using UnityEngine;

namespace Code
{
    public class ClickParticleSystem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition) +
                                    Vector3.forward * _camera.nearClipPlane;
                ParticleSystem particleSystem = Instantiate(_particleSystem, worldPosition, Quaternion.identity);
                particleSystem.Play();
                Destroy(particleSystem.gameObject, 0.4f);
            }
        }
    }
}