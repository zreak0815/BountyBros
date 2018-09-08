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
    //Das Objekt das Zerstört werden soll
    public GameObject destroyObject;

    public Collider2D hurtbox;

    private float invincibility = 0;

    /// <summary>
    /// Beschädigt das Objekt und zerstört es
    /// </summary>
    /// <param name="damage">Schaden für das Objekt</param>
    public void dealDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            if (destroyObject == null) {
                Destroy(gameObject);
            } else {
                Destroy(destroyObject);
            }
        }
    }

    private void Update() {
        invincibility = Mathf.Max(0, invincibility - 1);
        if (attack && invincibility == 0) {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(1 << LayerMask.NameToLayer("PlayerHitbox"));
            Collider2D[] results = new Collider2D[1];
            hurtbox.OverlapCollider(filter, results);
            if (results[0] != null) {
                invincibility = 5;
                PlayerOverworldHitbox hit = results[0].gameObject.GetComponent<PlayerOverworldHitbox>();
                dealDamage(hit.damage);
            }
        }
    }

}
