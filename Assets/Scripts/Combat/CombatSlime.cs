using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSlime : CombatBase {

    private int performingAttack = 0;
    public float moveSpeed = 0.4f;

    private float approachDistance = 0;

    public Animator animator;

    private enum anim { standing, walking, attacking, hurt, defeated };
    private anim battleAnimation = anim.standing;
    private int animationFrames = 0;
    private string[] triggers = { "Standing", "Walking", "Attacking", "Hurt", "Defeated" };

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
                    if (Vector2.Distance(transform.position, target.transform.position) > approachDistance) {
                        collisionBox.setHSpeed(-moveSpeed);
                    }
                    else {
                        collisionBox.setHSpeed(0);
                        moveState = MoveState.idle;
                        animationFrames = 0;
                    }
                    battleAnimation = anim.walking;
                    break;
                case MoveState.idle:
                    if (animationFrames == 20) {
                        combatManager.dealDamage();
                    }
                    if (animationFrames >= 33) {
                        moveState = MoveState.retreat;
                        approachDistance = 0.5f;
                    }
                    battleAnimation = anim.attacking;
                    break;
                case MoveState.retreat:
                    if (Vector2.Distance(transform.position, origin) > approachDistance) {
                        collisionBox.setHSpeed(moveSpeed);
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
