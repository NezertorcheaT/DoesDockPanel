using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer.Unity;

[UsedImplicitly]
public class TransparentWindow : ITickable
{
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
        uint uFlags);

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32.dll")]
    private static extern bool UpdateWindow(IntPtr hWnd);

    private const int GWL_EXSTYLE = -20;
    private const uint WS_EX_LAYERED = 0x00080000;
    private const uint WS_EX_TRANSPARENT = 0x00000020;
    private const uint WS_EX_NOACTIVATE = 0x08000000;
    private const int LWA_ALPHA = 0x00000002;
    private const int SWP_NOSIZE = 0x0001;
    private const int SWP_NOMOVE = 0x0002;
    private const int SWP_SHOWWINDOW = 0x0040;
    private static readonly IntPtr HWND_BOTTOM = new(1);

    [DllImport("user32.dll")]
    private static extern IntPtr SetActiveWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    private static IntPtr _currentWindow = IntPtr.Zero;
    private static uint _initialStyle;
    private static readonly List<RaycastResult> Rl = new();

    private static IntPtr GetUnityWindow()
    {
        if (_currentWindow == IntPtr.Zero)
            _currentWindow = Process.GetCurrentProcess().MainWindowHandle;
        _initialStyle = GetWindowLong(_currentWindow, GWL_EXSTYLE);
        return _currentWindow;
    }

    // ReSharper disable once UnusedMember.Local
    [UsedImplicitly]
    public static void NotEditor()
    {
        GetUnityWindow();
        SetActiveWindow(_currentWindow);
        var margins = new MARGINS { cxLeftWidth = -1, cxRightWidth = -1, cyBottomHeight = -1, cyTopHeight = -1 };
        SetWindowLong(_currentWindow, GWL_EXSTYLE, _initialStyle | WS_EX_LAYERED | WS_EX_NOACTIVATE);
        SetWindowPos(_currentWindow, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
        DwmExtendFrameIntoClientArea(_currentWindow, ref margins);
        UpdateWindow(_currentWindow);
        SetActiveWindow(_currentWindow);
    }

    // ReSharper disable once UnusedMember.Local
    [UsedImplicitly]
    private void NotEditorUpdate()
    {
        EventSystem.current.RaycastAll(
            new PointerEventData(EventSystem.current) { position = Mouse.current.position.ReadValue() }, Rl);
        SetWindowLong(_currentWindow, GWL_EXSTYLE,
            _initialStyle | WS_EX_NOACTIVATE | (Rl.Count == 0
                ? WS_EX_LAYERED | WS_EX_TRANSPARENT
                : WS_EX_LAYERED
            )
        );
    }

    void ITickable.Tick()
    {
#if !UNITY_EDITOR
        NotEditorUpdate();
#endif
    }
}