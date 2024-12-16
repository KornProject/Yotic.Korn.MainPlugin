using Korn;
using Korn.Plugins.Core;
using System.Diagnostics.CodeAnalysis;

class MainPlugin : Plugin
{
    [AllowNull] public static LocalLogger Logger;
    [AllowNull] public static string DataDirectory;
    [AllowNull] static AssemblyWatcher assemblyWatcher;

    public override void OnLoad()
    {
        Initialize();

        assemblyWatcher = new AssemblyWatcher();
        assemblyWatcher.AssemblyLoaded += OnAssemblyLoaded;
        assemblyWatcher.EnsureAllAssembliesLoaded();

        void Initialize()
        {
            DataDirectory = Path.Combine(PluginDirectory, "Data");
            if (!Directory.Exists(DataDirectory))
                Directory.CreateDirectory(DataDirectory);

            var logFile = Path.Combine(DataDirectory, "log.txt");
            Logger = new LocalLogger(logFile);

            Logger.WriteMessage("MainPlugin initialized");
            KornLogger.WriteMessage("message!");
        }
    }

    public override void OnUnload()
    {
        assemblyWatcher.Dispose();

        if (codeAnalysisCSharoAssenbly is not null)
            codeAnalysisCSharoAssenbly.Dispose();
    }

    CodeAnalysisCSharpAssembly? codeAnalysisCSharoAssenbly;
    void OnAssemblyLoaded(string name)
    {
        if (name == "Microsoft.CodeAnalysis.CSharp")
            codeAnalysisCSharoAssenbly = new CodeAnalysisCSharpAssembly();
    }
}