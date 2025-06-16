using Korn.Plugins.Core;
using System.Reflection;
using Korn.Utils;
using Korn;

class MainPlugin : Plugin
{    
    static MainPlugin instance;
    public static new KornLogger Logger => ((Plugin)instance).Logger;

    protected override void OnLoad()
    {
        instance = this;
        Logger.WriteMessage($"MainPlugin started in process \"{Process.Current.Name}\"({Process.Current.ID})");

        RegisterAssemblyLoad("Microsoft.CodeAnalysis.CSharp", OnLoadAssembly_CSharpAnalysis);
    }

    CodeAnalysisCSharpAssembly codeAnalysisCSharAssembly;
    void OnLoadAssembly_CSharpAnalysis(Assembly assembly)
    {
        codeAnalysisCSharAssembly = new CodeAnalysisCSharpAssembly();
    }
}