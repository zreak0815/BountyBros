using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Klasse, die den rundenbasierten Kampf grundlegend steuert und 
 Befehle an die folgenden Klassen/Funktionen weitergibt.
     */

public class CombatManager : MonoBehaviour {

    //Kameras
    public GameObject playerCamera;
    public GameObject battleCamera;

    //Kampfpositionen
    public GameObject playerPodium;
    public GameObject monsterPodium;

    //Spieler-/Gegnerwerte
    public PlayerValues player;
    public EnemyBase enemyBase;

    //Aktuelles Menü und aktuelle Auswahl
    public BattleMenu currentMenu;
    public int currentSelection;

    //Ist der Spieler am Zug
    public bool isPlayersTurn;

    //Ist der Spieler im Kampf
    public bool inCombat = false;

    //Lebensanzeigen
    public Text playerHPText;
    public Text enemyHPText;

    //Panel mit Texten für HP und EP für das Jump & Run UI
    public GameObject HPPanel;
    public Text jumpAndRunUIText;
    public GameObject XPPanel;
    public Text xpText;

    //Der berührte Gegner
    private GameObject touchedEnemy;

    //Spieler und Gegner
    public CombatBase playerCombat;
    public CombatBase enemyCombat;

    //Menü um eine Aktion auszuwählen
    [Header("Selection")]
    public GameObject selectionMenu;
    public Text attack;
    public Text item;
    public Text escape;
    private const int SELECTION_AMOUNT = 3;
    private string copyAttack;
    private string copyItem;
    private string copyEscape;

    //Menü um einen Angriff auszuwählen
    [Header("Attacks")]
    public GameObject attackMenu;
    public Text lightHit;
    public Text heavyHit;
    public Text preciseShot;
    public Text stompAttack;
    private const int ATTACK_AMOUNT = 4;
    private string copyLightHit;
    private string copyHeavyHit;
    private string copyPreciseShot;
    private string copyStompAttack;

    //Menü um ein Item auszuwählen
    [Header("Items")]
    public GameObject itemsMenu;
    public Text hpPotion;
    private const int ITEM_AMOUNT = 1;
    private string copyHpPotion;

    private int savedDamage = 0;
    private bool savedEvade = false;
    private bool canAttack = true;

    //Enum für mögliche Aktionen
    public enum BattleMenu
    {
        Selection,
        Items,
        Attacks
    };

