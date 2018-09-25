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

   public GameObject gameText;

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

   public GameObject[] enemyList;

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

   public void showText(Vector3 position, string text, Color color) {
      GameObject newText = Instantiate(gameText, position, new Quaternion());
      newText.GetComponent<TextMesh>().text = text;
      newText.GetComponent<TextMesh>().color = color;
   }

   //Enum für mögliche Aktionen
   public enum BattleMenu {
      Selection,
      Items,
      Attacks
   };

   // Use this for initialization
   void Start() {
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
      xpText = GameObject.Find("TextXP").GetComponent<Text>();
   }


   // Update is called once per frame
   void Update() {

      if (!inCombat) {
            return;
        }
        //Debug.Log(player.getFullHP());
      // Wechselt die Auswahl im aktuellen Menü
      if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
         // Hier ist switch notwendig, weil BattleMenu.Selection nur 3 Auswahlmöglichkeiten hat       
         switch (currentMenu) {
            case BattleMenu.Selection:
               if (currentSelection < SELECTION_AMOUNT) {
                  currentSelection++;
                  switchSelection();
               }
               break;
            case BattleMenu.Attacks:
               if (currentSelection < ATTACK_AMOUNT) {
                  currentSelection++;
                  switchSelection();
               }
               break;
            case BattleMenu.Items:
               if (currentSelection < ITEM_AMOUNT) {
                  currentSelection++;
                  switchSelection();
               }
               break;
         }
      }
      if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
         if (currentSelection > 1) {
            currentSelection--;
            switchSelection();
         }
      }

      // reagiert auf Enter Eingabe
      if (isPlayersTurn && (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.E))) {
         //prüft das aktuelle Menü
         switch (currentMenu) {
            case BattleMenu.Selection:
               switch (currentSelection) {
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
               switch (currentSelection) { // perform attack
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
               switch (currentSelection) { // use item
                  case 1:
                     if (canAttack && player.getHPFlaskAmount() > 0) {
                        player.useHPPotion();
                        showText(playerCombat.transform.position, player.getHPPotionRegen().ToString(), Color.green);
                        hpPotion.text = "> " + player.getHPFlaskAmount() + "x Heiltrank";
                        playerHPText.text = "HP " + player.getHP().ToString() + "/" + player.getFullHP();
                        endTurn();
                     }
                     break;
               }
               break;
         }
      }

      // geht zum vorherigen Menü zurück
      if (Input.GetKeyDown(KeyCode.Escape)) {
         switch (currentMenu) {
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
   public void setInCombat(bool combat) {
      inCombat = combat;
      if (inCombat) {
         HPPanel.SetActive(false);
         XPPanel.SetActive(false);
      } else {
         HPPanel.SetActive(true);
         XPPanel.SetActive(true);
      }
   }

   // Setzt den Spieler in den Kampf
   public bool getInCombat() {
      return inCombat;
   }

   // Wechselt die Auswahl des aktullen Menüs
   private void switchSelection() {
      switch (currentMenu) {
         case BattleMenu.Selection:
            attack.text = copyAttack;
            item.text = copyItem;
            escape.text = copyEscape;
            switch (currentSelection) {
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

   // Wechselt das aktuelle Menü zum übergebenen Menü
   public void changeMenu(BattleMenu battleMenu) {
      currentMenu = battleMenu;
      currentSelection = 1;
      switch (battleMenu) {
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
   private void performEnemyAttack() {
      float evaded = Random.Range(0.0f, 1.0f);
      savedEvade = enemyBase.getEvasion() > evaded;

      savedDamage = Mathf.RoundToInt((enemyBase.getBasicAttackDamage() - player.getDefense()) * Random.Range(0.8f, 1.2f));

      enemyCombat.performAttackAnimation(1);

   }

   // Führt einen Angriff des Spielers aus
   private void performPlayerAttack(int attackSkillIndex) {
      if (canAttack && player.getCooldownFromAttack(attackSkillIndex) == 0) {
         float evaded = Random.Range(0.0f, 1.0f);
         savedEvade = enemyBase.getEvasion() > evaded;

         savedDamage = Mathf.RoundToInt((player.getDamageFromAttack(attackSkillIndex) - enemyBase.getDefence()) * Random.Range(0.8f, 1.2f));

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

   // Beendet einen Spielzug
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

   // Fügt entsprechenden Schaden zu
   public void dealDamage() {
      if (savedEvade) {

         if (isPlayersTurn) {
            showText(enemyCombat.transform.position, "Dodge!", Color.white);
         } else {
            showText(playerCombat.transform.position, "Dodge!", Color.white);
         }

         Debug.Log((isPlayersTurn ? "Enemy" : "Player") + " evaded!");
      } else {
         if (isPlayersTurn) {
            enemyBase.changeHP(-savedDamage);
            enemyHPText.text = "HP " + enemyBase.getHP() + "/" + enemyBase.getFullHP();
            enemyCombat.playHurtAnimation();
            showText(enemyCombat.transform.position, savedDamage.ToString(), Color.red);

            if (enemyBase.getHP() <= 0) {
               enemyCombat.playDefeatAnimation();
            }
         } else {
            player.changeHP(-savedDamage);
            playerHPText.text = "HP " + player.getHP() + "/" + player.getFullHP();
            playerCombat.playHurtAnimation();
            showText(playerCombat.transform.position, savedDamage.ToString(), Color.red);

            if (player.getHP() <= 0) {
               playerCombat.playDefeatAnimation();
            }
         }
         Debug.Log((isPlayersTurn ? "Enemy" : "Player") + " hit with " + savedDamage + " damage!");
      }
   }

   // Lässt den Spieler verlieren
   public void playerLost() {
      setInCombat(false);
   }

   // Beendet den Kampf. Der Spieler hat in diesem Fall gewonnen.
   private void endCombat() {
      jumpAndRunUIText.text = "HP " + player.getHP() + "/" + player.getFullHP();
      player.changeXP(enemyBase.getxpReward());
        Debug.Log("current XP: " + player.getXP());
        Debug.Log("XP for level: " + player.getXPForLevel(player.getLevel()));
      xpText.text = "XP " + player.getXP() + "/" + player.getXPForLevel(player.getLevel());
      Destroy(touchedEnemy);
      playerCamera.SetActive(true);
      battleCamera.SetActive(false);

      setInCombat(false);
      Debug.Log("Player won fight!");
      Destroy(enemyCombat.gameObject);

   }

   // Versucht aus dem Kampf zu fliehen
   private void tryEscape() {
      if (isPlayersTurn) {
         float escape = Random.Range(0.0f, 1.0f);
         Debug.Log(escape);
         if (player.getEscapeChance() < escape) {
            showText(playerCombat.transform.position, "Fehlgeschlagen!", Color.white);
            Debug.Log("Failed to escape!");
            endTurn();
         } else {
            HPPanel.SetActive(true);
            Debug.Log("Escape successful!");
            Destroy(enemyCombat.gameObject);
            playerCamera.SetActive(true);
            battleCamera.SetActive(false);
            setInCombat(false);
         }
      }
   }

   // Startet den Kampf
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

         GameObject enemy = Instantiate(enemyList[(int) enemyType.GetEnemyType()], monsterPodium.transform);
         enemyCombat = enemy.GetComponent<CombatBase>();

         playerCombat.setTarget(true);
         enemyCombat.setTarget(false);

         enemyBase = enemyType;
         canAttack = true;

         if (priority > 0) {
            isPlayersTurn = true;
         } else {
            isPlayersTurn = false;
            performEnemyAttack();
         }

         setInCombat(true);
         enemyHPText.text = "HP " + enemyBase.getHP() + "/" + enemyBase.getFullHP();

         reduceCooldowns();
      }
   }
}
