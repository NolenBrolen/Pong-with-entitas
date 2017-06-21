using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using System;

public class CollisionAngle : MonoBehaviour
{
    public string test;

    void OnCollision2D(Collision2D col)
    {

        //col.gameObject
        //distinguish if you hit racket1 or racket2
        //if ()
        //context.CreateEntity();
        //AddCollision(gameobject, player1)

        //we create a new component called CollisionComponent
        //add data to our component, a GameObject variable called owner
        //and GameObject called partnerinCrime

        //then we come to our monobehaviour with our Collision2D method
        //the only thing we need to declare in the method is:
        //from the contexts class we get access to sharedinstance which then gives access to the gamecontext or any other context like InputContext
        //then we create on the declared context, in this case gamecontext, a new entity to which we add a collisioncomponent that we created earlier
        //This takes the amount of parameters as the amount of fields we declared in the Collision component
        //we pass in our parameters: the gameobject this is attached to and the gameobject that is collided with

        //we can write one big ass chain that's hard to understand or

        // Contexts.sharedInstance.game.CreateEntity().AddCollision(gameObject, col.gameObject);

        //we can break it up into steps

        GameContext context = Contexts.sharedInstance.game;
        var e = context.CreateEntity();
        e.AddCollision(gameObject, col.gameObject);

    }
}

