using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : EnemyBase {

    //Timer für Bewegung
    //Bei 0 wird der Bewegungsstatus geändert
    private float moveTimer = 0;

    //Bewegungsgeschwindigkeit
    private float movementSpeed = 0.05f;

    private Vector3 moveTarget;

    //Animator
    private Animator anim;

    //Die Art der Bewegung
    private enum MoveState { stationary, moving };
    private MoveState movement;

    private void Awake() {
        fullHP = 25;
        HP = fullHP;

        basicAttackDamage = 6;
        defence = 0;
        evasion = 0.2f;

        anim = GetComponent<Animator>();
        tag = "Bat";
    }

    // Update is called once per frame
    void Update() {
        moveTimer -= Time.deltaTime;

        if (movement == MoveState.stationary) {

            if (moveTimer <= 0) {
                movement = MoveState.moving;

                moveTarget = home.transform.position + Vector3.right * Random.Range(-1.0f, 1.0f) * home.pointRange.x + Vector3.up * Random.Range(-1.0f, 1.0f) * home.pointRange.y;
                collisionBox.setSpeed((moveTarget - transform.position).normalized * movementSpeed);
                moveTimer = 20;

                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (collisionBox.velocity.x > 0 ? 1 : -1), transform.localScale.y, transform.localScale.z);
            }
        } else {

            print(Vector3.Distance(moveTarget, transform.position));
            if (Vector3.Distance(moveTarget, transform.position) <= 0.1f || moveTimer <= 0) {
                movement = MoveState.stationary;
                moveTimer = Random.Range(0.5f, 2.0f);
                collisionBox.setSpeed(Vector2.zero);
            }

        }

        collisionBox.movement();
    }

    public override enemyType GetEnemyType() {
        return enemyType.BAT;
    }
}
