using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyType { SLIME, BAT };

/**
 * Eine abstrakte Klasse für einen Gegner
 */
public abstract class EnemyBase : MonoBehaviour {

    //Die Kollisionsbox für diesen Gegner
    public CollisionBox collisionBox;
    //Referenz für den Spawnpunkt des Gegners
    public EnemySpawnPoint home;
    //Maximale Fallgeschwindigkeit
    public float fallSpeed = 3;
    //Wie viel der Gegner pro Schritt nach unten beschleunigt
    public float gravity = 0.05f;

    //Lebenspunkte
    protected int fullHP = 40;
    protected int HP = 40;

    protected int xpReward = 50;

    protected int basicAttackDamage = 5;
    protected int defence = 0;
    protected float evasion = 0.1f;

    /// <summary>
    /// Setzt die Referenz für den Spawnpunkt
    /// </summary>
    /// <param name="home">Der Spawnpunkt</param>
    public void initializeEnemy(EnemySpawnPoint home) {
        this.home = home;
    }

    public int getHP() {
        return HP;
    }

    public void changeHP(int amount) {
        HP += amount;
        if (HP < 0) {
            HP = 0;
        }
    }

    public int getDefence() {
        return defence;
    }

    public float getEvasion() {
        return evasion;
    }

    public int getBasicAttackDamage() {
        return basicAttackDamage;
    }

    public int getFullHP() {
        return fullHP;
    }

    public int getxpReward()
    {
        return xpReward;
    }

    abstract public enemyType GetEnemyType();

}
