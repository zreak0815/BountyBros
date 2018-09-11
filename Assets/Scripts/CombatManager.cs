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

    //TODO add real items
    [Header("Items")]
    public GameObject itemsMenu;
    public Text itemOne;
    public Text itemTwo;
    public Text itemThree;
    public Text itemFour;
    private const int ITEM_AMOUNT = 4;
    private string copyItemOne;
    private string copyItemTwo;
    private string copyItemThree;
    private string copyItemFour;




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

        copyItemOne = itemOne.text;
        copyItemTwo = itemTwo.text;
        copyItemThree = itemThree.text;
        copyItemFour = itemFour.text;
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
        if (Input.GetKeyDown(KeyCode.Return))
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
                    { //TODO use items
                        case 1:
                            
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

    public void setInCombat()
    {
        inCombat = !inCombat;
    }

    public bool getInCombat()
    {
        return inCombat;
    }

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
                        itemOne.text = "> " + copyItemOne;
                        itemTwo.text = copyItemTwo;
                        itemThree.text = copyItemThree;
                        itemFour.text = copyItemFour;
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
                        itemOne.text = copyItemOne;
                        itemTwo.text = "> " + copyItemTwo;
                        itemThree.text = copyItemThree;
                        itemFour.text = copyItemFour;
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
                        itemOne.text = copyItemOne;
                        itemTwo.text = copyItemTwo;
                        itemThree.text = "> " + copyItemThree;
                        itemFour.text = copyItemFour;
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
                        itemOne.text = copyItemOne;
                        itemTwo.text = copyItemTwo;
                        itemThree.text = copyItemThree;
                        itemFour.text = "> " + copyItemFour;
                        break;
                }
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
    }

    private void performPlayerAttack (int attackSkillIndex)
    {
        int attackDamage = 0;
        float evaded = Random.Range(0.0f, 1.0f);
        if (enemyBase.getEvasion() > evaded)
        {
            Debug.Log("Enemy evaded!");
            performEnemyAttack();
        } else
        {
            attackDamage = player.getDamageFromAttack(attackSkillIndex) - enemyBase.getDefence();
            enemyBase.changeHP(-attackDamage);
            if (enemyBase.getHP() <= 0)
            {
                endCombat();
            }
            else
            {
                enemyHPText.text = "HP " + enemyBase.getHP() + "/" + enemyBase.getFullHP();
                performEnemyAttack();
            }
            Debug.Log("Enemy hit with " + attackDamage + " damage!");
        }
        isPlayersTurn = !isPlayersTurn;
        
    }

    public void playerLost()
    {
        playerCamera.SetActive(true);
        battleCamera.SetActive(false);
        setInCombat();
    }

    private void endCombat()
    {
        Destroy(GameObject.FindGameObjectWithTag("Slime"));
        playerCamera.SetActive(true);
        battleCamera.SetActive(false);
        setInCombat();
        Debug.Log("Player won fight!");
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

    public void startCombat(int enemyIndex)
    {
        playerCamera.SetActive(false);
        battleCamera.SetActive(true);
        changeMenu(BattleMenu.Selection);
        switchSelection();
        

        //TODO only finds the first enemy called Slime & place correct enemy image at monsterPodium
        EnemySlime enemy = GameObject.FindGameObjectWithTag("Slime").GetComponent<EnemySlime>();
        enemyBase = GameObject.FindGameObjectWithTag("Slime").GetComponent<EnemySlime>();


    }
}
