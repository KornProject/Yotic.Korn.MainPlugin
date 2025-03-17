using Korn.Plugins.Core;
using System.Reflection;
using Korn;

class MainPlugin : Plugin
{    
    static MainPlugin instance;
    public static new KornLogger Logger => ((Plugin)instance).Logger;

    protected override void OnLoad()
    {
        instance = this;
        Logger.WriteMessage($"MainPlugin started in process \"{CoreEnv.CurrentProcess.ProcessName}\"({CoreEnv.CurrentProcess.Id})");

        RegisterAssemblyLoad("Microsoft.CodeAnalysis.CSharp", OnLoadAssembly_CSharpAnalysis);
    }

    protected override void OnUnload()
    {
        if (codeAnalysisCSharAssembly != null)
            codeAnalysisCSharAssembly.Dispose();
    }

    protected override void OnAssemblyLoaded(Assembly assembly)
    {

    }

    CodeAnalysisCSharpAssembly codeAnalysisCSharAssembly;
    void OnLoadAssembly_CSharpAnalysis(Assembly assembly)
    {
        codeAnalysisCSharAssembly = new CodeAnalysisCSharpAssembly();
    }
}