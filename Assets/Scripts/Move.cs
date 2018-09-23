using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public Vector2 moveBy;
	
	// Update is called once per frame
	void Update () {
        transform.position += (Vector3)moveBy;
	}
}
