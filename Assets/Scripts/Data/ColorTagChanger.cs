#if UNITY_EDITOR
using EditorScripts;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.EditorScripts
{
	[CreateAssetMenu(fileName = "ColorTagChanger", menuName = "AutoScripts/ColorTagChanger")]
	public class ColorTagChanger : ScriptableObject
	{
		[SerializeField] private GameObject[] _prefabs;
		[SerializeField, Header("0")] private Color _buttons;
		[SerializeField, Header("1")] private Color _buttons2;
        [SerializeField, Header("2")] private Color _buttons3;
        [SerializeField, Header("3")] private Color _header;
		[SerializeField, Header("4")] private Color _descriptionBackground;
		[SerializeField, Header("5")] private Color _panels;

        public void ChangeInPrefabs()
		{
            foreach (var prefab in _prefabs)
			{
				ChangeInPrefab(prefab);
			}
		}

		private void ChangeInPrefab(GameObject prefab)
		{
			var colorTags = prefab.transform.GetComponentsInChildren<ColorTag>(true);
			foreach(var colorTag in colorTags)
			{
				Image image = colorTag.GetComponent<Image>();
				if (image == null)
				{
					Debug.Log("Image null " + colorTag.gameObject.name);
				}

				Color color = colorTag.ColorNum switch
				{
					0 => _buttons,
					1 => _buttons2,
					2 => _buttons3,
					3 => _header,
					4 => _descriptionBackground,
                    5 => _panels,
                    _ => throw new ArgumentException("Неизвестный цвет")
				};

				Undo.RecordObject(prefab, "Change color");
				image.color = color;
				EditorUtility.SetDirty(prefab);
				PrefabUtility.RecordPrefabInstancePropertyModifications(image);
				UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(prefab.scene);
			}
		}
	}

	[CustomEditor(typeof(ColorTagChanger))]
	public class ColorTagChangerButton : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			ColorTagChanger cardCreator = (ColorTagChanger)target;
			if(!GUILayout.Button("Change color in prefabs"))
			{
				return;
			}

			cardCreator.ChangeInPrefabs();
			Debug.Log("ColorTagChanger changed");
		}
	}
}
#endif