using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 Klasse für das Beenden eines Levels
     */
public class LevelExit : MonoBehaviour {

    public string nextLevelName;
    public float exitRange = 3;
    private SimplePlatformController hero;

    private void Start() {
        hero = FindObjectOfType<SimplePlatformController>();
    }

    // Update is called once per frame
    void Update () {
        Debug.DrawLine(transform.position + Vector3.right * exitRange + Vector3.up * exitRange, transform.position + Vector3.right * exitRange + Vector3.down * exitRange);
        Debug.DrawLine(transform.position + Vector3.left * exitRange + Vector3.up * exitRange, transform.position + Vector3.left * exitRange + Vector3.down * exitRange);

        if (Mathf.Abs(hero.transform.position.x - transform.position.x) < exitRange &&
            Mathf.Abs(hero.transform.position.y - transform.position.y) < exitRange) {
            hero.GetComponent<PlayerValues>().saveStats();
            SceneManager.LoadScene(nextLevelName);
        }
	}
}
