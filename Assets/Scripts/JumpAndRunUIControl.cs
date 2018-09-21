using UnityEngine;
using UnityEngine.UI;

/*
 Klasse für das Charaktermenü
     */

public class JumpAndRunUIControl : MonoBehaviour {
   private bool KeyIsPressed = false;
   private bool Displayed = false;

   private GameObject levelImage;
   private Text levelLabel;

   public PlayerValues stats { get; set; }

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
