using UnityEngine;
using System.Collections;

public class StarScaling : MonoBehaviour {

	public GameObject skySphere;
	public float scaleFactor;

	//TILEABLE NEBULA BACKGROUND CREATED FROM MODIFIED NASA IMAGE

	void Start () {
		//SCALES OUTER SPHERE OF STARS RELATIVE TO INNER SPHERE OF NEBULA
		transform.localScale = skySphere.transform.localScale * scaleFactor;
	}

	void Update () {
	
	}
}
