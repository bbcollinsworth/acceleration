using UnityEngine;
using System.Collections;

public class SunController : MonoBehaviour {

	//public Color startColor;
	//public Color endColor;
	//public float duration = 1.0F;
	public float sunRotation;
	//public Material material;

	private float xRot, yRot, zRot = 0.0F;

	void Update() {

		yRot += sunRotation;
		rigidbody.rotation = Quaternion.Euler (xRot, yRot, zRot);
	}
}
