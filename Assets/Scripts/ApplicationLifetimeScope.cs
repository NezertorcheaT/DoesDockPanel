using CustomHelper;
using Input;
using MiddleSpawn;
using VContainer;
using VContainer.Unity;
using PrimeTween;
using Saving.Settings;

public class ApplicationLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        builder.RegisterEntryPoint<ConfigEntry>().AsSelf();
        builder.RegisterEntryPoint<TransparentWindow>().AsSelf();
        builder.RegisterEntryPoint<WindowsInputActions>().AsSelf();
        builder.RegisterEntryPoint<MainContainer>().AsSelf();
        builder.RegisterBuildCallback((IObjectResolver r, ConfigEntry e) =>
            _ = DockTextures.Update());
    }
}