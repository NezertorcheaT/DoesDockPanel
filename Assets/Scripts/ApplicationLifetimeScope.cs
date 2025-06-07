using CustomHelper;
using Input;
using MiddleSpawn;
using VContainer;
using VContainer.Unity;
using PrimeTween;
using Saving.Settings;
using UI;
using UnityEngine;

public class ApplicationLifetimeScope : LifetimeScope
{
    [SerializeField] private RepresentationsContainer container;

    protected override void Configure(IContainerBuilder builder)
    {
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        builder.RegisterEntryPoint<ConfigEntry>().AsSelf();
        builder.RegisterEntryPoint<TransparentWindow>().AsSelf();
        builder.RegisterEntryPoint<WindowsInputActions>().AsSelf();
        builder.RegisterEntryPoint<KeyListener>().AsSelf();
        builder.RegisterEntryPoint<MainContainer>().AsSelf();
        builder.RegisterBuildCallback(async (IObjectResolver resolver, ConfigEntry _, MainContainer _) =>
        {
            resolver.Inject(container);
            await DockTextures.Update();
            container.Initialize();
        });
    }
}