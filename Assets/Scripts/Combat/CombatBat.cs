using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBat : CombatBase {

    public Animator animator;
    public float flySpeed = 0.4f;
    public float attackSpeed = 0.7f;

    private int performingAttack = 0;
    private float approachDistance = 0;

    private enum anim { standing, attacking, hurt, defeated };
    private anim battleAnimation = anim.standing;
    private int animationFrames = 0;
    private string[] triggers = { "Standing", "Attacking", "Hurt", "Defeated" };

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
        if (performingAttack == 1) {
            switch (moveState) {
                case MoveState.approach:
                    battleAnimation = anim.attacking;
                    if (animationFrames > 18) {
                        collisionBox.setSpeed((target.transform.position - transform.position).normalized * attackSpeed);
                    }
                    if (Vector2.Distance(transform.position, target.transform.position) < approachDistance) {
                        collisionBox.setSpeed(Vector2.zero);
                        moveState = MoveState.retreat;
                        animationFrames = 0;
                        combatManager.dealDamage();
                    }
                    break;
                case MoveState.retreat:
                    battleAnimation = anim.standing;
                    collisionBox.setSpeed((origin - (Vector2)transform.position).normalized * flySpeed);
                    if (Vector2.Distance(transform.position, origin) < approachDistance) {
                        collisionBox.setSpeed(Vector2.zero);
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
                    approachDistance = attackSpeed;
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
