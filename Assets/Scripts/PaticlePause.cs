using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaticlePause : MonoBehaviour {

	void Awake () {
		foreach(ParticleSystem p in this.GetComponentsInChildren(typeof(ParticleSystem),true)){
			p.Pause(true);
		}
	}
	
}
