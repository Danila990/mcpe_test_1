using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Code
{
    [Serializable]
    public class ManualComponent
    {
        public LocalizedString Text;
        public Sprite Image;

        public bool IsText => !Text.IsEmpty;

        public bool IsImage => Image != null;
    }
}