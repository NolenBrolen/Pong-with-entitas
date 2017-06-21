using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameButtonScript : MonoBehaviour
{
    public void quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
