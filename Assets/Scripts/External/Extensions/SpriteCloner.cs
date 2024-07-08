using UnityEngine;

namespace Assets.Scripts.Extensions
{
	public static class SpriteCloner
	{
		public static Sprite Clone(this Sprite sprite)
		{
			Texture2D originalTexture = sprite.texture;
			var copyTexture = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, 
				originalTexture.mipmapCount, false);
			Graphics.CopyTexture(originalTexture, copyTexture);
			return Sprite.Create(copyTexture, sprite.rect, sprite.pivot, sprite.pixelsPerUnit, 0, SpriteMeshType.FullRect);
		} 
	}
}
