using Assets.Scripts;
using Scripts.AD;
using Scripts.AddonData.AddonOpen;
using Scripts.FileLoaders;
using Scripts.UI.UIStates.OpenAddonPageScripts;
using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Code
{
    public class LoadModPage : BasePage
    {
		[SerializeField] private Button _openModButton;
        [SerializeField] private Button _redonloadButton;
		[SerializeField] private GameObject _errorPanel;
		[SerializeField] private GameObject _progressPanel;
		[SerializeField] private TMP_Text _completeText;
        [SerializeField] private TMP_Text _loadText;
        [SerializeField] private TMP_Text _progresText;

        private string _modPath;
        private string _pathToAddon;
        private CancellationTokenSource _cancellationToken;
        private int _lastProgress = -1;

        [field: SerializeField] public NativeMessageBoxLocalizedMessages MessageBoxLocalizedMessages { get; private set; }

        public event Action<float> OnProgressUpdate;

        protected override void Awake()
        {
            base.Awake();

			_openModButton.onClick.AddListener(OpenAddon);
            _redonloadButton.onClick.AddListener(LoadAddon);
            ModData modData = _stackPages.GetPage<SelectedModPage>(PageType.SelectedMod).ModData;
            _modPath = modData.ModPath;
            if (modData.IsShowRewardAdAfterOpenMod)
            {
                AdsController.Instance.ShowRewardAd();
            }

            LoadAddon();
        }

        private async void LoadAddon()
		{
			_cancellationToken = new CancellationTokenSource();
			CancellationToken token = _cancellationToken.Token;
			try
			{
				OnLoadingStart();
				var result = await LoadAddon(token);
				token.ThrowIfCancellationRequested();
				OnLoadingEnd(result);
			}
			catch(OperationCanceledException)
			{
			}
			finally
			{
				_cancellationToken.Dispose();
				_cancellationToken = null;
			}
		}

        private void OnLoadingStart()
        {
            _openModButton.interactable = false;
            _progressPanel.gameObject.SetActive(true);
            _errorPanel.gameObject.SetActive(false);
            _completeText.gameObject.SetActive(false);
            //_loadText.gameObject.SetActive(true);
            OnLoadingProgressUpdate(0);
        }

        private void OnLoadingProgressUpdate(float value)
        {
            int progress = (int)(value * 100);
            if (progress == _lastProgress)
            {
                return;
            }

            _progresText.text = $"{progress} %";
            _lastProgress = progress;
        }

        private void OnLoadingEnd(LoadStatus addonLoadResult)
        {
            if (addonLoadResult == LoadStatus.Success)
            {
                _openModButton.interactable = true;
                _progressPanel.gameObject.SetActive(true);
                _completeText.gameObject.SetActive(true);
                //_loadText.gameObject.SetActive(false);
            }
            else
            {
                _openModButton.interactable = false;
                _progressPanel.gameObject.SetActive(false);
                _errorPanel.gameObject.SetActive(true);
            }
            OnLoadingProgressUpdate(1);
            ShowNativeMessageBoxIfError(addonLoadResult);
        }

        private async void OpenAddon()
		{
            OpenAddonResult openAddonResult = AddonOpener.OpenAddon(_pathToAddon);

            LocalizedString errorLocalization = openAddonResult switch
            {
                OpenAddonResult.Success => null,
                OpenAddonResult.Deleted => MessageBoxLocalizedMessages.FileMissing,
                OpenAddonResult.NotInstalled => MessageBoxLocalizedMessages.MineNotInstalled,
                OpenAddonResult.Unknown => MessageBoxLocalizedMessages.UnknownError,
                _ => throw new ArgumentOutOfRangeException(nameof(openAddonResult), openAddonResult, null)
            };

            if (openAddonResult == OpenAddonResult.Success)
            {
                EventExecutorOnApplicationFocus.OnApplicationFocusEvent += ShowRateBoxOnBackToApp;
            }

            if (errorLocalization != null)
            {
                var error = await errorLocalization.GetLocalizedStringAsync().Task;
                NativeMessageBoxWrapper.Show(error);
            }
        }

        private void ShowRateBoxOnBackToApp(bool focus)
        {
            if (focus)
            {
                EventExecutorOnApplicationFocus.OnApplicationFocusEvent -= ShowRateBoxOnBackToApp;
                Debug.Log("RateInApp");
                InAppReview.RateInApp();
            }
        }

        private async void ShowNativeMessageBoxIfError(LoadStatus result)
        {
            if (result == LoadStatus.Success || result == LoadStatus.Cancel)
            {
                return;
            }

            LocalizedString error = result switch
            {
                LoadStatus.NoAccess => MessageBoxLocalizedMessages.NoAccess,
                LoadStatus.DiskIsFull => MessageBoxLocalizedMessages.DiskIsFull,
                LoadStatus.ConnectionError => MessageBoxLocalizedMessages.ConnectionError,
                LoadStatus.InternalError => MessageBoxLocalizedMessages.InternalError,
                LoadStatus.UnknownError => MessageBoxLocalizedMessages.UnknownError,
                _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
            };

            var errorLocalization = await error.GetLocalizedStringAsync().Task;
            NativeMessageBoxWrapper.Show(errorLocalization);
        }

        public async Task<LoadStatus> LoadAddon(CancellationToken token)
		{
			_pathToAddon = null;
			var addonLoader = new FileLoader(_modPath);
			addonLoader.OnProgressUpdate += (progress) => OnLoadingProgressUpdate(progress);
			LoadStatus result = await addonLoader.Load(token);
			_pathToAddon = result == LoadStatus.Success ? addonLoader.PathToFile : null;
			return result;
		}
    }
}