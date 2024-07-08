#if ADMOB_ENABLED

using UnityEngine;

namespace Scripts.AD.Native
{
	[RequireComponent(typeof(RectTransform), typeof(BoxCollider))]
	public class NativeAdBoxColliderDisabler : MonoBehaviour
	{
		public Collider DependentCollider;

		private int _countOfDisablers = 0;

		/// <summary>
		/// Количество ограничителей, выключающих коллайдер нейтива(чтобы по нему нельзя было нажать)
		/// </summary>
		public int CountOfDisablers
		{
			get
			{
				return _countOfDisablers;
			}

			set
			{
				_countOfDisablers = value;
				EnableDependentCollider(_countOfDisablers == 0);
			}
		}

		private void Start()
		{
			RectTransform dependentTransform = DependentCollider.GetComponent<RectTransform>();
			BoxCollider collider = GetComponent<BoxCollider>();
			Vector2 size = dependentTransform.rect.size;
			collider.size = new Vector3(size.x, size.y, collider.size.z);
			CountOfDisablers = 0;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if(IsCollidedWithDisabler(collision.collider))
			{
				CountOfDisablers++;
			}
		}

		private void OnCollisionExit(Collision collision)
		{
			if(IsCollidedWithDisabler(collision.collider))
			{
				CountOfDisablers--;
			}
		}

		private void EnableDependentCollider(bool isEnable)
		{
			DependentCollider.enabled = isEnable;
		}

		private bool IsCollidedWithDisabler(Collider collider)
		{
			LayerMask nativeDisablerLayer = AdsController.Instance.AdmobController.Parameters.NativeDisablerLayer;
			if(collider.gameObject.layer != nativeDisablerLayer.value)
			{
				return false;
			}

			RectTransform collidedRectTransform = collider.gameObject.GetComponent<RectTransform>();
			if(collidedRectTransform == null)
			{
				return false;
			}

			Canvas thisCanvas = DependentCollider.GetComponentInParent<Canvas>().rootCanvas;
			Canvas collidedCanvas = collider.gameObject.GetComponentInParent<Canvas>().rootCanvas;
			return thisCanvas.sortingOrder <= collidedCanvas.sortingOrder;
		}
	}
}

#endif