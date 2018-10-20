using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// このAIは原点中心の半径150の円を10角形に近似して移動しつつ一番近い敵に撃つ.
/// </summary>
public class AIShipController : ShipController {
	private int div = 10;
	private float angle;
	private int timeCount;

	protected override void onLoad(){
		angle = Random.Range(0f, 2 * Mathf.PI);
	}
	protected override void AIThink(){
		if(radar.OtherShipsData.Count > 0){				//他の船が1隻以上いるなら
			ShipData target = radar.OtherShipsData[0]; //適当な敵の船のデータを取得
			foreach(var s in radar.OtherShipsData){
				if(!s.Equals(target)){//sとtargetが等しくないなら
					if(Vector3.Distance(transform.position,s.position) < Vector3.Distance(transform.position, target.position)){//自分の船との距離比較
						target = s; //一番距離が近い敵に目標を設定
					}
				}
			}
			AutoGunRotateToTarget(target.position); // 砲門を目標に自動回転
			FireTorpedo(target.position);			// 目標地点に向けて魚雷発射
		}
		
		if(finishedAutoMove){	//自動移動が終了しているなら
			AutoMoveToTarget(new Vector3(Mathf.Sin(angle),0,Mathf.Cos(angle)) * 150f);	//円周移動
			angle+= Mathf.PI * 2 / div;
		}
		Fire();//砲門斉射
	}
}

