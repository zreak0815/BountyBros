using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textBox : MonoBehaviour {

    public Canvas can;
    public Text textObj;
    public float activationRange;
    public string[] text;

    private bool opened = false;
    private SimplePlatformController hero;
    private int count;

    // Use this for initialization
    void Start () {
		hero = FindObjectOfType<SimplePlatformController>();
        can.enabled = false;
        count = 0;
    }
	
	// Update is called once per frame
	void Update () {
		if (!opened)
        {
           print("test");
            
            if (Mathf.Abs(hero.transform.position.x - transform.position.x) < activationRange &&
                Mathf.Abs(hero.transform.position.y - transform.position.y) < activationRange)
            {
                opened = !opened;

                hero.textBoxTriggered();
                //set text for textBox
                textObj.text = text[count++];
                //show textBox
                toggleTextbox();
            }
        }

        //wait for player to close textBox
        if (opened)
        {
            if (Input.GetButtonDown("Attack"))
            {
                if (count < text.Length)
                {
                    textObj.text = text[count++];
                } else
                {
                    toggleTextbox();
                    hero.textBoxTriggered();
                    Destroy(gameObject);
                }
                
            }
        }
	}

    private void toggleTextbox()
    {
        can.enabled = !can.enabled;
    }
}
