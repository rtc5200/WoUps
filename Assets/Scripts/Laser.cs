using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	void Start(){
		var lr = GetComponent<LineRenderer>();
		var ss = transform.root.gameObject.GetComponent<ShipStats>();
		Color c = ss == null ? Color.red : ss.color;
			lr.widthMultiplier = 0.2f;
			Gradient grad = new Gradient();
			grad.SetKeys(
				new GradientColorKey[]{new GradientColorKey(c,0f),new GradientColorKey(c,1f)},
				new GradientAlphaKey[]{new GradientAlphaKey(1f,0f),new GradientAlphaKey(1f,1f)}
			);
			lr.colorGradient = grad;	
	}
	
	// Update is called once per frame
	void Update () => GetComponent<LineRenderer>().SetPositions(new Vector3[]{transform.position,transform.position + transform.forward.normalized * 1000f});
}
