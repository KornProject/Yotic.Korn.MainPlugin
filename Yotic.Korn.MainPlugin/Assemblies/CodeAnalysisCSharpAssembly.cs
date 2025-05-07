using Microsoft.CodeAnalysis.CSharp;
using Korn.Plugins.Core.Interfaces;
using System.Collections.Generic;
using Korn.Hooking;
using System;
using System.Runtime.CompilerServices;

unsafe class CodeAnalysisCSharpAssembly : IHookImplemention
{
    public CodeAnalysisCSharpAssembly()
    {
        this
        .AddHook((Func<string, SyntaxKind>)SyntaxFacts.GetKeywordKind, "GetKeywordKind")
        .AddHook((Func<SyntaxKind, string>)SyntaxFacts.GetText, "GetText")
        .EnableHooks();
    }

    List<MethodHook> hooks = new List<MethodHook>();
    public List<MethodHook> Hooks => hooks;

    [MethodImpl(MethodImplOptions.NoOptimization)]
    static bool GetKeywordKind(ref string text, ref SyntaxKind result)
    {
        if (text == "ретёрн")
        {
            result = SyntaxKind.ReturnKeyword;
            return false;
        }        

        return true;
    }

    [MethodImpl(MethodImplOptions.NoOptimization)]
    static bool GetText(ref SyntaxKind kind, ref string result)
    {        
        if (kind == SyntaxKind.ReturnKeyword)
        {
            result = "ретёрн";
            return false;
        }
                
        return true;
    }

    public void Dispose() => this.DisposeHooks();
}