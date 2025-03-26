using System;
using Il2CppInterop.Runtime.Attributes;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.ImGui;
using UnityEngine;

namespace TORW;

[RegisterInIl2Cpp]
public class DebugWindow(IntPtr cppPtr) : MonoBehaviour(cppPtr)
{
    [HideFromIl2Cpp]
    public DragWindow DebuggingWindow { get; } = new(
        new Rect(10, 10, 0, 0),
        "TOR-W DEBUGGING MENU",
        () =>
        {
        })
    {
        Enabled = true,
    };

    public void OnGUI()
    {
        DebuggingWindow.OnGUI();
    }
}