	// Use this for initialization
	void Start () {
        playerCamera.SetActive(true);
        battleCamera.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerValues>();

        currentSelection = 1;

        copyAttack = attack.text;
        copyItem = item.text;
        copyEscape = escape.text;

        copyLightHit = lightHit.text;
        copyHeavyHit = heavyHit.text;
        copyPreciseShot = preciseShot.text;
        copyStompAttack = stompAttack.text;

        copyHpPotion = hpPotion.text;
    }

	
	// Update is called once per frame
	void Update () {

        // Wechselt die Auswahl im aktuellen Menü
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            // Hier ist switch notwendig, weil BattleMenu.Selection nur 3 Auswahlmöglichkeiten hat       
            switch (currentMenu)
            {
                case BattleMenu.Selection:
                    if (currentSelection < SELECTION_AMOUNT)
                    {
                        currentSelection++;
                        switchSelection();
                    }
                    break;
                case BattleMenu.Attacks:
                    if (currentSelection < ATTACK_AMOUNT)
                    {
                        currentSelection++;
                        switchSelection();
                    }
                    break;
                case BattleMenu.Items:
                    if (currentSelection < ITEM_AMOUNT)
                    {
                        currentSelection++;
                        switchSelection();
                    }
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            if (currentSelection > 1)
            {
                currentSelection--;
                switchSelection();
            }
        }

        // reagiert auf Enter Eingabe
        if (isPlayersTurn && Input.GetKeyDown(KeyCode.Return))
        {
            //prüft das aktuelle Menü
            switch(currentMenu)
            {
                case BattleMenu.Selection:
                    switch(currentSelection)
                    {
                        case 1:
                            changeMenu(BattleMenu.Attacks);
                            switchSelection();
                            break;
                        case 2:
                            changeMenu(BattleMenu.Items);
                            switchSelection();
                            break;
                        case 3:
                            tryEscape();
                            break;
                    }
                    break;
                case BattleMenu.Attacks:
                    switch(currentSelection)
                    { // perform attack
                        case 1:
                            performPlayerAttack(1);
                            break;
                        case 2:
                            performPlayerAttack(2);
                            break;
                        case 3:
                            performPlayerAttack(3);
                            break;
                        case 4:
                            performPlayerAttack(4);
                            break;
                    } 
                    break;
                case BattleMenu.Items:
                    switch(currentSelection)
                    { // use item
                        case 1:
                            if (player.getHPFlaskAmount() > 0) {
                                player.useHPPotion();
                                hpPotion.text = "> " + player.getHPFlaskAmount() + "x Heiltrank";
                                playerHPText.text = "HP " + player.getHP().ToString() + "/" + player.getFullHP();
                            }
                            break;
                    }
                    break;
            }
        }

        // geht zum vorherigen Menü zurück
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch(currentMenu)
            {
                case BattleMenu.Attacks:
                case BattleMenu.Items:
                    changeMenu(BattleMenu.Selection);
                    switchSelection();
                    break;
            }
        }


    }

    // Wechselt, ob der Spieler im Kampf ist oder nicht und
    // aktiviert die entsprechenden UI Elemente
    public void setInCombat()
    {
        inCombat = !inCombat;
        if (inCombat)
        {
            HPPanel.SetActive(false);
            XPPanel.SetActive(false);
        } else
        {
            HPPanel.SetActive(true);
            XPPanel.SetActive(true);
        }
    }

    // Setzt den Spieler in den Kampf
    public bool getInCombat()
    {
        return inCombat;
    }

    // Wechselt die Auswahl des aktullen Menüs
    private  void switchSelection()
    {
        switch (currentSelection)
        {
            case 1:
                switch (currentMenu)
                {
                    case BattleMenu.Selection:
                        attack.text = "> " + copyAttack;
                        item.text = copyItem;
                        escape.text = copyEscape;
                        break;
                    case BattleMenu.Attacks:
                        lightHit.text = "> " + copyLightHit;
                        heavyHit.text = copyHeavyHit;
                        preciseShot.text = copyPreciseShot;
                        stompAttack.text = copyStompAttack;
                        break;
                    case BattleMenu.Items:
                        // Ist bei nur einem Item irrelevant
                        break;
                }
                break;
            case 2:
                switch (currentMenu)
                {
                    case BattleMenu.Selection:
                        item.text = "> " + copyItem;
                        attack.text = copyAttack;
                        escape.text = copyEscape;
                        break;
                    case BattleMenu.Attacks:
                        lightHit.text = copyLightHit;
                        heavyHit.text = "> " + copyHeavyHit;
                        preciseShot.text = copyPreciseShot;
                        stompAttack.text = copyStompAttack;
                        break;
                    case BattleMenu.Items:
                        // Ist bei nur einem Item irrelevant
                        break;
                }
                break;
            case 3:
                switch (currentMenu)
                {
                    case BattleMenu.Selection:
                        escape.text = "> " + copyEscape;
                        attack.text = copyAttack;
                        item.text = copyItem;
                        break;
                    case BattleMenu.Attacks:
                        lightHit.text = copyLightHit;
                        heavyHit.text = copyHeavyHit;
                        preciseShot.text = "> " + copyPreciseShot;
                        stompAttack.text = copyStompAttack;
                        break;
                    case BattleMenu.Items:
                        // Ist bei nur einem Item irrelevant
                        break;
                }
                break;
            case 4:
                switch (currentMenu)
                {
                    case BattleMenu.Attacks:
                        lightHit.text = copyLightHit;
                        heavyHit.text = copyHeavyHit;
                        preciseShot.text = copyPreciseShot; ;
                        stompAttack.text = "> " + copyStompAttack;
                        break;
                    case BattleMenu.Items:
                        // Ist bei nur einem Item irrelevant
                        break;
                }
                break;
        }
    }

    // Wechselt das aktuelle Menü zum übergebenen Menü
    public void changeMenu(BattleMenu battleMenu)
    {
        currentMenu = battleMenu;
        currentSelection = 1;
        switch(battleMenu)
        {
            case BattleMenu.Selection:
                selectionMenu.gameObject.SetActive(true);
                attackMenu.gameObject.SetActive(false);
                itemsMenu.gameObject.SetActive(false);
                break;
            case BattleMenu.Items:
                itemsMenu.gameObject.SetActive(true);
                selectionMenu.gameObject.SetActive(false);
                attackMenu.gameObject.SetActive(false);
                break;
            case BattleMenu.Attacks:
                attackMenu.gameObject.SetActive(true);
                selectionMenu.SetActive(false);
                itemsMenu.SetActive(false);
                break;
        }
    }

    // Führt einen Angriff des Gegners aus
    private void performEnemyAttack()
    {
        int attackDamage = 0;
        float evaded = Random.Range(0.0f, 1.0f);
        if (player.getEvasion() > evaded)
        {
            Debug.Log("Player evaded!");
        } else
        {
            attackDamage = enemyBase.getBasicAttackDamage() - player.getDefense();
            player.changeHP(-attackDamage);
            if (player.getHP() <= 0)
            {
                playerLost();
            } else
            {
                playerHPText.text = "HP " + player.getHP() + "/" + player.getFullHP();
            }
            Debug.Log("Player hit with " + attackDamage + " damage!");
        }

        endTurn();
    }

    // Führt einen Angriff des Spielers aus
    private void performPlayerAttack (int attackSkillIndex)
    {
        if (canAttack) {
            float evaded = Random.Range(0.0f, 1.0f);
            savedEvade = enemyBase.getEvasion() > evaded;

            savedDamage = player.getDamageFromAttack(attackSkillIndex) - enemyBase.getDefence();
            
            playerCombat.performAttackAnimation(attackSkillIndex);
            canAttack = false;
        }
        
    }

    // Beendet einen Spielzug
    public void endTurn() {
        isPlayersTurn = !isPlayersTurn;
        if (isPlayersTurn) {
            canAttack = true;
        } else {
            performEnemyAttack();
        }
    }

    // Fügt entsprechenden Schaden zu
    public void dealDamage() {
        if (savedEvade) {
            Debug.Log((isPlayersTurn ? "Enemy" : "Player") + " evaded!");
        }
        else {
            if (isPlayersTurn) {
                enemyBase.changeHP(-savedDamage);
                enemyHPText.text = "HP " + enemyBase.getHP() + "/" + enemyBase.getFullHP();

                if (enemyBase.getHP() <= 0) {
                    endCombat();
                }
            }
            else {
                player.changeHP(-savedDamage);
            }
            Debug.Log((isPlayersTurn ? "Enemy" : "Player") + " hit with " + savedDamage + " damage!");
        }
    }

    // Lässt den Spieler verlieren
    public void playerLost()
    {
        playerCamera.SetActive(true);
        battleCamera.SetActive(false);
        setInCombat();
    }

    // Beendet den Kampf. Der Spieler hat in diesem Fall gewonnen.
    private void endCombat()
    {
        jumpAndRunUIText.text = "HP " + player.getHP() + "/" + player.getFullHP();
        player.changeXP(enemyBase.getxpReward());
        xpText.text = "XP " + player.getXP() + "/" + player.getXPForLevel(player.getLevel());
        Destroy(touchedEnemy);
        playerCamera.SetActive(true);
        battleCamera.SetActive(false);
        setInCombat();
    }

    // Versucht aus dem Kampf zu fliehen
    private void tryEscape()
    {
        float escape = Random.Range(0.0f, 1.0f);
        Debug.Log(escape);
        if (player.getEscapeChance() < escape)
        {
            Debug.Log("Failed to escape!");
        } else
        {
            Debug.Log("Escape successful!");
            playerCamera.SetActive(true);
            battleCamera.SetActive(false);
            setInCombat();
        }
    }

    // Startet den Kampf
    public void startCombat(EnemyBase enemyType, GameObject enemyObject, int priority)
    {
        HPPanel.SetActive(false);
        playerHPText.text = "HP " + player.getHP() + "/" + player.getFullHP();
        enemyHPText.text = "HP " + enemyType.getHP() + "/" + enemyType.getFullHP();
        hpPotion.text = "> " + player.getHPFlaskAmount() + "x Heiltrank";
        Debug.Log(hpPotion.text);

        touchedEnemy = enemyObject;
        playerCamera.SetActive(false);
        battleCamera.SetActive(true);
        changeMenu(BattleMenu.Selection);
        switchSelection();

        playerCombat.setTarget(true);
        enemyCombat.setTarget(false);

        enemyBase = enemyType;
        canAttack = true;
    }
}
