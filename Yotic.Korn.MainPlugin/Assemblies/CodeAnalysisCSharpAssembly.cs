using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis.CSharp;
using Korn.Plugins.Core.Interfaces;
using System.Collections.Generic;
using Korn.Hooking;
using System;

unsafe class CodeAnalysisCSharpAssembly : IHookImplemention
{
    public CodeAnalysisCSharpAssembly()
    {
        this
        .AddHook((Func<string, SyntaxKind>)SyntaxFacts.GetKeywordKind, (GetKeywordKindDelegate)GetKeywordKind)
        .AddHook((Func<SyntaxKind, string>)SyntaxFacts.GetText, (GetTextDelegate)GetText)
        .EnableHooks();
    }

    public List<MethodHook> Hooks { get; private set; } = new List<MethodHook>();

    delegate bool GetKeywordKindDelegate(ref string text, ref SyntaxKind result);
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

    delegate bool GetTextDelegate(ref SyntaxKind kind, ref string result);
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
}