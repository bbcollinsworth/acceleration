using UnityEngine;
using System.Collections;

public class PressTextFadeIn : MonoBehaviour
{

		//ADDS A FADE-IN TO THE 'PRESS ANY KEY' TEXT:
		private GUIText pressText;
		private string prevText;
		private float alpha;
		public GameController gameControl;

		void Start ()
		{
				prevText = "";
				alpha = 0.0F;
		}

		void Update ()
		{
				pressText = GetComponent <GUIText> ();

				if (pressText.text != prevText) {
						if (gameControl.reset == true) {
								alpha = 1.0F;
								prevText = pressText.text;
								Color tempColor = pressText.color;
								tempColor.a = alpha;
								pressText.color = tempColor;
						} else {
								StartCoroutine (FadeText ());
						}
				}
		}

		IEnumerator FadeText ()
		{
				prevText = pressText.text;
				Color tempColor = pressText.color;
				tempColor.a = alpha;
				pressText.color = tempColor;
				while (pressText.color.a < 0.8) {
						yield return new WaitForEndOfFrame ();
						alpha += 0.05F;
						tempColor = pressText.color;
						tempColor.a = alpha;
						pressText.color = tempColor;
						tempColor = pressText.color;
				}
				//prevText = pressText.text;
				alpha = 0.0F;

		}
}
