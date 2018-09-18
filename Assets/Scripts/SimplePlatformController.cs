using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimplePlatformController : MonoBehaviour
{

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool canDoubleJump;
    [HideInInspector] public bool isStomping;

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

    public GameObject playerHitbox;

    //Spieler
    public PlayerValues player;

    public Text hpText;

    private int invincibility = 0;

    private void Start() {
        collisionBox.layerMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Objects");

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerValues>();

        hpText.text = "HP " + player.getHP().ToString() + "/" + player.getFullHP();
    }

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

    public void metEnemy(EnemyBase enemy, int priority) {

        //TODO Erstschlag einbauen
        if (priority == 1) {
            print("Spieler hat Erstschlag!");
        }

        //Gegner wurde berührt
        combatManager.setInCombat();

        collisionBox.setHSpeed(0f);
        collisionBox.setVSpeed(0f);


        combatManager.startCombat(enemy, enemy.gameObject, priority);
    }

    public void setInCombat()
    {
        inCombat = !inCombat;
    }


    // Update is called once per frame
    void Update()
    {
       // Debug.Log(player.getHP());
        if (player.getHP() <= 0)
        {
            Vector3 playerSpawnPosition = GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform.position;
            GameObject.FindGameObjectWithTag("Player").transform.position = playerSpawnPosition;
            player.changeHP(player.getFullHP());
            //TODO play death animation & after respawn: platforms etc are not shown, but player is in correct position
            if (inCombat)
            {
                inCombat = !inCombat;
                combatManager.playerLost();
            }
        }

        invincibility = Mathf.Max(0, invincibility - 1);

        if (Input.GetButtonDown("Attack")) {
            Instantiate(playerHitbox, transform);
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
                        //TODO damage
                        const int SPIKE_DAMAGE = 10;
                        player.changeHP(-SPIKE_DAMAGE);
                        hpText.text = "HP " + player.getHP().ToString() + "/" + player.getFullHP();
                        //Debug.Log(player.getHP());
                        print("Spike Damage");
                        invincibility = 120;
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

                if (Input.GetAxis("Horizontal") != 0)
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

            if (Input.GetButtonDown("Jump"))
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
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        bool fixedGrounded = collisionBox.isGrounded();

        if (!inCombat) {
            if (fixedGrounded)
            {
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
                if (h > 0 && !facingRight)
                {
                    Flip();
                }
                else if (h < 0 && facingRight)
                {
                    Flip();
                }
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
