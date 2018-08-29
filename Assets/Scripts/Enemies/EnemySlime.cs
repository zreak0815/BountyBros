using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Klasse für einen Schleim Gegner
 */
public class EnemySlime : EnemyBase {

    //Timer für Bewegung
    //Bei 0 wird der Bewegungsstatus geändert
    private float moveTimer = 0;

    //Bewegungsgeschwindigkeit
    private float movementSpeed = 0.03f;

    //Animator
    private Animator anim;

    //Die Art der Bewegung
    private enum MoveState { standing, moving_right, moving_left };
    private MoveState movement;

    //ob sich das Objekt auf dem Boden befindet
    private bool grounded = false;

    //Lebenspunkte
    private int HP = 100;

    void Awake() {
        anim = GetComponent<Animator>();
        this.tag = "Slime";
    }

    public int getHP()
    {
        return this.HP;
    }
	
	// Update is called once per frame
	void Update () {
        moveTimer -= Time.deltaTime;

        grounded = collisionBox.isGrounded();

        collisionBox.setHSpeed(0);

        if (grounded) {
            //Gegner is auf dem Boden
            collisionBox.setVSpeed(0);
        } else {
            //Gegner fällt
            collisionBox.setVSpeed(Mathf.Max(collisionBox.velocity.y - gravity, -fallSpeed));
        }

        if (movement != MoveState.standing) {
            //Prüfen ob der Gegner aus der Reichweite seines Homes ist
            if (Mathf.Abs((transform.position - home.transform.position).x) >= home.pointRange.x
                && ((transform.position.x > home.transform.position.x && movement == MoveState.moving_right)
                || transform.position.x < home.transform.position.x && movement == MoveState.moving_left)) {
                    moveTimer = 0;

            } else {

                //Gegner auf Bewegungsrichtung ausrichten
                Vector3 scale = transform.localScale;

                if (movement == MoveState.moving_right) {
                    collisionBox.setHSpeed(movementSpeed);
                    scale.x = Mathf.Abs(scale.x);
                } else {
                    collisionBox.setHSpeed(-movementSpeed);
                    scale.x = -Mathf.Abs(scale.x);
                }
                
                transform.localScale = scale;
            }
        }

        //Bewegungsart ändert
        if (moveTimer <= 0) {
            if (movement == MoveState.standing) {
                moveTimer = Random.Range(0.5f, 3.0f);
                if (Mathf.Abs((transform.position - home.transform.position).x) >= home.pointRange.x) {
                    if (transform.position.x > home.transform.position.x) {
                        movement = MoveState.moving_left;
                    } else {
                        movement = MoveState.moving_right;
                    }
                }
                else {
                    movement = Random.value > 0.5f ? MoveState.moving_right : MoveState.moving_left;
                }
            }
            else {
                moveTimer = Random.Range(0.5f, 2.0f);
                movement = MoveState.standing;
            }

        }

        collisionBox.movement();
    }
}
