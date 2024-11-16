using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer.Unity;

[UsedImplicitly]
public class TransparentWindow : IStartable, ITickable
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

    private const int GWL_EXSTYLE = -20;
    private const uint WS_EX_LAYERED = 0x00080000;
    private const uint WS_EX_TRANSPARENT = 0x00000020;
    private const uint WS_EX_NOACTIVATE = 0x08000000;
    private static readonly IntPtr HWND_BOTTOM = new(1);

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string className, string windowName);

    [DllImport("user32.dll")]
    private static extern IntPtr SetActiveWindow(IntPtr hWnd);

    private static IntPtr _currentWindow = IntPtr.Zero;

    private static IntPtr GetUnityWindow()
    {
        if (_currentWindow == IntPtr.Zero)
            _currentWindow = FindWindow(null, Application.productName);
        return _currentWindow;
    }

    // ReSharper disable once UnusedMember.Local
    private void NotEditor()
    {
        GetUnityWindow();
        SetActiveWindow(_currentWindow);
        var margins = new MARGINS { cxLeftWidth = -1 };
        SetWindowLong(_currentWindow, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_NOACTIVATE);
        SetWindowPos(_currentWindow, HWND_BOTTOM, 0, 0, 0, 0, 0);
        DwmExtendFrameIntoClientArea(_currentWindow, ref margins);
        SetActiveWindow(_currentWindow);
    }

    private List<RaycastResult> _rl;

    // ReSharper disable once UnusedMember.Local
    private void NotEditorUpdate()
    {
        EventSystem.current.RaycastAll(
            new PointerEventData(EventSystem.current) { position = Mouse.current.position.ReadValue() }, _rl);
        SetWindowLong(_currentWindow, GWL_EXSTYLE,
            _rl.Count <= 0
                ? WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE
                : WS_EX_LAYERED | WS_EX_NOACTIVATE
        );
    }

    void IStartable.Start()
    {
        _rl = new List<RaycastResult>();
#if !UNITY_EDITOR
        NotEditor();
#endif
        Application.runInBackground = true;
    }

    void ITickable.Tick()
    {
#if !UNITY_EDITOR
        NotEditorUpdate();
#endif
    }
}