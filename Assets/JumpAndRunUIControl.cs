using UnityEngine;
using UnityEngine.UI;

public class JumpAndRunUIControl : MonoBehaviour {
   private bool KeyIsPressed = false;
   private bool Displayed = false;

   private GameObject levelImage;
   private Text levelLabel;

   // Use this for initialization
   void Start() {
      levelImage = GameObject.Find("CharacterSheet");
      levelImage.SetActive(Displayed);
   }

   // Update is called once per frame
   void Update() {
      if (Input.GetKeyDown(KeyCode.C)) {
         Displayed = !Displayed;
         Debug.Log(Displayed);
         levelImage.SetActive(Displayed);
      }
   }
}
/*
 using UnityEngine;
using UnityEngine.UI;

public class CharacterSheetControl : MonoBehaviour {
   private bool KeyIsPressed = false;
   private bool Displayed = true;

   private GameObject levelImage;
   private Text levelLabel;

   // Use this for initialization
   void Start() {
      levelImage = GameObject.Find("CharacterSheet");
      levelImage.SetActive(Displayed);
   }

   // Update is called once per frame
   void Update() {
      if (Input.GetKeyDown(KeyCode.C)) {
         Displayed = !Displayed;
         Debug.Log(Displayed);
         levelImage.SetActive(Displayed);
      }
   }
}
    */
