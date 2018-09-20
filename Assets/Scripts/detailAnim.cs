using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detailAnim : MonoBehaviour {

    public Sprite[] sprites;

    public SpriteRenderer rend;

    private float count = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rend.sprite = sprites[(int)(count % sprites.Length)];
        count += 0.1f;
	}
}
