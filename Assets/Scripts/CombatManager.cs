using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {

    //Kameras
    public GameObject playerCamera;
    public GameObject battleCamera;

    //Kampfpositionen
    public GameObject playerPodium;
    public GameObject monsterPodium;

    //Spieler
    public PlayerValues player;

    public EnemyBase enemyBase;

    public BattleMenu currentMenu;
    public int currentSelection;

    public bool isPlayersTurn;
    public bool inCombat = false;

    public Text playerHPText;
    public Text enemyHPText;

    public GameObject HPPanel;
    public Text jumpAndRunUIText;

    public GameObject XPPanel;
    public Text xpText;

    private GameObject touchedEnemy;

    public CombatBase playerCombat;
    public CombatBase enemyCombat;

    public GameObject[] enemyList;

    [Header("Selection")]
    public GameObject selectionMenu;
    public Text attack;
    public Text item;
    public Text escape;
    private const int SELECTION_AMOUNT = 3;
    private string copyAttack;
    private string copyItem;
    private string copyEscape;

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

    [Header("Items")]
    public GameObject itemsMenu;
    public Text hpPotion;
    private const int ITEM_AMOUNT = 1;
    private string copyHpPotion;

    private int savedDamage = 0;
    private bool savedEvade = false;
    private bool canAttack = true;

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

    public void completeAction() {

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
                            Debug.Log("player has " + player.getHPFlaskAmount() + " hp flasks");
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
                    {
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

    public void setInCombat(bool combat)
    {
        inCombat = combat;
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

    public bool getInCombat()
    {
        return inCombat;
    }

    private  void switchSelection()
    {
        switch (currentMenu) {
            case BattleMenu.Selection:
                attack.text = copyAttack;
                item.text = copyItem;
                escape.text = copyEscape;
                switch(currentSelection) {
                    case 1:
                        attack.text = "> " + copyAttack;
                        break;
                    case 2:
                        item.text = "> " + copyItem;
                        break;
                    case 3:
                        escape.text = "> " + copyEscape;
                        break;
                }
                break;
            case BattleMenu.Attacks:
                updateAttackNames();
                break;
            case BattleMenu.Items:
                //hpPotion.text = copyHpPotion;
                break;
        }

    }

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

    private void performEnemyAttack()
    {
        float evaded = Random.Range(0.0f, 1.0f);
        savedEvade = enemyBase.getEvasion() > evaded;

        savedDamage = Mathf.RoundToInt((enemyBase.getBasicAttackDamage() - player.getDefense()) * Random.Range(0.8f, 1.2f));

        enemyCombat.performAttackAnimation(1);

    }

    private void performPlayerAttack (int attackSkillIndex)
    {
        if (canAttack && player.getCooldownFromAttack(attackSkillIndex) == 0) {
            float evaded = Random.Range(0.0f, 1.0f);
            savedEvade = enemyBase.getEvasion() > evaded;

            savedDamage = Mathf.RoundToInt((player.getDamageFromAttack(attackSkillIndex) - enemyBase.getDefence()) * Random.Range(0.8f, 1.2f));

            //isPlayersTurn = !isPlayersTurn;
            playerCombat.performAttackAnimation(attackSkillIndex);
            canAttack = false;

            player.setCooldownFromAttack(attackSkillIndex);
            updateAttackNames();
        }
        
    }

    private void setTextState(Text text, string name, int timeout, bool selected) {
        if (timeout == 0) {
            text.text = name;
            text.color = Color.black;
        } else {
            text.text = name + " - " + timeout.ToString();
            text.color = Color.gray;
        }

        if (selected) {
            text.text = "> " + text.text;
        }
    }

    private void reduceCooldowns() {
        player.reduceCooldowns();
        updateAttackNames();
    }

    private void updateAttackNames() {
        setTextState(lightHit, copyLightHit, 0, currentSelection == 1);
        setTextState(heavyHit, copyHeavyHit, player.getCooldownFromAttack(2), currentSelection == 2);
        setTextState(preciseShot, copyPreciseShot, player.getCooldownFromAttack(3), currentSelection == 3);
        setTextState(stompAttack, copyStompAttack, player.getCooldownFromAttack(4), currentSelection == 4);
    }

    public void endTurn() {
        isPlayersTurn = !isPlayersTurn;

        if (player.getHP() <= 0) {
            playerLost();
            return;
        }

        if (enemyBase.getHP() <= 0) {
            endCombat();
            return;
        }

        if (isPlayersTurn) {
            canAttack = true;
            reduceCooldowns();
        } else {
            performEnemyAttack();
        }
    }

    public void dealDamage() {
        if (savedEvade) {
            Debug.Log((isPlayersTurn ? "Enemy" : "Player") + " evaded!");
        }
        else {
            if (isPlayersTurn) {
                enemyBase.changeHP(-savedDamage);
                enemyHPText.text = "HP " + enemyBase.getHP() + "/" + enemyBase.getFullHP();
                enemyCombat.playHurtAnimation();

                if (enemyBase.getHP() <= 0) {
                    enemyCombat.playDefeatAnimation();
                }

            }
            else {
                player.changeHP(-savedDamage);
                playerHPText.text = "HP " + player.getHP() + "/" + player.getFullHP();
                playerCombat.playHurtAnimation();

                if (player.getHP() <= 0) {
                    playerCombat.playDefeatAnimation();
                }
            }
            Debug.Log((isPlayersTurn ? "Enemy" : "Player") + " hit with " + savedDamage + " damage!");
        }
    }

    public void playerLost()
    {
        setInCombat(false);
    }

    private void endCombat()
    {
        jumpAndRunUIText.text = "HP " + player.getHP() + "/" + player.getFullHP();
        player.changeXP(enemyBase.getxpReward());
        xpText.text = "XP " + player.getXP() + "/" + player.getXPForLevel(player.getLevel());
        Destroy(touchedEnemy);
        playerCamera.SetActive(true);
        battleCamera.SetActive(false);
        setInCombat(false);
        Debug.Log("Player won fight!");
        Destroy(enemyCombat.gameObject);
    }

    // Versucht aus dem Kampf zu fliehen
    private void tryEscape()
    {
        if (isPlayersTurn) {
            float escape = Random.Range(0.0f, 1.0f);
            Debug.Log(escape);
            if (player.getEscapeChance() < escape) {
                Debug.Log("Failed to escape!");
                endTurn();
            }
            else {
                HPPanel.SetActive(true);
                Debug.Log("Escape successful!");
                playerCamera.SetActive(true);
                battleCamera.SetActive(false);
                setInCombat(false);
            }
        }
    }

    public void startCombat(EnemyBase enemyType, GameObject enemyObject, int priority) {
        if (!inCombat) {
            HPPanel.SetActive(false);
            playerHPText.text = "HP " + player.getHP() + "/" + player.getFullHP();
            hpPotion.text = "> " + player.getHPFlaskAmount() + "x Heiltrank";
            Debug.Log(hpPotion.text);

            touchedEnemy = enemyObject;
            playerCamera.SetActive(false);
            battleCamera.SetActive(true);
            changeMenu(BattleMenu.Selection);
            switchSelection();

            GameObject enemy = Instantiate(enemyList[(int)enemyType.GetEnemyType()], monsterPodium.transform);
            enemyCombat = enemy.GetComponent<CombatBase>();

            playerCombat.setTarget(true);
            enemyCombat.setTarget(false);

            enemyBase = enemyType;
            canAttack = true;

            if (priority >= 0) {
                isPlayersTurn = true;
            }
            else {
                isPlayersTurn = false;
                performEnemyAttack();
            }

            setInCombat(true);
            enemyHPText.text = "HP " + enemyBase.getHP() + "/" + enemyBase.getFullHP();

            reduceCooldowns();
        }
    }
}
