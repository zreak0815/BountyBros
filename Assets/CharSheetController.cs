using UnityEngine;
using UnityEngine.UI;

public class CharSheetController : MonoBehaviour {
   public static int StatPoints = 1;
   public static int Atk = 1;
   public static int Def = 1;
   public static int Fcs = 1;
   public static int Evs = 1;

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

   // Use this for initialization
   void Start() {
      StatPts = GameObject.Find("Punkte").GetComponent<Text>();
      StatPts.text = StatPoints + (StatPoints == 1 ? " Statuspunkt" : " Statuspunkte");
      Atk_T = GameObject.Find("TextAtk").GetComponent<Text>();
      Def_T = GameObject.Find("TextDef").GetComponent<Text>();
      Fcs_T = GameObject.Find("TextFcs").GetComponent<Text>();
      Evs_T = GameObject.Find("TextEvasion").GetComponent<Text>();
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

   public static void IncrementByOne() {
      StatPoints++;
      //StatPts = GameObject.Find("Punkte").GetComponent<Text>();
      StatPts.text = StatPoints + (StatPoints == 1 ? " Statuspunkt" : " Statuspunkte");
   }
}
