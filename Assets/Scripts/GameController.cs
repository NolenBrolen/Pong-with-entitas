using Entitas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameController : MonoBehaviour
{
    public RectTransform canvas;

    Systems _systems;

    void Start()
    {
        // get a reference to the contexts
        var contexts = Contexts.sharedInstance;
        contexts.game.SetUiRoot(canvas);
        _systems = AllSystems.Get(contexts);  


        // call Initialize() on all of the IInitializeSystems
        _systems.Initialize();
    }

    void Update()
    {
        // call Execute() on all the IExecuteSystems and 
        // ReactiveSystems that were triggered last frame
        
        //PAUSE SYSTEM PSEUDOCODE
        //if(pausedComponent is not there)
        _systems.Execute();
        //else 
        // execute a seperate resume system


        // call cleanup() on all the ICleanupSystems
        _systems.Cleanup();
    }
}
