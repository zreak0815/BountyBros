using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 Klasse für zerstörbare Objekte
 */
public class HittableItem : MonoBehaviour {

    public int itemNumber;

    private bool collected = false;

    public Collider2D hurtbox;
    private PlayerValues playerValues;
    private float pickupDistance = 1;
    
    void Start()
    {
        playerValues = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerValues>();
    }

    // Update is called once per frame
    void Update () {
        if (!collected) {
            if (Vector2.Distance(transform.position, playerValues.transform.position) <= pickupDistance) {
                int amount = 2;
                playerValues.changeHPFlaskAmount(amount);
                print(amount.ToString() + " Items collected!");
                collected = true;
                FindObjectOfType<CombatManager>().showText(transform.position, "+" + amount + " Heiltränke!", Color.green);
                Destroy(hurtbox.gameObject);
            } 
        }
    }
}
