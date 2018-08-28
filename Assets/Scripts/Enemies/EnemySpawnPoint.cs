using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Eine Klasse für einen Punkt and dem Gegner platziert werden sollen
 */
public class EnemySpawnPoint : MonoBehaviour {

    //List der Gegner prefabs die platziert werden sollen
    public GameObject[] enemiesToSpawn;
    //Liste der Gegnerobjekte, die platziert wurden
    private GameObject[] spawnedEnemies;
    //Ob die Gegner platziert wurden
    private bool enemiesSpawned = false;
    //Die Hauptfigur
    private GameObject hero;

    //Reichweite des Gebietes, in dem sich die Gegner bewegen können
    public Vector2 pointRange;
    //Reichweite wann die Gegner platziert werden sollen
    public float detectionRange;

    private void Start() {
        //Findet die Hauptfigur
        hero = FindObjectOfType<SimplePlatformController>().gameObject;
    }

    private void Update() {
        //Wenn die Hauptfigur näher als detectionRange ist, werden Gegner platziert
        if (!enemiesSpawned && Vector3.Distance(transform.position, hero.transform.position) < detectionRange) {
            spawnEnemies();
        }

        //Debug zeichnet die Reichweiten
        Debug.DrawLine(transform.position + Vector3.right * pointRange.x + Vector3.up * pointRange.y,
            transform.position + Vector3.right * pointRange.x - Vector3.up * pointRange.y);
        Debug.DrawLine(transform.position - Vector3.right * pointRange.x + Vector3.up * pointRange.y,
            transform.position - Vector3.right * pointRange.x - Vector3.up * pointRange.y);

        for (int i = 0; i < 16; i++) {
            Debug.DrawLine(transform.position + new Vector3(Mathf.Cos(i * Mathf.PI / 8), Mathf.Sin(i * Mathf.PI / 8), 0) * detectionRange,
                transform.position + new Vector3(Mathf.Cos((i + 1) * Mathf.PI / 8), Mathf.Sin((i + 1) * Mathf.PI / 8), 0) * detectionRange, Color.white);
        }
    }

    /// <summary>
    /// Platziert die Gegner
    /// </summary>
    private void spawnEnemies() {
        enemiesSpawned = true;
        spawnedEnemies = new GameObject[enemiesToSpawn.Length];

        for (int i = 0; i < enemiesToSpawn.Length; i++) {
            GameObject newEnemy = null;
            if (enemiesToSpawn[i] != null) {
                newEnemy = Instantiate(enemiesToSpawn[i], transform.position, new Quaternion());
                newEnemy.GetComponent<EnemyBase>().initializeEnemy(this);
            }

            spawnedEnemies[i] = newEnemy;
        }
    }

}
