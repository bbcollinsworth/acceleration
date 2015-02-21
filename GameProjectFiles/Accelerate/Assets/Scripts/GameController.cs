using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

		public GUIText launchText;
		public GUIText homeText;
		public GUIText pressText;
		public float letterPause = 0.05f, textAlpha;
		public AudioClip sound;
		string message;
		string text;
		public string[] gameInstructions;
		public string[] resetInstructions;
		public string[] warningMessage;
		public GameObject flashColorSrc;
		public GameObject sun;
		public GameObject player;
		public float sunPadding;
		public GameObject planet;
		public int planets;
		public int spacing;
		public float spaceVary;
		private float distance;
		private float distVary;
		private int rotation;
		private float sunRad;
		private GameObject home;
		private Color normalColor;
		private Color flashColor;
		private Shader normalShader;
		private Shader flashShader;
		public int textState;
		public bool startCountdown = false;
		private int prevTextState;
		private float debounce, homeMarkDelay;
		private bool okToAdvance;
		private int homePicked, prevPicked;
		public bool reset = false;
		private bool prevResetState = false;
		private bool nextPlanetMsg = false;
		public Color warningColor;


		// Use this for initialization
		void Start ()
		{

				prevTextState = 1;
				textState = 0;
				homeText.text = "";
				prevPicked = 0;
				//pressText.text = "";
				okToAdvance = true;

				//TO GET PLANETS TO ORBIT FROM INITIAL POSITIONS
				sunRad = sun.transform.localScale.x * sunPadding;
				//Debug.Log ("SunRad is " + sunRad);

				for (int i = 0; i < planets; i ++) {
						distVary = Random.Range (-spaceVary, spaceVary);
						distance = spacing * (i + 1) + distVary + sunRad;
						rotation = Mathf.FloorToInt (Random.value * 30);
						Instantiate (planet, new Vector3 (distance, 0, 0), Quaternion.Euler (0.0F, 0.0F, rotation));
						planet.name = "planet" + (i + 1).ToString ();

						planet.tag = "Untagged";
						planet.collider.isTrigger = false;
		
				}
		}
		
		//FUNCTION TO SELECT HOME PLANET:
		void PickHome ()
		{
				homePicked = Mathf.CeilToInt (Random.Range (0.0F, (planets - 1)));

				//THIS IS IMPORTANT TO PREVENT ERROR 
				//WITH PICKED PLANET OUTSIDE RANGE OF ACTUAL PLANETS:
				if (homePicked > 5) {
						homePicked = 1;
				}
				if (homePicked == prevPicked) {
						homePicked --;
						if (homePicked < 1) {
								homePicked = planets - 1;
						}

				}
				Debug.Log ("Home is " + homePicked);
				string pickedPlanet = "planet" + homePicked.ToString () + "(Clone)";

				//FIND AND TAG SELECTED PLANET, EXPAND COLLIDER RADIUS SO GETTING CLOSE ADDS A POINT
				home = GameObject.Find (pickedPlanet);
				Debug.Log (pickedPlanet);
				home.tag = "home";
				home.GetComponent <SphereCollider> ().radius = 1.0F;
				if (textState > 2) {
						home.collider.isTrigger = true;
				}

				//SET FLASH COLORS OF HOME PLANET:
				normalColor = home.renderer.material.color;
				normalShader = Shader.Find ("Diffuse");
		
				flashColor = RGBConvert (150, 248, 142);
				flashShader = Shader.Find ("Self-Illumin/Diffuse");
		}

		Color RGBConvert (float _r, float _g, float _b)
		{ 
				float r = _r / 255.0F;
				float g = _g / 255.0F;
				float b = _b / 255.0F;

				return new Color (r, g, b);

		}

		void ResetGame ()
		{
				//RESETS OLD HOME PLANET TO NORMAL, SELECTS NEW HOME, GIVES PLAYER INSTRUCTIONS
				homeText.text = "";
				prevResetState = true;
				player.rigidbody.isKinematic = false;
				home.tag = "Untagged";
				home.collider.isTrigger = false;
				home.GetComponent <SphereCollider> ().radius = 0.5F;

				home.renderer.material.color = normalColor;
				home.renderer.material.shader = normalShader; 

				PickHome ();

				StartCoroutine (Pause ());

				okToAdvance = false;


		}

		// COROUTINE TO BRIEFLY PAUSE GAME WHEN PLAYER REACHES A PLANET:
		IEnumerator Pause ()
		{
				float pauseEndTime = Time.realtimeSinceStartup + 2.0F;
				Time.timeScale = 0.00001F;
				while (Time.realtimeSinceStartup < pauseEndTime) {
						yield return 0;
				}
				Time.timeScale = 1.0F;
				nextPlanetMsg = true;
				reset = false;
		}


		void Update ()
		{

		//GAME START STRUCTURE:
				//FOR INTRO SCREENS:
				if (textState < 2) {
						if (okToAdvance == true) {
								pressText.text = "( Click or press any key... )";
								if (Input.anyKeyDown) {
										textState ++;
								}
						} else {
								pressText.text = "";
						}
				} 
				//FOR EACH CHANGE IN STATE:
				if (textState != prevTextState) {
						//FIRST SELECTION OF HOME PLANET:
						if (textState == 1) {
								PickHome ();
								StartCoroutine (HomeTextFlash ());
								homeMarkDelay = Time.fixedTime + 1.0F;
						}
						advanceText (textState);
						//IMMEDIATELY RESETTING THIS MAKES SURE 
						//THAT PLANET SELECTION IS ONLY CALLED ONCE:
						prevTextState = textState;
						prevPicked = homePicked;
				}
				//GIVES A SLIGHT DELAY TO THE SELECTION OF HOME, INTIATION OF FLASH ON PLANET:
				if (textState > 0 && Time.fixedTime > homeMarkDelay) {
						if (Time.fixedTime % 1 >= 0.5) { 
								home.renderer.material.shader = flashShader;
								home.renderer.material.color = flashColor;
						} else {
								home.renderer.material.shader = normalShader;
								home.renderer.material.color = normalColor;
						}
				} 

		}

		void FixedUpdate ()
		{
				//RESET STATE PASSED FROM PLAYER CONTROLLER
				if (reset == true) {
						ResetGame ();
				}

				if (nextPlanetMsg == true) {
						StartCoroutine (TypeText (resetInstructions, true));
						StartCoroutine (HomeTextFlash ());
						prevPicked = homePicked;
						nextPlanetMsg = false;
				}

		}
	
		IEnumerator HomeTextFlash ()
		{
				yield return new WaitForSeconds (1.0F);
				string[] arrowsL = {">  >  >  ", "> > > ",">>>"};
				string[] arrowsR = {"  <  <  <", " < < <","<<<"};
				for (int i = 0; i < 3; i++) {

						homeText.text = arrowsL [i] + " HOME " + arrowsR [i];
						yield return new WaitForSeconds (0.3F);
				}
		}

		//FUNCTION TO ADVANCE GAME THROUGH STARTUP STATE SYSTEM:
		void advanceText (int _state)
		{
				if (_state == 0) {
						launchText.text = ("A C C E L E R A T I O N");

				} else if (_state == 1) {
						launchText.text = "";
						StartCoroutine (TypeText (gameInstructions, false));
						okToAdvance = false;

				} else if (_state == 2) {
						pressText.text = "";
						launchText.text = "";
						launchText.fontSize = 40;
						warningColor = RGBConvert (255, 221, 26);
						warningColor.a = textAlpha;
						launchText.color = warningColor;
						launchText.lineSpacing = 0.7F;
						StartCoroutine (TypeWarning ());

				} else if (_state == 3) {
						home.collider.isTrigger = true;
						launchText.text = "";
						textState++;
				}
		}

		//THANKS TO clunk47 FOR THE BASIC TEXT-APPEARS-LETTER-BY-LETTER SCRIPT USED HERE: 
		// http://answers.unity3d.com/questions/264717/gui-text-show-one-letter-at-a-time.html

		IEnumerator TypeText (string[] textToType, bool resetCalled)
		{
				launchText.fontSize = 25;
				Color textColor = flashColor;
				textColor.a = textAlpha;
				launchText.color = textColor;
				launchText.text = "";
				foreach (string line in textToType) {
						foreach (char letter in line.ToCharArray()) {
								launchText.text += letter;
								if (sound) {
										audio.clip = sound;
										audio.Play ();
								}

								yield return 0;
								yield return new WaitForSeconds (letterPause);
						}   
						launchText.text += "\n \n";
						yield return new WaitForSeconds (letterPause * 20.0F);
				}
				okToAdvance = true;
				if (resetCalled == true) {
						yield return new WaitForSeconds (5.0F);
						launchText.text = "";
				}

		}

		IEnumerator TypeWarning ()
		{
				foreach (string line in warningMessage) {
						audio.Play ();
						launchText.text += line + "\n \n";
						yield return 0;
						yield return new WaitForSeconds (letterPause * 100);
				}      
				startCountdown = true;
		
		}
}
