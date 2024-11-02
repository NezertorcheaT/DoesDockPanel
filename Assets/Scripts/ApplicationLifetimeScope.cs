using Saving;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using PrimeTween;

public class ApplicationLifetimeScope : LifetimeScope
{
    [SerializeField] private DockLinks dockLinks;
    [SerializeField] private BarImages barImages;

    protected override void Configure(IContainerBuilder builder)
    {
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        builder.RegisterEntryPoint<ConfigEntry>();
        builder.RegisterEntryPoint<TransparentWindow>();
        builder.RegisterComponent(dockLinks).AsSelf();
        builder.RegisterComponent(barImages).AsSelf();
    }
}