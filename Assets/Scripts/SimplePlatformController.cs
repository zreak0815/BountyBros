using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimplePlatformController : MonoBehaviour
{
    // Blickrichtung des Spielers
    [HideInInspector] public bool facingRight = true;

    // Kann der Spieler Doppelsprung oder Stampfangriff ausführen
    [HideInInspector] public bool canDoubleJump;
    [HideInInspector] public bool isStomping;

    // Hat der Spieler die Fähigkeit für den Doppelsprung bzw. den Stampfangriff
    public bool abilityDoubleJump = true;
    public bool abilityStomp = true;

    //Geschwindigkeit auf dem Boden
    public float groundSpeed = 6;
    public float groundAcceleration = 0.1f;
    public float groundDeceleration = 0.1f;

    //Geschwindigkeit in der Luft
    public float airSpeed = 3;
    public float airAcceleration = 0.1f;
    public float airDeceleration = 0.1f;

    //Fallgeschwindigkeit
    public float fallSpeed = 3;
    public float gravity = 0.05f;

    //Sprunghöhen
    public float jumpForce = 1000f;
    public float doubleJumpForce = 1000f;

    //Stampfgeschwindigkeit
    public float stompForce = 2000f;

    //Kollisionsbox
    public CollisionBox collisionBox;

    //Ob der Spieler sich auf dem Boden befindet
    public bool grounded = false;

    // True, wenn der Spieler im Kampf ist, sonst false
    public bool inCombat = false;

    //Animator
    public Animator anim;

    //Combatmanager
    public CombatManager combatManager;

    //Spielerhitbox
    public GameObject playerHitbox;

    //Spieler
    public PlayerValues player;

    //Text der HP-Anzeige
    public Text hpText;

    // True, wenn eine TextBox angezeigt wird, sonst false
    private bool activeTextBox = false;

    //Unverwundbarkeit nach dem Berühren von Spikes
    private int invincibility = 0;

    private int attackTime = 0;

    private int combatTimeout = 0;

    //initialization
    private void Start() {
        collisionBox.layerMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Objects");

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerValues>();

        hpText.text = "HP " + player.getHP().ToString() + "/" + player.getFullHP();
    }

    // Gibt den Skill mit der übergebenen ID zurück
    public void getSkill(int skillId) {
        switch(skillId) {
            case 0:
                abilityDoubleJump = true;
                print("Skill Obtained: Double Jump!");
                break;
            case 1:
                abilityStomp = true;
                print("Skill Obtained: Stomp!");
                break;
        }
    }

    /// <summary>
    /// Prüft of die Kollisionsbox einen Gegner berührt
    /// </summary>
    public void checkEnemies()
    {
        RaycastHit2D enemy = Physics2D.BoxCast(collisionBox.transform.position, collisionBox.transform.lossyScale, 0, Vector2.right, 0, 1 << LayerMask.NameToLayer("Enemies"));
        if (enemy.collider != null)
        {
            Debug.Log("enemy touched!");
            
            metEnemy(enemy.collider.GetComponent<EnemyOverworldHitbox>().owner, 0);
        }
    }

    // Spieler trifft auf den übergebenen Gegner
    public void metEnemy(EnemyBase enemy, int priority) {

        if (combatTimeout == 0 || priority > 0) {
            //TODO Erstschlag einbauen
            if (priority == 1) {
                print("Spieler hat Erstschlag!");
            }

            collisionBox.setHSpeed(0f);
            collisionBox.setVSpeed(0f);

            combatTimeout = 60;

            combatManager.startCombat(enemy, enemy.gameObject, priority);
        }
        
    }

    //Setzt, ob der Spieler im Kampf ist
    public void setInCombat()
    {
        inCombat = !inCombat;
    }

    public void textBoxTriggered()
    {
        setActiveTextBox();

        collisionBox.setHSpeed(0f);
    }

    public void setActiveTextBox()
    {
        activeTextBox = !activeTextBox;
    }

    // Update is called once per frame
    void Update()
    {

        if (player.getHP() <= 0 && !combatManager.inCombat) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        invincibility = Mathf.Max(0, invincibility - 1);
        if (!combatManager.inCombat) {
            combatTimeout = Mathf.Max(0, combatTimeout - 1);
        }

        attackTime = Mathf.Max(0, attackTime - 1);

        if (attackTime == 15) {
            Instantiate(playerHitbox, transform);
        }

        if (Input.GetButtonDown("Attack") && !activeTextBox) {
            anim.SetTrigger("Attack");
            attackTime = 20;
            if (!grounded) {
                checkFlip(Input.GetAxis("Horizontal"));
            }
        }

        if (!combatManager.getInCombat()) {
            //Spieler wird bewegt
            collisionBox.movement();

            //Prüft ob der Spieler auf dem Boden ist
            grounded = collisionBox.isGrounded();

            if (grounded)
            {
                //Stacheln
                if (invincibility == 0) {
                    Spikes floorDamage = collisionBox.getGround().GetComponent<Spikes>();
                    if (floorDamage != null) {
                        const int SPIKE_DAMAGE = 10;
                        player.changeHP(-SPIKE_DAMAGE);
                        hpText.text = "HP " + player.getHP().ToString() + "/" + player.getFullHP();
                        invincibility = 120;
                        combatManager.showText(transform.position, SPIKE_DAMAGE.ToString(), Color.red);
                    }
                }

                //Spieler ist auf dem boden
                if (isStomping)
                {
                    //Spieler landet von einer Stampfattacke
                    GameObject ground = collisionBox.getGround();
                    if (ground != null)
                    {
                        Destructable des = ground.GetComponent<Destructable>();
                        if (des != null && des.stomp)
                        {
                            des.dealDamage(1);
                            collisionBox.setVSpeed(0.4f);
                        }
                    }
                } else {
                    if (collisionBox.velocity.x != 0) {
                        GameObject wall = collisionBox.getWall(collisionBox.velocity.x > 0 ? Vector3.right : Vector3.left);
                        if (wall != null) {
                            Pushable push = wall.GetComponent<Pushable>();
                            if (push != null) {
                                wall.GetComponent<CollisionBox>().wallCollisionH(collisionBox.velocity.x > 0 ? push.pushSpeed : -push.pushSpeed);
                            }
                        }
                    }
                }
                canDoubleJump = true;
                isStomping = false;

                if (Input.GetAxis("Horizontal") != 0 && !activeTextBox)
                {
                    anim.SetTrigger("Walking");
                }
                else
                {
                    anim.SetTrigger("Standing");
                }

            }
            else
            {
                anim.SetTrigger("Jumping");
            }

            if (Input.GetButtonDown("Jump") && !activeTextBox)
            {
                if (grounded)
                {
                    //Sprung von dem Boden
                    anim.ResetTrigger("Standing");
                    anim.ResetTrigger("Walking");
                    anim.ResetTrigger("Stomp");
                    anim.SetTrigger("Jumping");
                    collisionBox.setVSpeed(jumpForce);
                    grounded = false;
                }
                else if (canDoubleJump && abilityDoubleJump)
                {
                    //Doppelsprung
                    canDoubleJump = false;
                    //Stampfattacke wird abgrbrochen
                    isStomping = false;
                    anim.SetTrigger("DoubleJump");
                    collisionBox.setVSpeed(doubleJumpForce);
                }
            }
            if (abilityStomp && Input.GetButtonDown("Stomp") && !grounded)
            {
                //Stampfattacke
                anim.ResetTrigger("Jumping");
                anim.SetTrigger("Stomp");
                isStomping = true;
            }

            //Prüft auf Gegnerberührung
            checkEnemies();
        }
        hpText.text = "HP " + player.getHP() + "/" + player.getFullHP();
    }

    // Prüft, ob die Animation gedreht werden muss
    private void checkFlip(float direction) {
        if (direction > 0 && !facingRight) {
            Flip();
        }
        else if (direction < 0 && facingRight) {
            Flip();
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        bool fixedGrounded = collisionBox.isGrounded();

        if (!inCombat) {
            if (fixedGrounded)
            {
                if (attackTime > 0 || activeTextBox) {
                    h = 0;
                }

                //Bewegung auf dem Boden
                collisionBox.setHSpeed(Mathf.Clamp(collisionBox.velocity.x + h * groundAcceleration, -groundSpeed, groundSpeed));

                if (h == 0)
                {
                    if (collisionBox.velocity.x > 0)
                    {
                        collisionBox.setHSpeed(Mathf.Max(collisionBox.velocity.x - groundDeceleration, 0));
                    }

                    if (collisionBox.velocity.x < 0)
                    {
                        collisionBox.setHSpeed(Mathf.Min(collisionBox.velocity.x + groundDeceleration, 0));
                    }
                }
                else
                {
                }

                collisionBox.setVSpeed(0);
            }
            else
            {
                //Bewegung in der Luft
                collisionBox.setHSpeed(Mathf.Clamp(collisionBox.velocity.x + h * airAcceleration, -airSpeed, airSpeed));

                if (h == 0)
                {
                    if (collisionBox.velocity.x > 0)
                    {
                        collisionBox.setHSpeed(Mathf.Max(collisionBox.velocity.x - airDeceleration, 0));
                    }

                    if (collisionBox.velocity.x < 0)
                    {
                        collisionBox.setHSpeed(Mathf.Min(collisionBox.velocity.x + airDeceleration, 0));
                    }
                }

                collisionBox.setVSpeed(Mathf.Max(collisionBox.velocity.y - gravity, -fallSpeed));
            }

            if (fixedGrounded)
            {
                checkFlip(h);
            }
            if (isStomping)
            {
                collisionBox.setVSpeed(-stompForce);
            }
        }
    }

    /// <summary>
    /// Spiegelt das Objekt
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
