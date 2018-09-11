using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour {

    public int timer = 0;
	
	// Update is called once per frame
	void Update () {
        
        if (timer <= 0) {
            Destroy(gameObject);
        }
        timer -= 1;
    }
}
