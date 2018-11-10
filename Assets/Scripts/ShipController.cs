using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipController : MonoBehaviour {
	private Rigidbody rb;
	private GunManager guns;
	private float speed;
	private float rotation_speed;
	private float StartTimeAutoRot;

	protected Radar radar;
	
	
	/// <summary>
	/// 自動移動目標地点座標
	/// </summary>
	protected Vector3 AutoMoveTargetPos;
	/// <summary>
	/// 自動旋回目標地点座標
	/// </summary>
	protected Vector3 AutoRotTargetPos;

	/// <summary>
	/// 自動移動が終了する/した時true
	/// </summary>
	protected bool finishedAutoMove{get;private set;} = true;
	/// <summary>
	/// 自動旋回が終了する/した時true
	/// </summary>
	protected bool finishedAutoRot{get;private set;} = true;


	/// <summary>
	/// 船の移動スピード(-1 ~ +1). 1で1/call
	/// </summary>
	/// <value></value>
	protected float Speed{
		get{
			return speed;
		}
		set{
			var clamped = Mathf.Clamp(value,-1f,1f);
			if(clamped != value)Debug.LogError("Speed Value out of Range!!");
			speed = clamped;
		}
	}

	/// <summary>
	/// 船の回転スピード(-1～+1). -1で1度/call左回転,+1で1度/call右回転.
	/// </summary>
	/// <value></value>
	protected float RoSpeed{
		get{
			return rotation_speed;
		}
		set{
			var clamped = Mathf.Clamp(value,-1f,1f);
			if(clamped != value)Debug.LogError("RoSpeed Value out of Range!!");
			rotation_speed = clamped;
		}
	}
	
	/// <summary>
	/// AIThink関数が何回目の実行であるか
	/// </summary>
	protected int AIThinkCount{get;private set;} = 0;

	void Awake()=>rb = GetComponent<Rigidbody>();	

	void Start (){
		guns = GetComponent<GunManager>();
		radar = GetComponent<Radar>();
		onLoad();
	}
	
    void FixedUpdate () {
		if(Time.fixedTime > 1f){
			GetComponent<Radar>().UpdateData();
			AIThinkCount++;
			AIThink();
			rotate();
			move();
			guns.Rotate(0);
			guns.Rotate(1);
		}
	}

	private void move(){
		if(!finishedAutoRot){
			Speed = 0f;
		}else if(!finishedAutoMove){
			if(Vector3.Distance(transform.position, AutoMoveTargetPos) < 2f){
				finishedAutoMove = true;
				Speed = 0f;
			}else{
				var diff = AutoRotTargetPos - transform.position;
				var angle = Vector3.Angle(transform.forward, diff);
				if(angle < 1f){
					Speed = 1f;
				}else{
					finishedAutoRot = false;
					StartTimeAutoRot = Time.time;
					Speed = 0f;
				}
			}
		}
		rb.velocity = transform.forward  * speed * 50f; //speed 1f = 1 / call
		//transform.position += transform.forward * speed * Time.fixedDeltaTime * 50f;
	}
	private void rotate(){
		if(!finishedAutoRot){
			var diff = AutoRotTargetPos - transform.position;
			var angle = Vector3.Angle(transform.forward, diff);
			if(angle < 0.5f || Time.time - StartTimeAutoRot > 10f){
				RoSpeed = 0f;
				finishedAutoRot = true;
			}else if(angle < 1f){
				RoSpeed = Vector3.Cross(transform.forward,diff).y < 0 ? -(angle-0.5f): angle-0.5f;
			}else{
				RoSpeed = Vector3.Cross(transform.forward,diff).y < 0 ? -1f: 1f;
			}
		}
		rb.velocity = Vector3.zero;
		rb.MoveRotation(
			rb.rotation * 
			Quaternion.Euler(0,rotation_speed * 50f * Time.fixedDeltaTime,0) 
		); // speed 1f = 1 degree / call
	}
	/// <summary>
	/// 砲門の回転スピードを設定
	/// </summary>
	/// <param name="index">砲門のindex値(0(前方),1(後方))</param>
	/// <param name="speed">回転スピード(-1度～1度)</param>
	protected void SetGunRotationSpeed(int index,float speed)=>guns.SetRotationSpeed(index,speed);

	/// <summary>
	/// 主砲発射.クールタイム中の場合発射せず終了.
	/// </summary>
	protected void Fire()=>guns.Fire();
	/// <summary>
	/// 主砲発射.クールタイム中の場合発射せず終了.
	/// </summary>
	/// <param name="index">砲門番号(0or1)</param>
	protected void Fire(int index)=>guns.Fire(index);
	/// <summary>
	/// 魚雷発射. クールタイム中は発射せず終了.
	/// </summary>
	/// <param name="pos">魚雷が向かう座標</param>
	protected void FireTorpedo(Vector3 pos)=>GetComponent<TorpedoFireStarter>().Fire(pos);

	/// <summary>
	/// 主砲の残りクールタイムを取得
	/// </summary>
	/// <param name="index">砲門番号(0or1)</param>
	/// <returns>残りクールタイム</returns>
	protected float getGunCoolTimeLeft(int index) => guns.getCoolTimeLeft(index);
	/// <summary>
	/// 魚雷の残りクールタイムを取得
	/// </summary>
	/// <returns>残りクールタイム</returns>
	protected float getTorpedoCoolTimeLeft() => GetComponent<TorpedoFireStarter>().coolTimeLeft;

	/// <summary>
	/// 目標地点を設定し,自動旋回&移動. タイムアウトなし.
	/// </summary>
	/// <param name="pos">移動目標地点の座標</param>
	protected void AutoMoveToTarget(Vector3 pos){
		if(finishedAutoMove && finishedAutoRot){
			AutoRotateToTarget(pos);
			AutoMoveTargetPos = pos;
			finishedAutoMove = false;
		}
		
	}
	/// <summary>
	/// 目標地点を設定し,その方向に自動旋回.自動移動はこっちも実行される.タイムアウト10秒.
	/// </summary>
	/// <param name="pos">旋回目標地点の座標</param>
	protected void AutoRotateToTarget(Vector3 pos){
		if(finishedAutoRot){
			AutoRotTargetPos = pos;
			finishedAutoRot = false;
			StartTimeAutoRot = Time.time;
		}
	}
	/// <summary>
	/// 指定角度自動回転.タイムアウト10秒.
	/// </summary>
	/// <param name="angle">角度(右回転なら正,左回転なら負)</param>
	protected void AutoRotateToTarget(float angle){
		Vector3 direction = Quaternion.Euler(0f,-angle,0f) * transform.forward;
		Vector3 target = transform.position + direction.normalized;
		AutoRotateToTarget(target);
	}
	/// <summary>
	/// 目標地点を設定し,その方向に砲門を自動旋回.
	/// </summary>
	/// <param name="pos">砲門旋回目標地点の座標</param>
	protected void AutoGunRotateToTarget(Vector3 pos)=> guns.StartAutoCtrl(pos);
	/// <summary>
	/// 砲門の自動旋回を停止.
	/// </summary>
	protected void StopAutoGunRotation()=>guns.StopAutoCtrl();

	/// <summary>
	/// 今の砲門の角度で撃てるかどうか
	/// </summary>
	/// <param name="index">砲門のindex(0,1)</param>
	/// <returns>true: 撃てる false: 撃てない</returns>
	protected bool isFireable(int index) => guns.isFireable(index);
	/// <summary>
	/// 砲門のtransformを取得
	/// </summary>
	/// <param name="index">砲門のindex(0or1)</param>
	/// <returns>砲門のtransform</returns>
	/// <seealso>UnityEngine.Transform</seealso>
	protected Transform getGunTransform(int index) => guns.getTransform(index);
	/// <summary>
	/// 船に割当てられた固有のIDを取得
	/// </summary>
	/// <returns>ID</returns>
	protected int getID() => GetComponent<ShipStats>().ID;
	/// <summary>
	/// 船のHPを取得
	/// </summary>
	/// <returns>HP</returns>
	protected int getHP() => GetComponent<ShipStats>().HP;
	/// <summary>
	/// ゲーム開始からの経過時間を取得
	/// </summary>
	/// <returns>経過時間</returns>
	protected float getTime() => GameObject.Find("GameController").GetComponent<GameController>().gameStartedTime;

	/// <summary>
	/// ゲーム開始前に一度だけ実行されるメソッド. 要オーバーライド
	/// </summary>
	protected virtual void onLoad(){}

	/// <summary>
	/// AI思考メソッド. これをオーバーライド. 1秒間に50回実行される.
	/// </summary>
	protected virtual void AIThink(){
		//do nothing
	}
}
