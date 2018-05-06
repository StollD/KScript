using System;

namespace KScript
{
    /// <summary>
    /// An enumeration of scenes where a function can be active
    /// </summary>
    public enum ScriptScene
    {
        FlightEditorAndKSC,
        AllGameScenes,
        FlightAndEditor,
        FlightAndKSC,
        EveryScene,
        MainMenu,
        Settings,
        Credits,
        SpaceCentre,
        EditorAny,
        EditorSPH,
        EditorVAB,
        Flight,
        TrackingStation,
    }

    /// <summary>
    /// An enumeration of events in the MonoBehaviour lifecycle
    /// </summary>
    public enum ScriptEvent
    {
        Awake,
        Start,
        Update,
        FixedUpdate,
        LateUpdate,
        OnGUI,
        OnDestroy
    }
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class KSPHook : Attribute
    {
        /// <summary>
        /// The scene where the hook gets called
        /// </summary>
        public ScriptScene ScriptScene;

        /// <summary>
        /// The event where the hook gets called
        /// </summary>
        public ScriptEvent ScriptEvent;
        
        public KSPHook(ScriptScene scriptScene, ScriptEvent scriptEvent)
        {
            ScriptScene = scriptScene;
            ScriptEvent = scriptEvent;
        }
    }
}