using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHero : CombatBase {

    private int performingAttack = 0;
    public float moveSpeed = 0.4f;

    private float approachDistance = 0;

    public Animator animator;

    private enum anim { standing, walking, attacking, bow, jump, stomp, hurt, defeated };
    private anim battleAnimation = anim.standing;
    private int animationFrames = 0;
    private string[] triggers = { "Standing", "Walking", "Attacking", "Bow", "Jump", "Stomp", "Hurt", "Defeated" };

    public GameObject arrowObject;

    private int hurt = 0;
    private bool defeated = false;

    public void Update() {

        anim lastAnim = battleAnimation;

        battleAnimation = anim.standing;

        if (hurt > 0) {
            hurt -= 1;
            battleAnimation = anim.hurt;
        }
        if (defeated) {
            battleAnimation = anim.defeated;
        }
        if (performingAttack == 1 || performingAttack == 2) {
            switch(moveState) {
                case MoveState.approach:
                    if (Vector2.Distance(transform.position, target.transform.position) > approachDistance) {
                        collisionBox.setHSpeed(moveSpeed);
                    }
                    else {
                        collisionBox.setHSpeed(0);
                        moveState = MoveState.idle;
                        animationFrames = 0;
                    }
                    battleAnimation = anim.walking;
                    break;
                case MoveState.idle:
                    if (animationFrames == 4) {
                        combatManager.dealDamage();
                    }
                    if (animationFrames >= 16) {
                        moveState = MoveState.retreat;
                        approachDistance = 0.5f;
                    }
                    battleAnimation = anim.attacking;
                    break;
                case MoveState.retreat:
                    if (Vector2.Distance(transform.position, origin) > approachDistance) {
                        collisionBox.setHSpeed(-moveSpeed);
                    }
                    else {
                        collisionBox.setHSpeed(0);
                        transform.position = origin;
                        moveState = MoveState.idle;
                        performingAttack = 0;
                        combatManager.endTurn();
                    }
                    battleAnimation = anim.walking;
                    break;
            }
        }

        if (performingAttack == 3) {
            switch (moveState) {
                case MoveState.idle:
                    if (animationFrames == 20) {
                        GameObject arrow = Instantiate(arrowObject, transform.position + new Vector3(0.1f, -0.15f), new Quaternion());
                        arrow.GetComponent<CombatProjectile>().target = target.transform.position;
                    }
                    if (animationFrames >= 50) {
                        performingAttack = 0;
                        combatManager.endTurn();
                    }
                    battleAnimation = anim.bow;
                    break;
            }
        }

        if (performingAttack == 4) {
            switch (moveState) {
                case MoveState.approach:
                    if (Vector2.Distance(transform.position, target.transform.position + Vector3.up * 7) > approachDistance) {
                        collisionBox.setSpeed(((target.transform.position + Vector3.up * 7) - transform.position).normalized * 0.8f);
                    }
                    else {
                        collisionBox.setSpeed(Vector2.zero);
                        moveState = MoveState.idle;
                        approachDistance = 2;
                    }
                    battleAnimation = anim.jump;
                    break;
                case MoveState.idle:
                    if (Vector2.Distance(transform.position, target.transform.position) > approachDistance) {
                        collisionBox.setSpeed(((target.transform.position) - transform.position).normalized * 2);
                    }
                    else {
                        collisionBox.setSpeed(new Vector2(-moveSpeed * 0.3f, 0.5f));
                        moveState = MoveState.retreat;
                        approachDistance = moveSpeed;
                        combatManager.dealDamage();
                    }
                    battleAnimation = anim.stomp;
                    break;
                case MoveState.retreat:
                    if (Vector2.Distance(transform.position, origin) > approachDistance) {
                        if (collisionBox.isGrounded()) {
                            collisionBox.setHSpeed(-moveSpeed);
                            collisionBox.setVSpeed(0);
                            battleAnimation = anim.walking;
                        } else {
                            collisionBox.setHSpeed(-moveSpeed * 0.3f);
                            collisionBox.setVSpeed(collisionBox.velocity.y - 0.03f);
                            battleAnimation = anim.jump;
                        }
                        
                    }
                    else {
                        collisionBox.setHSpeed(0);
                        transform.position = origin;
                        moveState = MoveState.idle;
                        performingAttack = 0;
                        combatManager.endTurn();
                    }
                    
                    break;
            }
        }

        animationFrames++;
        if (lastAnim != battleAnimation) {
            animationFrames = 0;
            foreach (string trigger in triggers) {
                animator.ResetTrigger(trigger);
            }
            animator.SetTrigger(triggers[(int)battleAnimation]);
        }

        collisionBox.movement();
    }

    public override void performAttackAnimation(int id) {
        animationFrames = 0;
        if (performingAttack == 0) {
            switch (id) {
                case 1:
                    performingAttack = 1;
                    moveState = MoveState.approach;
                    approachDistance = 2;
                    break;
                case 2:
                    performingAttack = 1;
                    moveState = MoveState.approach;
                    approachDistance = 2;
                    break;
                case 3:
                    performingAttack = 3;
                    break;
                case 4:
                    performingAttack = 4;
                    moveState = MoveState.approach;
                    approachDistance = 1;
                    break;
            }
        }
    }

    public override void playHurtAnimation() {
        hurt = 30;
    }

    public override void playDefeatAnimation() {
        defeated = true;
    }
}
