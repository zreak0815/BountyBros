using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * eine Klasse, die die Kamera steuert
 */
public class Camera : MonoBehaviour {

    //Referenz auf die Hauptfigur, der gefolgt werden soll
    public GameObject hero;
    //Geschwindigkeit der Kamera
    public float followSpeed = 0.1f;
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, hero.transform.position + Vector3.back, followSpeed);
	}
}
