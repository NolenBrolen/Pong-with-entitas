using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class Trigger : MonoBehaviour
{
    GameContext gameContext;
    
      void OnTriggerEnter2D(Collider2D other)
    {
        //Defining gameObject script is attached to as owner
        //Clarifying meaning of other
        GameObject owner = gameObject;
        GameObject triggerObject = other.gameObject;


        gameContext = Contexts.sharedInstance.game;
        var triggerEntity = gameContext.CreateEntity();
        triggerEntity.AddTrigger(owner, triggerObject);


        //ReactiveSystem will execute actual logic based on this script
    }
}
