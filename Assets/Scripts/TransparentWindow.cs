using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
#if !UNITY_EDITOR
using UnityEngine.InputSystem;
#endif
using VContainer.Unity;

public class TransparentWindow : IStartable, ITickable
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

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

    const int GWL_EXSTYLE = -20;
    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;
    const uint WS_EX_NOACTIVATE = 0x08000000;
    static readonly IntPtr HWND_TOPMOST = new(1);
    private IntPtr hWnd;

    // ReSharper disable once UnusedMember.Local
    private void NotEditor()
    {
        hWnd = GetActiveWindow();

        var margins = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);
        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_NOACTIVATE);
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
    }

    void IStartable.Start()
    {
        _rl = new List<RaycastResult>();
#if !UNITY_EDITOR
        NotEditor();
#endif
        Application.runInBackground = true;
    }

    private List<RaycastResult> _rl;

    void ITickable.Tick()
    {
#if !UNITY_EDITOR
        EventSystem.current.RaycastAll(
            new PointerEventData(EventSystem.current) { position = Mouse.current.position.ReadValue() }, _rl);
        SetWindowLong(hWnd, GWL_EXSTYLE,
            _rl.Count <= 0
                ? WS_EX_LAYERED | WS_EX_NOACTIVATE | WS_EX_TRANSPARENT
                : WS_EX_LAYERED | WS_EX_NOACTIVATE
        );
#endif
    }
}