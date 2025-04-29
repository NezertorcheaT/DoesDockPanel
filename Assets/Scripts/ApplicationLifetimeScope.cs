using Input;
using VContainer;
using VContainer.Unity;
using PrimeTween;
using Saving.Settings;

public class ApplicationLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        builder.RegisterEntryPoint<ConfigEntry>();
        builder.RegisterEntryPoint<TransparentWindow>();
        builder.RegisterEntryPoint<WindowsInputActions>();
        builder.RegisterBuildCallback(r => _ = DockTextures.Update());
    }
}