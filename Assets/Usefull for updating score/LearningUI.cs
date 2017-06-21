using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearningUI : MonoBehaviour {

    public Text placeholderString;

    private string playerName = "Nolan";
    
	// Use this for initialization
	void Start () {

        placeholderString.text = "Name: " + playerName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

//when we have a canvas with a text child 
//we create a script that we attach to an emptygameobject
//that script has a public Text type variable
//we create a private string with the player name
//in our start function we assign the public text type variable to become whatever it is that is in the private string

 //how could we do this in entitas?

    //we have an entity that has a textcomponent and some other components with default string
    //then depending on a situation we change the component with default string value to value that we want
    //we would have a initialize system and reactivesystem to do this since we only need to change it once when a certain event happens