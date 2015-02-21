using UnityEngine;
using System.Collections;

public class TextFollow : MonoBehaviour
{

		private GameObject toFollow;
		public GameObject player;
		public GameController gameControl;
		public int minFontSize, maxFontSize;
		public float scaleDamp;
		private float xPos;
		private float yPos, zPos;
		private Vector3 followVector;

		void Start ()
		{

		}

		void FixedUpdate ()
		{

				//MAKES "HOME" GUITEXT TRACK WITH HOME PLANET
				//BY TRANSLATING 3D POSITION OF PLANET TO CAMERA VIEW COORDINATES OF GUITEXT

				if (gameControl.textState > 0) {
						toFollow = GameObject.FindWithTag ("home");

						xPos = toFollow.transform.position.x;
						yPos = toFollow.transform.localScale.y * 0.5F * 1.3F;
						zPos = toFollow.transform.position.z;
						followVector = new Vector3 (xPos, yPos, zPos);
						//Debug.Log ("Vector to follow: "+followVector);
						transform.position = Camera.main.WorldToViewportPoint (followVector);

						Vector3 vectorToHome = player.transform.position - toFollow.transform.position;
						float distanceToHome = vectorToHome.magnitude;

						float fontScale;
						fontScale = Map (distanceToHome * distanceToHome, 250 * 250, 0, minFontSize, maxFontSize);
						fontScale = Mathf.Clamp (fontScale, minFontSize, maxFontSize);
						Debug.Log (fontScale);
						GetComponent<GUIText> ().fontSize = Mathf.CeilToInt (fontScale);
				}

		}

		//FUNCTION TO MAP FONT-SIZE OF HOME MARKER TO DISTANCE BETWEEN SHIP AND PLANET
		float Map (float value, float min, float max, float mappedMin, float mappedMax)
		{
				float newValue = mappedMin + (mappedMax - mappedMin) * ((value - min) / (max - min));
				return newValue;
		}
}
