using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour {

	public GameObject playerlifebar;
	public static Singleton instance;
	public float lifePlayer;
	// Use this for initialization
	void Start () {
		playerlifebar = GameObject.Find("playerlifebar"); 
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void LifeBar (){
		playerlifebar.GetComponent<Image> ().fillAmount = (HandleBar (playerController.life, lifePlayer, 1));
	}
	public float HandleBar(float value, float maxValue, float fillMax){
		float resultLife = (value) * (fillMax) / (maxValue);
		return resultLife;
	}
	public void Changelvl(int lvl){
		SceneManager.LoadScene (lvl);
	}

	public void nextlvl(){
		int lvl = SceneManager.GetActiveScene ().buildIndex;
		if(lvl < SceneManager.sceneCountInBuildSettings){
			SceneManager.LoadScene (lvl+1);
		}
	}
}
