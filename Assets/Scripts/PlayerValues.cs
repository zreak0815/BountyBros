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

   public int[] XP_PER_LEVEL = { 100, 110, 120, 130 };

   public const int HP_POTION_REGEN = 15;

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

   public float escapeChance = 0.3f;

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
   public int cooldownHeavyAttack = 3;
   public int cooldownPreciseShot = 3;
   public int cooldownStompAttack = 5;
   GameObject CharSheet;


   Button Atk;
   Button Def;
   Button Fcs;
   Button Evs;


   // Use this for initialization
   void Start() {
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

   public void useHPPotion() {
      HP += HP_POTION_REGEN;
      if (HP > fullHP) {
         HP = fullHP;
      }
      hpFlaskAmount--;
   }

   public void changeHPFlaskAmount(int amount) {
      hpFlaskAmount += amount;
   }

   public int getHPFlaskAmount() {
      return hpFlaskAmount;
   }

   public int getCooldownFromAttack(int attackSkillIndex) {
      switch (attackSkillIndex) {
         case 1:
            return cooldownLightAttack;
         case 2:
            return cooldownHeavyAttack;
         case 3:
            return cooldownPreciseShot;
         case 4:
            return cooldownStompAttack;
         default:
            // wird nie eintreten
            return -1;
      }
   }

   public void setCooldownFromAttack(int attackSkillIndex, int value) {
      switch (attackSkillIndex) {
         case 1:
            // lightAttack besitzt keinen cooldown
            break;
         case 2:
            cooldownHeavyAttack = value;
            break;
         case 3:
            cooldownPreciseShot = value;
            break;
         case 4:
            cooldownStompAttack = value;
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
   }

   // Gibt die HP zurück
   public int getHP() {
      return HP;
   }

   public int getFullHP() {
      return fullHP;
   }

   // Erhöht das Level um 1
   public void incLevel() {
      playerLevel++;
   }

   public void changeXP(int amount) {
      currentXP += amount;
      if (currentXP >= XP_PER_LEVEL[playerLevel - 1]) {
         currentXP -= XP_PER_LEVEL[playerLevel - 1];
         playerLevel++;
         CharSheetController.IncrementByOne();
      }
   }

   public int getXP() {
      return currentXP;
   }

   public int getXPForLevel(int level) {
      return XP_PER_LEVEL[level < XP_PER_LEVEL.Length ? level - 1 : XP_PER_LEVEL.Length - 1];
   }

   // Gbt das Spielerlevel zurück
   public int getLevel() {
      return playerLevel;
   }

   public int getDefense() {
      return defense;
   }


   public float getEvasion() {
      return evasion;
   }

   public float getEscapeChance() {
      return escapeChance;//Level in Charsheet
   }
}
