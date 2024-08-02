using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Manager : MonoBehaviour
{
    public SceneTransitionData sceneTransitionData; 

    public void GoToScene1()
    {
        sceneTransitionData.fromScene2 = true;
  
    }
}
