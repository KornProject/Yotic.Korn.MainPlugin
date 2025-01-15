using Korn;
using Korn.Plugins.Core;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

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
        try
        {
            if (name == "Microsoft.CodeAnalysis.CSharp")
            {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                codeAnalysisCSharoAssenbly = new CodeAnalysisCSharpAssembly();
            }
        } 
        catch (Exception ex)
        {
            KornLogger.Error(ex.ToString());
        }
    }

    Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
    {
        return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == args.Name.Split(", ")[0]);
    }
}