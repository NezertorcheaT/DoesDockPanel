using System.Linq;
using Files;
using Input;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using PrimeTween;
using Saving.Settings;
using UI.Files;

public class ApplicationLifetimeScope : LifetimeScope
{
    [SerializeField] private DockLinks dockLinks;
    [SerializeField] private BarImages barImages;

    protected override void Configure(IContainerBuilder builder)
    {
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        builder.RegisterEntryPoint<ConfigEntry>();
        builder.RegisterEntryPoint<TransparentWindow>();
        builder.RegisterEntryPoint<WindowsInputActions>();
        builder.RegisterComponent(dockLinks).AsSelf();
        builder.RegisterComponent(barImages).AsSelf();
        var controls = new Controls();
        var bools = controls.devices?.Select(a =>
        {
            Debug.Log(a.canRunInBackground);
            return a.canRunInBackground;
        }).ToArray();
        controls.Enable();
        builder.RegisterInstance(controls).AsSelf();

        builder.RegisterFactory<LinkUI, Transform, Link, LinkUI>((prefab, container, file) =>
        {
            var linkUI = Container.Instantiate(prefab, Vector3.zero, Quaternion.identity, container);
            linkUI.Initialize(file);
            return linkUI;
        }).AsSelf();
        builder.RegisterFactory<IFolderUI, Transform, Folder, bool, IFolderUI>((prefab, container, file, inside) =>
        {
            var folderUI = Container
                .Instantiate(prefab.ContainedIn, Vector3.zero, Quaternion.identity, container)
                .GetComponent<IFolderUI>();
            folderUI.Initialize(file, inside);
            return folderUI;
        }).AsSelf();
    }
}