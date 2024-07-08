using UnityEngine;

namespace Scripts.AD.Native
{
	[RequireComponent(typeof(RectTransform), typeof(BoxCollider))]
	public class BoxColliderSizeFitterToTransformSize : MonoBehaviour
	{
		private void Start()
		{
			RectTransform transform = gameObject.GetComponent<RectTransform>();
			BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
			Vector2 size = transform.rect.size;
			boxCollider.size = new Vector3(size.x, size.y, boxCollider.size.z);
		}
	}
}
