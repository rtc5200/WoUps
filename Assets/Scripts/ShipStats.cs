using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStats : MonoBehaviour {
	public int ID = 0;
	[SerializeField]
	public string shipName;
	[SerializeField]
	public int maxHP;
	private int hp;
	[SerializeField]
	public Color color;
	[SerializeField]
	public string author;
	[SerializeField]
	public Sprite icon;
	public int HP{
		get{
			return hp;
		}
		set{
			hp = Mathf.Clamp(value,0,int.MaxValue);
		}
	}

	void Awake(){
		HP = maxHP;
		if(color.Equals(new Color(0,0,0,0)))color = getRandomColor();
	}
	private Color getRandomColor(){
		float h = Random.Range(0f,1f), s =  Random.Range(0.5f,1f), v =  Random.Range(0.5f,1f);
		return Color.HSVToRGB(h,s,v);
	}

}
