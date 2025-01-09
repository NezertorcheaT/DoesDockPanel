using UnityEngine;
using UnityEngine.Rendering;

#if !UNITY_EDITOR
public class SkipSplash
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void BeforeSplashScreen()
    {
        Application.runInBackground = true;
        TransparentWindow.NotEditor();
        System.Threading.Tasks.Task.Run(AsyncSkip);
    }

    private static void AsyncSkip()
    {
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }
}
#endif