using UnityEngine;
using UnityEngine.UI;

public class PlayerValues : MonoBehaviour {
   // Lebenspunkte
   public int HP = 100;
   private int prv_fullHP = 100;
   public int fullHP {
      get {
         return prv_fullHP + (CharSheetController.Fcs * 10);
      }
   }
   public int playerLevel = 1;
   public int currentXP = 0;

    // EP pro Level
   public int[] XP_PER_LEVEL = { 100, 110, 120, 130 };

    // HP Regeneration bei einem Heiltrank
   private const int HP_POTION_REGEN = 30;

   // Prozentwerte für Ausweichen und zusätzliche Verteidigung
   private int prv_defense = 2;
   public int defense {
      get {
         return prv_defense + Mathf.RoundToInt(prv_defense * CharSheetController.Def * (float) 0.2);
      }
   }
   public float evasion {
      get {
         return 0.1f * Mathf.Sqrt(CharSheetController.Evs);
      }
   }

   public float escapeChance = 0.5f;

    // Anzahl der Heiltränke des Spielers
   public int hpFlaskAmount = 0;

   // Schadenswerte für Angriffe
   private int prv_lightAttackDamage = 10;
   public int lightAttackDamage {
      get {
         return prv_lightAttackDamage + Mathf.RoundToInt(prv_lightAttackDamage * CharSheetController.Atk * (float) 0.2);
      }
   }
   private int prv_heavyAttackDamage = 20;
   public int heavyAttackDamage {
      get {
         return prv_heavyAttackDamage + Mathf.RoundToInt(prv_heavyAttackDamage * CharSheetController.Atk * (float) 0.2);
      }
   }
   private int prv_preciseShotDamage = 15;
   public int preciseShotDamage {
      get {
         return prv_preciseShotDamage + Mathf.RoundToInt(prv_preciseShotDamage * CharSheetController.Atk * (float) 0.2);
      }
   }
   private int prv_stompAttackDamage = 10;
   public int stompAttackDamage {
      get {
         return prv_stompAttackDamage + Mathf.RoundToInt(prv_stompAttackDamage * CharSheetController.Atk * (float) 0.2);
      }
   }

   private const int INITIAL_COOLDOWN_LIGHTATTACK = 0;
   private const int INITIAL_COOLDOWN_HEAVYATTACK = 0;
   private const int INITIAL_COOLDOWN_PRECISESHOT = 0;
   private const int INITIAL_COOLDOWN_STOMPATTACK = 0;

   public int cooldownLightAttack = 0;
   public int cooldownHeavyAttack = 4;
   public int cooldownPreciseShot = 3;
   public int cooldownStompAttack = 5;
   GameObject CharSheet;

   public int[] attacksCooldown = { 0, 0, 0, 0 };
   Button Atk;
   Button Def;
   Button Fcs;
   Button Evs;

    private GlobalStats globalStats;

   // Use this for initialization
   void Start() {

        globalStats = FindObjectOfType<GlobalStats>();
        if (globalStats == null) {
            globalStats = new GameObject().AddComponent<GlobalStats>();
        }
        else {
            globalStats.loadStats(this);
        }

        CharSheet = GameObject.Find("CharacterSheet");
      Atk = GameObject.Find("AddAtack").GetComponent<Button>();
      Atk.onClick.AddListener(AddAtk);
      Def = GameObject.Find("AddDef").GetComponent<Button>();
      Def.onClick.AddListener(AddDef);
      Fcs = GameObject.Find("AddFocus").GetComponent<Button>();
      Fcs.onClick.AddListener(AddFcs);
      Evs = GameObject.Find("AddEvasion").GetComponent<Button>();
      Evs.onClick.AddListener(AddEvs);

        
   }

   void AddAtk() {
      CharSheetController.AddStatPoint(CharSheetController.StatusPoints.Atack);
   }

   void AddDef() {
      CharSheetController.AddStatPoint(CharSheetController.StatusPoints.Defense);
   }

   void AddFcs() {
      CharSheetController.AddStatPoint(CharSheetController.StatusPoints.Focus);
   }

   void AddEvs() {
      CharSheetController.AddStatPoint(CharSheetController.StatusPoints.Evasion);
   }

   private static bool firstStart = true;

