using UnityEngine;
using UnityEngine.UI;

public class CharSheetController : MonoBehaviour {
   public static int StatPoints = 2;
   public static int Atk = 0;
   public static int Def = 0;
   public static int HP = 0;
   public static int Evs = 0;
   public static int Lvl = 1;

   private static Text HPPanelText;

   public enum StatusPoints {
      Atack,
      Defense,
      Health,
      Evasion
   }

   static Text StatPts;
   static Text Atk_T;
   static Text Def_T;
   static Text HP_T;
   static Text Evs_T;
   static Text Lvl_T;

   // Use this for initialization
   private void Start() {
      StatPts = GameObject.Find("Punkte").GetComponent<Text>();
      StatPts.text = StatPoints + (StatPoints == 1 ? " Statuspunkt" : " Statuspunkte");
      Atk_T = GameObject.Find("TextAtk").GetComponent<Text>();
      Def_T = GameObject.Find("TextDef").GetComponent<Text>();
      HP_T = GameObject.Find("TextFcs").GetComponent<Text>();
      Evs_T = GameObject.Find("TextEvasion").GetComponent<Text>();
      Lvl_T = GameObject.Find("LevelText").GetComponent<Text>();
      HPPanelText = GameObject.Find("HPPanelText").GetComponent<Text>();
   }

   public static void AddStatPoint(StatusPoints aType) {
      Text StatPts = GameObject.Find("Punkte").GetComponent<Text>();
      if (StatPoints > 0) {
         switch (aType) {
            case StatusPoints.Atack: {
                  Atk++;
                  Atk_T.text = Atk.ToString();
                  break;
               }

            case StatusPoints.Defense: {
                  Def++;
                  Def_T.text = Def.ToString();
                  break;
               }
            case StatusPoints.Evasion: {
                  Evs++;
                  Evs_T.text = Evs.ToString();
                  break;
               }
            case StatusPoints.Health: {
                  HP++;
                  HP_T.text = HP.ToString();
                  break;
               }
         }
         StatPoints--;
         StatPts.text = StatPoints + " Statuspunkte";
      }
   }

   // Update is called once per frame
   void Update() {

   }

   public static void IncrementOnLevelUp() {
      StatPoints += 2;
      ++Lvl;
      StatPts.text = StatPoints + (StatPoints == 1 ? " Statuspunkt" : " Statuspunkte");
      Lvl_T.text = "Level " + Lvl;
   }
}
