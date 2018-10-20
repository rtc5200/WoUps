using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAIShipController : ShipController {
	private float nextFire;
	protected override void AIThink(){
		Speed = 0.5f;
		if(Time.time > nextFire){
			Fire();
			nextFire = Time.time + 3f;
		}
	}
}