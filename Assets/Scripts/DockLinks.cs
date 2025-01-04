using System;
using System.IO;
using System.Threading.Tasks;
using Files;
using R3;
using Saving.Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DockLinks : MonoBehaviour
{
    [SerializeField] private UnityEvent updateStarted;
    [SerializeField] private UnityEvent updateEnded;

    public Observable<Unit> UpdateStarted => _updateStarted;
    private readonly Subject<Unit> _updateStarted = new();
    public Observable<Unit> UpdateEnded => _updateEnded;
    private readonly Subject<Unit> _updateEnded = new();

    public readonly ObservableList<FileObject> Links = new();

    private void Start()
    {
        if (!Directory.Exists(ConfigEntry.Instance.LinksPath))
            Directory.CreateDirectory(ConfigEntry.Instance.LinksPath);
        UpdateImages();
    }

    public async void UpdateImages()
    {
        try
        {
            await DockTextures.Update();
            updateStarted.Invoke();
            _updateStarted.OnNext(new Unit());
            await FileObjectUtility.Populate(Links, ConfigEntry.Instance.LinksPath);
            updateEnded.Invoke();
            _updateEnded.OnNext(new Unit());
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void OnDestroy()
    {
        Links.Dispose();
    }
}