using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverworldHitbox : MonoBehaviour {

    public int damage = 1;
    public Collider2D hitbox;

    private void Update() {

        RaycastHit2D enemy = Physics2D.CircleCast(transform.position, transform.lossyScale.x, Vector2.right, 0, 1 << LayerMask.NameToLayer("Enemies"));
        if (enemy.collider != null) {
            Debug.Log("enemy attacked!");

            FindObjectOfType<SimplePlatformController>().metEnemy(enemy.collider.GetComponent<EnemyOverworldHitbox>().owner, 1);

            Destroy(gameObject);
        }
    }
}
