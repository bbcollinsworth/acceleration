using UnityEngine;
using System.Collections;


//PLANET MAPS USED HERE ARE COPYRIGHT (C) JAMES HASTINGS-TREW 
//GUIDELINES FOR THIRD-PARTY USE: http://planetpixelemporium.com/planets.html
 
public class PlanetController : MonoBehaviour
{

		private Vector3 orbit;
		private Vector3 shipOrbit;
		private float xPos, yPos, zPos;
		private float shipX, shipY, shipZ;
		private float xRot, yRot, zRot = 0;
		private GameObject ship;

		public float shipMass, planetMass;
		public float radFactor, repulseScale;
		public int speedMin, speedMax, sizeMin, sizeMax;
		public float rotateMin, rotateMax;
		public float colMin;
		public int timeMult;
		public float distance;
		public Material[] materials;

		private float size;
		private float rotFactor;
		private float speed;
		private int matIndex;
		private int matMax;
		private float r, g, b;
		private float timeOffset, timeFactor;


		// Use this for initialization
		void Start ()
		{
				if (tag == "home") {
				}
		
				timeOffset = Random.value * timeMult;

				//Set planet size
				size = Random.Range (sizeMin, sizeMax);
				transform.localScale = new Vector3 (size, size, size);

				//Get distance from sun for use in planet orbit
				distance = this.transform.position.x;

				//set speed
				speed = Mathf.FloorToInt (Random.Range (speedMin, speedMax));

				//Pick planet material index
				matMax = materials.Length;
				matIndex = Mathf.FloorToInt (Random.Range (0.0F, matMax));

				//set planet hue adjustment
				r = Random.Range (colMin, 1.0F);
				g = Random.Range (colMin, 1.0F);
				b = Random.Range (colMin, 1.0F);

				//Set planet material and hue adjustment
				renderer.material = materials [matIndex];
				renderer.material.color = new Color (r, g, b, 1.0F);

				//Set planet rotation step
				rotFactor = Random.Range (-rotateMax, rotateMax);
				if (rotFactor < rotateMin && rotFactor >= 0.0F) {
						rotFactor += rotateMin;
				} else if (rotFactor > -1 * rotateMin && rotFactor < 0.0F) {
						rotFactor += -1 * rotateMin;
				}
		}

		void FixedUpdate ()
		{
				timeFactor = Time.fixedTime + timeOffset;
				xPos = (Mathf.Sin (timeFactor / speed)) * distance;
				yPos = 0;
				zPos = (Mathf.Cos (timeFactor / speed)) * distance;

				xRot += 0;
				yRot += rotFactor;
				zRot += 0;

				orbit = new Vector3 (xPos, yPos, zPos);
				rigidbody.position = orbit;
				rigidbody.rotation = Quaternion.Euler (xRot, yRot, zRot);

				ship = GameObject.FindWithTag ("Player");

				//GRAVITY IMPLEMENTATION:
				Vector3 shipToPlanet = rigidbody.position - ship.rigidbody.position;
				float shipDistance = shipToPlanet.magnitude;

				float g = 0.5F;
				//planetMass *= transform.localScale *0.1;

				float strength = (g * shipMass * planetMass) / (shipDistance);// * shipDistance * 0.25F);
				Vector3 forceToAdd = shipToPlanet.normalized * strength;
				ship.rigidbody.AddForce (forceToAdd);

				//REPULSION FORCE:
				float radius = transform.localScale.x / 2 * radFactor;
				//Debug.Log (this.gameObject.name + " tag: " + this.gameObject.tag);
				if (this.gameObject.tag != "home") {
						if (shipDistance < radius) {
								shipToPlanet *= 1.0F - shipDistance / radius;
								ship.rigidbody.AddForce (-shipToPlanet * repulseScale);
						}
				}
		}
}
