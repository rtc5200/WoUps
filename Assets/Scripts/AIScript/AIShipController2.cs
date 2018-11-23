using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipController2 : ShipController {

	private ShipData target;//目標の船のデータ保存用変数

	protected override void AIThink(){
		if(target == null || target.hp == 0){//目標が設定されていない,または目標のHPが0
			float dif = Mathf.Infinity;//距離の２乗保存用変数
			foreach(ShipData s in radar.OtherShipsData){//他の船のデータ１つ１つに対して処理
				if((s.position - transform.position).sqrMagnitude < dif){
					//自分の位置から敵の船の位置へのベクトルの長さの２乗を比較
					target = s;//目標設定
				}
			}
		}
		AutoMoveToTarget(target.position);//目標の地点へ自動移動開始
		AutoGunRotateToTarget(target.position);//目標へ砲門自動回転
		Fire();//撃つ
		FireTorpedo(target.position);//魚雷も撃つ
	}
}
