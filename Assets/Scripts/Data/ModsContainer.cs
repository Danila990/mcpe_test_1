using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.Localization.Tables;
using UnityEditor.Localization;
using System.IO;
#endif

namespace Code
{
    [CreateAssetMenu(menuName = "ModsContainer", fileName = "ModsContainer")]
    public class ModsContainer : ScriptableObject
    {
        [SerializeField] private ModData[] _modsData;
  
        public ModData[] GetModDatas()
        {
            return _modsData;
        }

#if UNITY_EDITOR
        public void Parce()
        {
            if (!CheckFolder())
            {
                return;
            }

            int modInex;
            for (int i = 0; i < _modsData.Length; i++)
            {
                modInex = i + 1;
                ModData modData = Resources.Load<ModData>($"ModsData/Data/ModData_{modInex}");
                _modsData[i] = modData;
                ParceSprites(modData, modInex);
                ParceVerisonMod(modData, modInex);
                ParceDescription(modData, modInex);
                EditorUtility.SetDirty(modData);
            }

            ClearEmptyData();
            EditorUtility.SetDirty(this);
        }

        private bool CheckFolder()
        {
            string folderPath = Application.dataPath + "/Resources/Наполнение";

            if (Directory.Exists(folderPath))
            {
                _modsData = new ModData[Directory.GetDirectories(folderPath).Length];
                return true;
            }
            else
            {
                Debug.LogError("Папка " + folderPath + " не существует!");
                return false;
            }
        }

        private void ParceDescription(ModData modData, int loadIndex)
        {
            StringTableCollection tableCollection = LocalizationEditorSettings.GetStringTableCollection("ModsPageTable");
            TextAsset textDescription = Resources.Load<TextAsset>($"Наполнение/{loadIndex}/ОПИСАНИЕ");
            foreach (var locale in LocalizationEditorSettings.GetLocales())
            {
                LocalizationTable table = tableCollection.GetTable(locale.Identifier.Code);
                SetLocale(table as StringTable, textDescription, loadIndex);
            }
        }

        private void SetLocale(StringTable table, TextAsset textDescription, int indexMod)
        {
            string codeLocale = table.LocaleIdentifier.Code;
            StreamReader streamReader = new StreamReader(new MemoryStream(textDescription.bytes));
            bool isNeedLine = false;
            string nameMod = "";
            string desctiptionMod = "";
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                if (line == null)
                {
                    streamReader.Close();
                }

                if(codeLocale == line.ToLower() && isNeedLine == false)
                {
                    isNeedLine = true;
                }
                else if (!isNeedLine)
                {
                    continue;
                }
                else if (line.Length > 1 && isNeedLine)
                {
                    if (line[0] == '-')
                    {
                        break;
                    }
                }

                if (line.Length <= 0 && nameMod.Length <= 0 || codeLocale == line.ToLower())
                {
                    continue;
                }
                else if (nameMod.Length <= 0)
                {
                    nameMod = line;
                    continue;
                }

                if(desctiptionMod.Length <= 0)
                {
                    desctiptionMod = line;
                    continue;
                }
                else
                {
                    desctiptionMod += "\n" + line;
                }
            }

            if(nameMod.Length <= 0)
            {
                Debug.LogError(codeLocale + " Не найден");
            }
            else
            {
                table.GetEntry($"Mod_{indexMod}_name").Value = nameMod;
                table.GetEntry($"Mod_{indexMod}_Description").Value = desctiptionMod;
                EditorUtility.SetDirty(table);
                AssetDatabase.SaveAssets();
            }
        }

        private void ParceSprites(ModData modData, int loadIndex)
        {
            modData.SetIcon(Resources.Load<Sprite>($"Наполнение/{loadIndex}/icon"));
            modData.SetScreenshots(Resources.LoadAll<Sprite>($"Наполнение/{loadIndex}/СКР"));
        }

        private void ParceVerisonMod(ModData modData, int loadIndex)
        {
            TextAsset textVersions = Resources.Load<TextAsset>($"Наполнение/{loadIndex}/ВЕРСИИ");
            if (textVersions == null)
            {
                modData.SetVerison("");
            }
            else
            {
                modData.SetVerison(textVersions.text);
            }
        }

        private void ClearEmptyData()
        {
            for (int i = _modsData.Length; i < 4; i++)
            {
                int modInex = i + 1;
                ModData modData = Resources.Load<ModData>($"ModsData/Data/ModData_{modInex}");
                modData.ResetData();
                EditorUtility.SetDirty(modData);
            }
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ModsContainer))]
    public class CellDebugCustomEditor : Editor
    {
        private ModsContainer _modsContainer;

        private void OnEnable()
        {
            _modsContainer = (ModsContainer)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Parce Data"))
            {
                _modsContainer.Parce();
            }
        }
    }
#endif
}