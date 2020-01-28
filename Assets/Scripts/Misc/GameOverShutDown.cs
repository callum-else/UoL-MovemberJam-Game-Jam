using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverShutDown : MonoBehaviour
{
    public void EndGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
