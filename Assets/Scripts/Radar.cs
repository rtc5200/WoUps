using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour {
	/// <summary>
	/// 他の船のデータ(ShipsData)のList
	/// </summary>
	[HideInInspector]
	public List<ShipData> OtherShipsData;
	/// <summary>
	/// 砲弾のデータ
	/// </summary>
	[HideInInspector]
	public List<BulletData> BulletsData;
	/// <summary>
	/// 魚雷のデータ
	/// </summary>
	[HideInInspector]
	public List<TorpedoData> TorpedosData;

	public void UpdateData() {
		OtherShipsData = GameObject.Find("GameController").GetComponent<GameController>().getOtherShipsData(this);
		var bullets = GameObject.FindGameObjectsWithTag("Bullet");
		var bList = new List<BulletData>();
		if(bullets is GameObject[]){
			foreach( var b in bullets){
				bList.Add(new BulletData(b));
			}
		}
		BulletsData = bList;

		var torpedos = GameObject.FindGameObjectsWithTag("Torpedo");
		var tList = new List<TorpedoData>();
		if(torpedos is GameObject[]){
			foreach(var t in torpedos){
				tList.Add(new TorpedoData(t));
			}
		}
		TorpedosData = tList;
	}
}
/// <summary>
/// 船が取得可能な発射された魚雷のデータ
/// </summary>
public class TorpedoData : BulletData
{
	public TorpedoData(GameObject torpedo) : base(torpedo){}
}
/// <summary>
/// 船が取得可能な船のデータ
/// </summary>
public class ShipData{
	public readonly int id;
	public readonly int hp;
	public readonly Vector3 position;
	public readonly Vector3 forward;
	public readonly Quaternion quaternion;
	public ShipData(GameObject ship){
		ShipStats stats = ship.GetComponent<ShipStats>();
		id = stats.ID;
		hp = stats.HP;
		position = ship.transform.position;
		forward = ship.transform.forward;
		quaternion = ship.transform.rotation;
	}
}
/// <summary>
/// 船が取得可能な発射された砲弾のデータ
/// </summary>
public class BulletData
{
	public readonly Vector3 position;
	public readonly Vector3 forward;
	public readonly Quaternion quaternion;
	public readonly int shipid;

	public BulletData(GameObject bullet){
		this.position = bullet.transform.position;
		this.forward = bullet.transform.forward;
		this.quaternion = bullet.transform.rotation;
		this.shipid = bullet.GetComponent<Mover>().parentShip.GetComponent<ShipStats>().ID;
	}
}
