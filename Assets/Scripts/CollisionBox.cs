using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Klasse für eine Kollisionsbox
 * Bewegt das Objekt und berechnet Kollisionen mit Wänden
 */
public class CollisionBox : MonoBehaviour {

    //Geschwindigkeit
    public Vector2 velocity;
    //Das Objekt zu dem die Kollisionsbox gehört
    public Transform mainTransform;
    //Ob die Kollisionsbox von sich aus movement aufruft
    public bool auto;
    //Beschleunigung nach unten
    public float gravity;
    //Layer Mask für Kollision
    public int layerMask = 4096;

    private void Start() {
        if (auto) {
            layerMask = 1 << LayerMask.NameToLayer("Ground");
        }
    }

    private void Update() {
        if (auto) {
            movement();
        }
    }

    /// <summary>
    /// Setzt die horizontale Geschwindigkeit
    /// </summary>
    /// <param name="hSpeed">die neue Geschwindigkeit</param>
    public void setHSpeed(float hSpeed) {
        velocity = new Vector2(hSpeed, velocity.y);
    }

    /// <summary>
    /// Setzt die vertikale Geschwindigkeit
    /// </summary>
    /// <param name="vSpeed">die neue Geschwindigkeit</param>
    public void setVSpeed(float vSpeed) {
        velocity = new Vector2(velocity.x, vSpeed);
    }

    public void setSpeed(Vector2 speed) {
        velocity = speed;
    }

    /// <summary>
    /// Prüft auf Kollision unter der Kollisionsbox
    /// </summary>
    /// <returns>Ob die Kollisionsbox einen Boden hat</returns>
    public bool isGrounded() {
        return velocity.y <= 0 && Physics2D.Raycast(transform.position + Vector3.down * transform.lossyScale.y * 0.5f, Vector3.down, 0.01f, layerMask).collider != null;
    }

    /// <summary>
    /// Liefert das Objekt unter der Kollisionsbox
    /// </summary>
    /// <returns>das Objekt unter der Kollisionsbox</returns>
    public GameObject getGround() {
        Collider2D coll = Physics2D.Raycast(transform.position + Vector3.down * transform.lossyScale.y * 0.5f, Vector3.down, 0.01f, layerMask).collider;
        if (coll != null) {
            return coll.gameObject;
        }
        return null;
    }

    public GameObject getWall(Vector3 direction) {
        Collider2D coll = Physics2D.BoxCast(new Vector2(transform.position.x + Mathf.Abs(transform.lossyScale.x) * 0.25f * direction.x, transform.position.y), new Vector2(Mathf.Abs(transform.lossyScale.x) * 0.5f - 0.1f, transform.lossyScale.y - 0.1f), 0, direction, 0.01f, layerMask).collider;
        if (coll != null) {
            return coll.gameObject;
        }
        return null;
    }

    /// <summary>
    /// Bewegt das Objekt horizontal mit Wandkollision
    /// </summary>
    /// <param name="hdistance">horizontale Geschwindigkeit</param>
    public void wallCollisionH(float hdistance) {
        RaycastHit2D ray;
        Vector3 direction = Vector3.right;
        if (hdistance < 0) {
            direction = Vector3.left;
        }
        ray = Physics2D.BoxCast(new Vector2(transform.position.x + Mathf.Abs(transform.lossyScale.x) * 0.25f * direction.x, transform.position.y), new Vector2(Mathf.Abs(transform.lossyScale.x) * 0.5f - 0.1f, transform.lossyScale.y - 0.1f), 0, direction, Mathf.Abs(hdistance), layerMask);
        if (ray.collider != null) {
            mainTransform.Translate(direction * ray.distance);
        } else {
            mainTransform.Translate(Vector3.right * hdistance);
        }
    }

    /// <summary>
    /// Bewegt das Objekt vertikal mit Wandkollision
    /// </summary>
    /// <param name="vdistance">vertikale Geschwindigkeit</param>
    public void wallCollisionV(float vdistance) {

        RaycastHit2D ray;
        Vector3 direction = Vector3.up;
        if (vdistance < 0) {
            direction = Vector3.down;
        }
        ray = Physics2D.Raycast(transform.position + direction * transform.lossyScale.y * 0.5f, direction, Mathf.Abs(vdistance), layerMask);

        if (ray.collider != null) {
            mainTransform.Translate(direction * ray.distance);
        } else {
            mainTransform.Translate(Vector3.up * vdistance);
        }

    }

    /// <summary>
    /// Bewegt das Objekt Horizontal und Vertikal
    /// </summary>
    public void movement() {
        if (auto) {
            if (isGrounded()) {
                setVSpeed(0);
            }
            else {
                setVSpeed(velocity.y - gravity);
            }
        }
        wallCollisionH(velocity.x);
        wallCollisionV(velocity.y);
    }
	
}
