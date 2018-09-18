using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatProjectile : MonoBehaviour {

    public float projectileSpeed = 1;
    public Vector2 target;
    public CollisionBox collisionBox;

    private CombatManager combatManager;

	// Use this for initialization
	void Start () {
        combatManager = FindObjectOfType<CombatManager>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 move = (target - (Vector2)transform.position).normalized;
        collisionBox.setSpeed(move * projectileSpeed);
        transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, move), Vector3.forward);
        if (Vector2.Distance(transform.position, target) <= projectileSpeed) {
            combatManager.dealDamage();
            Destroy(gameObject);
        }

        collisionBox.movement();
	}
}
