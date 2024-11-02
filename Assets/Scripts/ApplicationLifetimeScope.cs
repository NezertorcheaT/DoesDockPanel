using Saving;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ApplicationLifetimeScope : LifetimeScope
{
    [SerializeField] private DockLinks dockLinks;
    [SerializeField] private BarImages barImages;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<ConfigEntry>().AsSelf();
        builder.RegisterEntryPoint<TransparentWindow>().AsSelf();
        builder.RegisterComponent(dockLinks).AsSelf();
        builder.RegisterComponent(barImages).AsSelf();
    }
}