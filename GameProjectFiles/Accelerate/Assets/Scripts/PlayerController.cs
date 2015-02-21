using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

		public GameController gameControl;
		public float countdownDelay;
		public GameObject planet;
		public AudioClip alarmSound;

		private float shipX, shipY, shipZ;
		private Vector3 shipOrbit;

		public GameObject rockets;
		public float speed;
		public float acceleration;
		public GUIText countdownText;
		public GUIText scoreText;
		public GUIText winText;
		public GameObject homeText;

		private int score, prevScore;
		private bool shipOn = false;
		private int shipStart = 0;
		private Vector3 relativePoint;

		void Start ()
		{
				score = 0;
				prevScore = 0;
				scoreText.text = "";
				winText.text = "";
				countdownText.text = "";
				rockets.SetActive (false);
		}

		//FUNCTION TO CHECK IF HOME PLANET IS AHEAD OR BEHIND SHIP
		//IF BEHIND, GUITEXT HOME INDICATOR IS HIDDEN
		void HomeCheck ()
		{
				GameObject home = GameObject.FindWithTag ("home");

				relativePoint = transform.InverseTransformPoint (home.transform.position);
				if (relativePoint.z > 0) {
						homeText.SetActive (true);
						Debug.Log ("The planet is in front of this object");
				} else {
						homeText.SetActive (false);
						Debug.Log ("The planet is behind of this object");
				}
		
		}
	
		void FixedUpdate ()
		{
				if (gameControl.textState > 0) {
						HomeCheck ();
				}

				float moveHorizontal = Input.GetAxis ("Horizontal");

				//STEERS THE SHIP:
				rigidbody.AddTorque (0.0F, moveHorizontal * speed * Time.deltaTime, 0.0F);
				//Debug.Log ("Time since start is " + timeFactor);

				if (gameControl.startCountdown == true && shipOn == false) {
						shipStart ++;
						shipOn = true;
				}

				// STATE SYSTEM TO TURN ON SHIP:
				if (shipStart == 1) {
						StartCoroutine (Countdown ());
						SetCountText ();
						shipStart++;
				} else if (shipStart == 3) {
						gameControl.textState ++;
						rigidbody.isKinematic = false;
						rockets.SetActive (true);
						shipStart ++;
				} else if (shipStart == 4) {

						engineFire ();
				}

		}

		//COROUTINE FOR ENGINE FIRE COUNTDOWN
		IEnumerator Countdown ()
		{
				for (int i = 3; i > 0; i--) {
						audio.Play ();
						countdownText.text = i.ToString ();
						yield return new WaitForSeconds (1.0F);
						countdownText.text = "";
				}
				shipStart ++;

				yield return new WaitForSeconds (1.0F);
				countdownText.fontSize = 50;
				for (int i = 4; i > 0; i--) {

						winText.text = "< STEER >";
						for (int j=3; j> 0; j--) {
								audio.Play ();
								yield return new WaitForSeconds (0.2F);
						}
						winText.text = "";
						yield return new WaitForSeconds (0.6F);
				}
		}
		
		//FUNCTION FOR CONSTANT ACCELERATION FORCE FROM ENGINE ONCE GAME STARTS:
		void engineFire ()
		{
				rigidbody.AddRelativeForce (Vector3.forward * acceleration);
				Debug.Log ("Current velocity is " + rigidbody.velocity);
		}

		//FUNCTION FOR WHEN HOME PLANET IS SUCCESSFULLY REACHED:
		//ADVANCES SCORE, SENDS RESET GAME SIGNAL TO GAME CONTROLLER 
		void OnTriggerEnter (Collider other)
		{
				if (other.gameObject.tag == "home" && gameControl.reset == false) {

						score ++;
						SetCountText ();
						gameControl.reset = true;

				}
		}

		//FUNCTION TO UPDATE SCORE, GIVE SUCCESS MESSAGE TO PLAYER WHEN PLANET REACHED:
		void SetCountText ()
		{
				scoreText.text = "Score: " + score.ToString ();
				if (score != prevScore) {
						audio.Play ();
						winText.text = "";
						winText.fontSize = 40;
						Color warningColor = gameControl.warningColor;
						warningColor.a = gameControl.textAlpha;
						winText.color = warningColor;
						winText.lineSpacing = 0.7F;
						winText.text = "YOU MADE IT! \n \n Score +1";
				}
		}
}

