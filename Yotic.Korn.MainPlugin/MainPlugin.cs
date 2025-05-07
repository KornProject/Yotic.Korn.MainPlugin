using Korn.Plugins.Core;
using System.Reflection;
using Korn;
using System;
using System.Linq;

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
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Logger.WriteMessage($"ass: {assemblies.Length}, last: {assemblies.Last().GetName().Name}, in: {assembly.GetName().Name}");
        codeAnalysisCSharAssembly = new CodeAnalysisCSharpAssembly();
    }
}