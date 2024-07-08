using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts.UI.UIStates.OpenAddonPageScripts
{
	[Serializable]
	public class NativeMessageBoxLocalizedMessages
	{
		[field: SerializeField]
		public LocalizedString MineNotInstalled
		{
			get;
			private set;
		}

		[field: SerializeField] 
		public LocalizedString NoAccess
		{
			get;
			private set;
		}

		[field: SerializeField] 
		public LocalizedString DiskIsFull
		{
			get;
			private set;
		}

		[field:	SerializeField] 
		public LocalizedString ConnectionError
		{
			get;
			private set;
		}

		[field: SerializeField] 
		public LocalizedString FileMissing
		{
			get;
			private set;
		}

		[field: SerializeField] 
		public LocalizedString InternalError
		{
			get;
			private set;
		}

		[field: SerializeField] 
		public LocalizedString UnknownError
		{
			get;
			private set;
		}
	}
}
