using UnityEngine;

public class PlayerValues : MonoBehaviour {

   // Lebenspunkte
   public int HP = 100;
   public int fullHP = 100;
   public int level = 1;

   // Prozentwerte für Ausweichen und zusätzliche Verteidigung
   public int defense = 2;
   public float evasion = 0.1f;
   public float escapeChance = 0.3f;

   public int lightAttackDamage = 10;
   public int heavyAttackDamage = 20;
   public int preciseShotDamage = 15;
   public int stompAttackDamage = 10;

   private const int INITIAL_COOLDOWN_LIGHTATTACK = 0;
   private const int INITIAL_COOLDOWN_HEAVYATTACK = 0;
   private const int INITIAL_COOLDOWN_PRECISESHOT = 0;
   private const int INITIAL_COOLDOWN_STOMPATTACK = 0;

   public int cooldownLightAttack = 0;
   public int cooldownHeavyAttack = 3;
   public int cooldownPreciseShot = 3;
   public int cooldownStompAttack = 5;

   // Use this for initialization
   void Start() {

   }

   // Update is called once per frame
   void Update() {

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
      level++;
   }

   // Gbt das Spielerlevel zurück
   public int getLevel() {
      return level;
   }

   // Verändert Verteidigung um den übergebenen Wert
   public void changeDefense(int amount) {
      defense += amount;
   }

   public int getDefense() {
      return defense;
   }

   public void changeEvasion(float amount) {
      evasion += amount;
   }

   public float getEvasion() {
      return evasion;
   }

   public void changeEscapeChance(float amount) {
      escapeChance += amount;
   }

   public float getEscapeChance() {
      return escapeChance;
   }

}
