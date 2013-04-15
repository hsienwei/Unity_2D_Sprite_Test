using UnityEngine;
using UnityEditor;
using System.Collections;

public class MenuTest : MonoBehaviour {
	
	 [MenuItem ("MyMenu/Do Something")]
	static  void a()
	{
		Debug.Log("menu test");
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
