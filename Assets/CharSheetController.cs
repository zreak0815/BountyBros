using UnityEngine;
using UnityEngine.UI;

public class CharSheetController : MonoBehaviour {
   public static int StatPoints = 0;
   public static int Atk = 0;
   public static int Def = 0;
   public static int Fcs = 0;
   public static int Evs = 0;
   public static int Lvl = 1;

   public enum StatusPoints {
      Atack,
      Defense,
      Focus,
      Evasion
   }

   static Text StatPts;
   static Text Atk_T;
   static Text Def_T;
   static Text Fcs_T;
   static Text Evs_T;
   static Text Lvl_T;

   // Use this for initialization
   private void Start() {
      StatPts = GameObject.Find("Punkte").GetComponent<Text>();
      StatPts.text = StatPoints + (StatPoints == 1 ? " Statuspunkt" : " Statuspunkte");
      Atk_T = GameObject.Find("TextAtk").GetComponent<Text>();
      Def_T = GameObject.Find("TextDef").GetComponent<Text>();
      Fcs_T = GameObject.Find("TextFcs").GetComponent<Text>();
      Evs_T = GameObject.Find("TextEvasion").GetComponent<Text>();
      Lvl_T = GameObject.Find("LevelText").GetComponent<Text>();
   }

   public static void AddStatPoint(StatusPoints aType) {
      Text StatPts = GameObject.Find("Punkte").GetComponent<Text>();
      StatPts.text = StatPoints + " Statuspunkt";
      if (StatPoints > 0) {
         switch (aType) {
            case StatusPoints.Atack: {
                  Atk++;
                  Atk_T.text = Atk.ToString();
               }
               break;

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
            case StatusPoints.Focus: {
                  Fcs++;
                  Fcs_T.text = Fcs.ToString();
                  break;
               }
         }
         StatPoints--;
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
