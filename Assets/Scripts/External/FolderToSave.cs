using System.IO;
using UnityEngine;

namespace Scripts
{
	public static class FolderToSave
	{
		public static readonly string FolderPath = Path.Combine(Application.persistentDataPath, "FillData");
	}
}
