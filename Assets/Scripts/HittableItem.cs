using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableItem : MonoBehaviour {

    public int itemNumber;
    public int minAmount;
    public int maxAmount;

    private bool collected = false;

    public Collider2D hurtbox;

    // Update is called once per frame
    void Update () {
        if (!collected) {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(1 << LayerMask.NameToLayer("PlayerHitbox"));
            Collider2D[] results = new Collider2D[1];
            hurtbox.OverlapCollider(filter, results);
            if (results[0] != null) {
                int amount = Random.Range(minAmount, maxAmount + 1);
                //TODO Items einsammeln
                print(amount.ToString() + " Items collected!");
                collected = true;
            }
        }
    }
}
