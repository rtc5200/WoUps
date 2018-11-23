using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : ShipController {

	protected override void AIThink(){
		Speed = Input.GetAxis("Vertical");		//前進後進スピードをW(Up)/S(Down)キー押下量に
		RoSpeed = Input.GetAxis("Horizontal");	//回転スピードをA(Left)/D(Right)キー押下量に
		if(Input.GetKey(KeyCode.Space)){		//Spaceキーで主砲発射
			Fire();
		}
		if(Input.GetKey(KeyCode.Return)){		//Enterキーでカメラの方向に魚雷発射
			var camt = GameObject.Find("Main Camera").transform;
			GetComponent<TorpedoFireStarter>().Fire(camt.forward * 10f + transform.position);
		}
		if(Input.GetKey(KeyCode.X)){			//Xキーで後ろの砲門左回転
			SetGunRotationSpeed(1,1f);
		}else if(Input.GetKey(KeyCode.Z)){		//Zキーで後ろの砲門右回転
			SetGunRotationSpeed(1,-1f);
		}else{									//X,Z押されていないときは回転せず
			SetGunRotationSpeed(1,0f);
		}
		if(Input.GetKey(KeyCode.E)){			//Eキーで前の砲門左回転
			SetGunRotationSpeed(0,1f);
		}else if(Input.GetKey(KeyCode.Q)){		//Qキーで前の砲門右回転
			SetGunRotationSpeed(0,-1f);
		}else{									//E,Q押されていないときは回転せず
			SetGunRotationSpeed(0,0f);
		}
		Debug.Log(radar.TorpedosData.Count);
	}
}
