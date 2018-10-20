using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListUIToggle : MonoBehaviour {

	public GameObject scroll;

	void Update(){
		if(Input.GetKeyDown(KeyCode.Tab)){
			scroll.SetActive(!scroll.activeSelf);
		}
	}
}
