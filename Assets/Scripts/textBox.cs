using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textBox : MonoBehaviour {

    public Text textObj;
    public float activationRange;
    public string[] texts;

    private bool opened = false;
    private int count = 0;
    private SimplePlatformController hero;

    // Use this for initialization
    void Start () {
		hero = FindObjectOfType<SimplePlatformController>();
    }
	
	// Update is called once per frame
	void Update () {
		if (!opened)
        {
            opened = !opened;
            if (Mathf.Abs(hero.transform.position.x - transform.position.x) < activationRange &&
                Mathf.Abs(hero.transform.position.y - transform.position.y) < activationRange)
            {
                //show textBox
                textObj.text = texts[count];

                count++;
            }
        }

        //wait for player to close textBox
        if (opened)
        {
            if (Input.GetButtonDown("Attack"))
            {

            }
        }
	}
}
