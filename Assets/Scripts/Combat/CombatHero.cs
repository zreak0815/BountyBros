using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHero : CombatBase {

    private int performingAttack = 0;
    public float moveSpeed = 0.4f;

    private float approachDistance = 0;

    public Animator animator;

    private enum anim { standing, walking, attacking, bow };
    private anim battleAnimation = anim.standing;
    private int animationFrames = 0;
    private string[] triggers = { "Standing", "Walking", "Attacking", "Bow" };

    public GameObject arrowObject;

    public void Update() {

        anim lastAnim = battleAnimation;

        battleAnimation = anim.standing;
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
            }
        }
    }
}
