using UnityEngine;
using System.Collections;

public class EngineFlicker : MonoBehaviour {
	
	private Light thrustLight;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		thrustLight = GetComponent <Light> ();
		
		//thrustLight.range = Random.Range (1, 3);
		thrustLight.intensity = Random.value * 1.2F;
		
		//Color lightFlicker = thrustLight.color;
		//lightFlicker.b += 0.1F*(Random.Range (-1, 1));
		//lightFlicker.g += 0.1F*(Random.Range (-1, 1));
		//thrustLight.color = lightFlicker;
	}
}
