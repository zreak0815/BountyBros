using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour {

    public float destroyTimer = 0;
	
	// Update is called once per frame
	void Update () {
        if (destroyTimer <= 0) {
            Destroy(gameObject);
        }
        destroyTimer -= 1;
	}
}