   // Update is called once per frame
   void Update() {
      //In der Start Methode wird bei SetActive(false) eine NullRef ausgelöst, deswegen hier erst initialisieren.
      if (firstStart) {
         firstStart = false;
         Cursor.visible = false;
         CharSheet.SetActive(false);
      }
      if (CharSheet != null && Input.GetKeyDown(KeyCode.C)) {
         bool lcl_CharSheetVisible = CharSheet.activeSelf;
         CharSheet.SetActive(!lcl_CharSheetVisible);
         Cursor.visible = !lcl_CharSheetVisible;
      }
   }

    public int getHPPotionRegen() {
        return HP_POTION_REGEN;
    }

    // Benutzt einen Heiltrank
   public void useHPPotion() {
      HP += HP_POTION_REGEN;
      if (HP > fullHP) {
         HP = fullHP;
      }
      hpFlaskAmount--;
   }

    // Verändert die Anzahl der Heiltränke
   public void changeHPFlaskAmount(int amount) {
      hpFlaskAmount += amount;
   }

    // Gibt die Anzahl der Heiltränke des Spielers zurück
   public int getHPFlaskAmount() {
      return hpFlaskAmount;

   }

    public void reduceCooldowns() {
        for (int i = 0; i < attacksCooldown.Length; i++) {
            attacksCooldown[i] = Mathf.Max(0, attacksCooldown[i] - 1);
        }
    }

    public int getCooldownFromAttack(int attackSkillIndex) {
        return attacksCooldown[attackSkillIndex - 1];
    }

   public void setCooldownFromAttack(int attackSkillIndex) {
      switch (attackSkillIndex) {
         case 1:
            // lightAttack besitzt keinen cooldown
            break;
         case 2:
                attacksCooldown[attackSkillIndex - 1] = cooldownHeavyAttack;
            break;
         case 3:
                attacksCooldown[attackSkillIndex - 1] = cooldownPreciseShot;
                break;
         case 4:
                attacksCooldown[attackSkillIndex - 1] = cooldownStompAttack;
                break;
      }
   }

   // Gibt den Schaden des übergebenen Angriffs zurück
   public int getDamageFromAttack(int attackSkillIndex) {
      switch (attackSkillIndex) {
         case 1:
            return lightAttackDamage;
         case 2:
            return heavyAttackDamage;
         case 3:
            return preciseShotDamage;
         case 4:
            return stompAttackDamage;
         default:
            // wird nie eintreten
            return -1;
      }
   }

    // Verändert die HP um den übergebenen Wert
    public void changeHP(int amount) {
        HP += amount;
        if (HP < 0) {
            HP = 0;
        }
    }

    // Gibt die HP zurück
    public int getHP() {
      return HP;
   }

    // Gibt die maximale HP des Spielers zurück
   public int getFullHP() {
      return fullHP;
   }

   // Erhöht das Level um 1
   public void incLevel() {
      playerLevel++;
   }

    // Erhöht das Level um 1
    public void setLevel(int level) {
        playerLevel = level;
    }

    // Verändert die Spieler-EP um den entsprechenden Wert
    public void changeXP(int amount) {
      currentXP += amount;
      if (currentXP >= XP_PER_LEVEL[playerLevel - 1]) {
         currentXP -= XP_PER_LEVEL[playerLevel - 1];
         playerLevel++;
         CharSheetController.IncrementByOne();
      }
   }

    // Gibt die EP zurück
   public int getXP() {
      return currentXP;
   }

    // Gibt die EP für das übergebene Level zurück
   public int getXPForLevel(int level) {
      return XP_PER_LEVEL[level < XP_PER_LEVEL.Length ? level - 1 : XP_PER_LEVEL.Length - 1];
   }

   // Gbt das Spielerlevel zurück
   public int getLevel() {
      return playerLevel;
   }

    // Gibt die Verteidigung des Spielers zurück
   public int getDefense() {
      return defense;
   }


    //Gibt den Ausweichwert des Spielers zurück
   public float getEvasion() {
      return evasion;
   }

    // Gibt die Wahrscheinlichkeit um aus dem Kampf zu entkommen zurück
   public float getEscapeChance() {
      return escapeChance;//Level in Charsheet
   }

    public void saveStats() {
        globalStats.saveStats(this);
    }
}
