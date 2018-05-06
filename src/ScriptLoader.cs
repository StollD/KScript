using System;
using System.Linq;
using System.Reflection;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;
using UnityEngine;

namespace KScript
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class ScriptLoader : MonoBehaviour
    {
        /// <summary>
        /// All assemblies that are loaded by KSP
        /// </summary>
        private Assembly[] _lib;
        
        /// <summary>
        /// Register the callback for GameDatabase
        /// </summary>
        void Awake()
        {
            GameEvents.OnGameDatabaseLoaded.Add(OnGameDatabaseLoaded);
            
            // Redirect Console.WriteLine to Debug.Log so we can use the print statement
            Console.SetOut(new ConsoleRedirect());
        }

        /// <summary>
        /// Transform all yaml files in GameDatabase into ConfigNodes
        /// </summary>
        void OnGameDatabaseLoaded()
        {
            // Fetch all loaded assemblies
            _lib = AppDomain.CurrentDomain.GetAssemblies();
            
            // Add all scripts as build files
            CrawlGameDatabase(GameDatabase.Instance.root);
            
            // Fetch all static Methods
            MethodInfo[] methods = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                .ToArray();
            
            // Check them for KSPHooks
            for (Int32 i = 0; i < methods.Length; i++)
            {
                KSPHook[] hooks = methods[i].GetCustomAttributes(typeof(KSPHook), true) as KSPHook[];
                if (hooks == null)
                {
                    continue;
                }

                for (Int32 j = 0; j < hooks.Length; j++)
                {
                    HookCaller.Hooks[hooks[j].ScriptScene][hooks[j].ScriptEvent].Add(methods[i]);
                    Debug.Log("[KScript] Registered method " + methods[i].DeclaringType?.FullName + "." +
                              methods[i].Name + " for event " + hooks[j].ScriptScene + "." + hooks[j].ScriptEvent);
                }
            }
        }

        /// <summary>
        /// Iterates over the files in a directory and transforms .boo files to assemblies
        /// </summary>
        /// <param name="directory"></param>
        void CrawlGameDatabase(UrlDir directory)
        {
            for (Int32 i = 0; i < directory.files.Count; i++)
            {
                UrlDir.UrlFile file = directory.files[i];
                
                // Did we found a YAML file?
                if (file.fileExtension == "boo")
                {
                    // Compile the file
                    BooCompiler compiler = new BooCompiler();
                    compiler.Parameters.Pipeline = new CompileToMemory();
                    compiler.Parameters.Ducky = true;
                    compiler.Parameters.Input.Add(new FileInput(file.fullPath));
                    foreach (Assembly assembly in _lib)
                    {
                        compiler.Parameters.References.Add(assembly);
                    }
                    CompilerContext context = compiler.Run();
                    
                    // Check for errors
                    if (context.GeneratedAssembly != null)
                    {
                        Debug.Log("[KScript] Compiled " + file.url);
                    }
                    else
                    {
                        Debug.Log("[KScript] Compilation failes on file " + file.url);
                        foreach (CompilerError err in context.Errors)
                        {
                            Debug.Log(err.ToString());
                        }
                    }
                }
            }
            
            // Crawl the subdirectories of the directory
            for (Int32 i = 0; i < directory.children.Count; i++)
            {
                CrawlGameDatabase(directory.children[i]);
            }
        }
    }
}