using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Macht ein Objekt Zerstörbar durch eine Stampfattacke oder einen Angriff
 */
public class Destructable : MonoBehaviour {

    //Ob das Objekt durch eine Stampfattacke beschädigt werden kann
    public bool stomp = false;
    //Ob das Objekt durch eine Attacke beschädigt werden kann
    public bool attack = false;
    //Wie oft das Objekt beschädigt werden muss um es zu zerstören
    public int health = 1;

    /// <summary>
    /// Beschädigt das Objekt und zerstört es
    /// </summary>
    /// <param name="damage">Schaden für das Objekt</param>
    public void dealDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }

}
