using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Klasse für das Beenden der Anwendung
     */ 
public class QuitOnClick : MonoBehaviour {

public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
