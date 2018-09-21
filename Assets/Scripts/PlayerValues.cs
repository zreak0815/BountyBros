using UnityEngine;

public class PlayerValues : MonoBehaviour {

    // Lebenspunkte
    public int HP = 100;
    public int fullHP = 100;

    // Spielerlevel
    public int playerLevel = 1;
    public int currentXP = 0;

    // EP pro Level
    public int[] XP_PER_LEVEL = {100, 110, 120, 130 };

    // HP Regeneration bei einem Heiltrank
    public const int HP_POTION_REGEN = 15;

   // Prozentwerte für Ausweichen und zusätzliche Verteidigung
   public int defense = 2;
   public float evasion = 0.1f;
   public float escapeChance = 0.3f;

    // Anzahl der Heiltränke des Spielers
    public int hpFlaskAmount = 0;

    // Schadenswerte für Angriffe
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

   public int[] attacksCooldown = { 0, 0, 0, 0 };

   // Use this for initialization
   void Start() {

   }

   // Update is called once per frame
   void Update() {

   }

    // Benutzt einen Heiltrank
    public void useHPPotion()
    {
        HP += HP_POTION_REGEN;
        if (HP > fullHP)
        {
            HP = fullHP;
        }
        hpFlaskAmount--;
    }

    // Verändert die Anzahl der Heiltränke
    public void changeHPFlaskAmount(int amount)
    {
        hpFlaskAmount += amount;
    }

    // Gibt die Anzahl der Heiltränke des Spielers zurück
    public int getHPFlaskAmount()
    {
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

    // Verändert die Spieler-EP um den entsprechenden Wert
    public void changeXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= XP_PER_LEVEL[playerLevel - 1])
        {
            currentXP -= XP_PER_LEVEL[playerLevel - 1];
            playerLevel++;
            //TODO statuspunkte verteilen im character sheet und werte dementsprechend veränderns
        }
    }

    // Gibt die EP zurück
    public int getXP()
    {
        return currentXP;
    }

    // Gibt die EP für das übergebene Level zurück
    public int getXPForLevel(int level)
    {
        return XP_PER_LEVEL[level - 1];
    }

   // Gbt das Spielerlevel zurück
   public int getLevel() {
      return playerLevel;
   }

   // Verändert Verteidigung um den übergebenen Wert
   public void changeDefense(int amount) {
      defense += amount;
   }

    // Gibt die Verteidigung des Spielers zurück
   public int getDefense() {
      return defense;
   }

    // Verändert den Ausweichwert des Spielers
   public void changeEvasion(float amount) {
      evasion += amount;
   }

    //Gibt den Ausweichwert des Spielers zurück
   public float getEvasion() {
      return evasion;
   }

    // Verändert die Chance aus dem Kampf zu entkommen um den übergebenen Wert
   public void changeEscapeChance(float amount) {
      escapeChance += amount;
   }

    // Gibt die Wahrscheinlichkeit um aus dem Kampf zu entkommen zurück
   public float getEscapeChance() {
      return escapeChance;
   }

}
