using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// Setzt die Referenz für den Spawnpunkt
    /// </summary>
    /// <param name="home">Der Spawnpunkt</param>
    public void initializeEnemy(EnemySpawnPoint home) {
        this.home = home;
    }

    abstract public int getHP();

    abstract public void changeHP(int amount);

    abstract public int getDefence();

    abstract public float getEvasion();

    abstract public int getBasicAttackDamage();

    abstract public int getFullHP();

}
