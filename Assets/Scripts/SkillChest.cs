using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChest : MonoBehaviour {

    public int skillId = 0;
    public float activationRange = 3;
    private bool opened = false;
    private SimplePlatformController hero;

    private void Start() {
        hero = FindObjectOfType<SimplePlatformController>();
    }

    // Update is called once per frame
    void Update() {
        if (!opened) {
            Debug.DrawLine(transform.position + Vector3.right * activationRange + Vector3.up * activationRange, transform.position + Vector3.right * activationRange + Vector3.down * activationRange);
            Debug.DrawLine(transform.position + Vector3.left * activationRange + Vector3.up * activationRange, transform.position + Vector3.left * activationRange + Vector3.down * activationRange);

            if (Mathf.Abs(hero.transform.position.x - transform.position.x) < activationRange &&
                Mathf.Abs(hero.transform.position.y - transform.position.y) < activationRange) {
                hero.getSkill(skillId);
                opened = true;
            }
        }
    }
}
