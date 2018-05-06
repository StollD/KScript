using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace KScript
{
    /// <summary>
    /// Calls the KSPHook functions
    /// </summary>
    public abstract class HookCaller : MonoBehaviour
    {
        /// <summary>
        /// All the hooks that are currently loaded
        /// </summary>
        public static Dictionary<ScriptScene, Dictionary<ScriptEvent, List<MethodInfo>>> Hooks;

        /// <summary>
        /// Create the hook registry
        /// </summary>
        static HookCaller()
        {
            Hooks = new Dictionary<ScriptScene, Dictionary<ScriptEvent, List<MethodInfo>>>();
            foreach (Object scene in Enum.GetValues(typeof(ScriptScene)))
            {
                Hooks.Add((ScriptScene)scene, new Dictionary<ScriptEvent, List<MethodInfo>>());
                foreach (Object evt in Enum.GetValues(typeof(ScriptEvent)))
                {
                    Hooks[(ScriptScene)scene].Add((ScriptEvent)evt, new List<MethodInfo>());
                }
            }
        }

        /// <summary>
        /// The scene where this monobehaviour runs
        /// </summary>
        public abstract ScriptScene Scene { get; }

        protected virtual void Awake()
        {
            Hooks[Scene][ScriptEvent.Awake].ForEach(m => m.Invoke(null, null));
        }
        protected virtual void Start()
        {
            Hooks[Scene][ScriptEvent.Start].ForEach(m => m.Invoke(null, null));
        }
        protected virtual void Update()
        {
            Hooks[Scene][ScriptEvent.Update].ForEach(m => m.Invoke(null, null));
        }
        protected virtual void FixedUpdate()
        {
            Hooks[Scene][ScriptEvent.FixedUpdate].ForEach(m => m.Invoke(null, null));
        }
        protected virtual void LateUpdate()
        {
            Hooks[Scene][ScriptEvent.LateUpdate].ForEach(m => m.Invoke(null, null));
        }
        protected virtual void OnGUI()
        {
            Hooks[Scene][ScriptEvent.OnGUI].ForEach(m => m.Invoke(null, null));
        }
        protected virtual void OnDestroy()
        {
            Hooks[Scene][ScriptEvent.OnDestroy].ForEach(m => m.Invoke(null, null)); 
        }
    }
    
    [KSPAddon(KSPAddon.Startup.FlightEditorAndKSC, false)]
    public class __HookCallerFlightEditorAndKSC : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.FlightEditorAndKSC; }
        }
    }

    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
    public class __HookCallerAllGameScenes : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.AllGameScenes; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
    public class __HookCallerFlightAndEditor : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.FlightAndEditor; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.FlightAndKSC, false)]
    public class __HookCallerFlightAndKSC : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.FlightAndKSC; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class __HookCallerEveryScene : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.EveryScene; }
        }

        protected override void Awake()
        {
            if (HighLogic.LoadedScene == GameScenes.LOADING || HighLogic.LoadedScene == GameScenes.PSYSTEM)
            {
                DestroyImmediate(this);
            }
            else
            {
                base.Awake();
            }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class __HookCallerMainMenu : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.MainMenu; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.Settings, false)]
    public class __HookCallerSettings : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.Settings; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.Credits, false)]
    public class __HookCallerCredits : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.Credits; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class __HookCallerSpaceCentre : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.SpaceCentre; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class __HookCallerEditorAny : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.EditorAny; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.EditorSPH, false)]
    public class __HookCallerEditorSPH : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.EditorSPH; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.EditorVAB, false)]
    public class __HookCallerEditorVAB : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.EditorVAB; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class __HookCallerFlight : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.Flight; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class __HookCallerTrackingStation : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.TrackingStation; }
        }
    }
    
    [KSPAddon(KSPAddon.Startup.PSystemSpawn, false)]
    public class __HookCallerPSystemSpawn : HookCaller
    {
        public override ScriptScene Scene
        {
            get { return ScriptScene.PSystemSpawn; }
        }
    }
}