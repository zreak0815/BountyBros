using UnityEngine;
using UnityEngine.UI;

public class MakeCharacterSheet : MonoBehaviour {

   private bool KeyIsPressed = false;
   private bool Displayed = true;

   private GameObject levelImage;
   private Text levelLabel;

   // Use this for initialization
   void Start() {
      levelImage = GameObject.Find("Image");
      levelLabel = GameObject.Find("LevelLabel").GetComponent<Text>();
      //Debug.Log("Hello + Displayed");
      levelLabel.text = "Level 1";
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
