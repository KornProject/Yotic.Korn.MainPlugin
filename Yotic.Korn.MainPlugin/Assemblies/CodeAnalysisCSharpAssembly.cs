using Korn;
using Korn.CLR;
using Korn.Hooking;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;
using System.Runtime.InteropServices;

unsafe class CodeAnalysisCSharpAssembly : IDisposable
{
    public CodeAnalysisCSharpAssembly()
    {        
        hooks =
        [
            MethodHook.Create(SyntaxFacts.GetKeywordKind).AddHook(GetKeywordKind),
            MethodHook.Create((Func<SyntaxKind, string>)SyntaxFacts.GetText).AddHook(GetText),
        ];

        EnableHooks();
    }

    MethodHook[] hooks;

    static bool GetKeywordKind(ref string text, ref SyntaxKind result)
    {
        KornLogger.WriteMessage($"GetKeywordKind! {Process.GetCurrentProcess().ProcessName}");

        if (text == "ret")
        {
            result = SyntaxKind.ReturnKeyword;
            return false;
        }
        else if (text == "return")
        {
            result = SyntaxKind.None;
            return false;
        }

        return true;
    }

    static bool GetText(ref SyntaxKind kind, ref string result)
    {
        KornLogger.WriteMessage($"GetText! {Process.GetCurrentProcess().ProcessName}");
        if (kind == SyntaxKind.ReturnKeyword)
        {
            result = "ret";
            return false;
        }

        return true;
    }

    void EnableHooks()
    {
        foreach (var hook in hooks)
            hook.Enable();
    }

    void DisableHooks()
    {
        foreach (var hook in hooks)
            hook.Disable();
    }

    public void Dispose() => DisableHooks();
}