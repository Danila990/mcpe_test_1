using Assets.Scripts.UI.Localization;
using Scripts.UI.FixedScroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect), typeof(FixedScrollRect))]
public class LanguageScroll : MonoBehaviour
{
    private readonly List<AsyncOperationHandle<Sprite>> _flagGetters = new List<AsyncOperationHandle<Sprite>>();

    [SerializeField] private LanguageScrollImage _imagePrefab;

    private FixedScrollRect _scrollRect;

    private void Awake()
    {
        _scrollRect = GetComponent<FixedScrollRect>();
    }

    private async void Start()
    {
        await LocalizationSettings.InitializationOperation.Task;
        await CreateFlags();
        ChangeLanguageToActual();
        _scrollRect.OnNowChildChanged += SetLanguage;
    }

    private void ChangeLanguageToActual()
    {
        Locale locale = LocalizationSettings.SelectedLocale;
        _scrollRect.SetChild(LocalizationSettings.AvailableLocales.Locales.IndexOf(locale));
    }

    private void SetLanguage(int index)
    {
        try
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private async Task CreateFlags()
    {
        await LocalizationSettings.InitializationOperation.Task;
        var tasks = new List<Task>();
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            LanguageScrollImage flagImage = GameObject.Instantiate(_imagePrefab);
            flagImage.transform.SetParent(_scrollRect.ScrollRect.content, false);
            tasks.Add(flagImage.SetLocale(locale));
        }

        await Task.WhenAll(tasks);
    }
}
