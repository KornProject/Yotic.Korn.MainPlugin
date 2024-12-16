using Microsoft.CodeAnalysis.CSharp;

class CodeAnalysisCSharpAssembly : IDisposable
{
    public CodeAnalysisCSharpAssembly()
    {
        MainPlugin.Logger.WriteMessage("CodeAnalysisCSharpAssembly->.ctor");

        var kind = SyntaxFacts.GetKeywordKind("return");
        MainPlugin.Logger.WriteMessage("kind: " + kind);
    }

    public void Dispose()
    {

    }
}