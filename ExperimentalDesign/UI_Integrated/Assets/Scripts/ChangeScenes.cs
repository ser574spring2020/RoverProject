using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScenes : MonoBehaviour
{

    public void changeUIScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void changeExperimentalScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

}
