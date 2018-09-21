using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Abstrakte Klasse für den Kampf
     */
public abstract class CombatBase : MonoBehaviour {

    protected CombatManager combatManager;
    protected Vector2 origin;

    public enum MoveState { idle, approach, retreat };
    protected MoveState moveState = MoveState.idle;

    public CombatBase target;

    public CollisionBox collisionBox;

    protected void Start() {
        combatManager = FindObjectOfType<CombatManager>();
        origin = transform.position;
    }

    public void setTarget( bool isPlayer) {
        if (isPlayer) {
            target = combatManager.enemyCombat;
        } else {
            target = combatManager.playerCombat;
        }
    }

    public abstract void performAttackAnimation(int id);
}
