using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

	private bool isGroundedL;
	private bool isGroundedR;
	private Transform groundedDetector;
	public LayerMask groundLayer;
	private Rigidbody2D rB;
	public float jumpSpeed;
	private SpriteRenderer sPR;
	private Animator anim;
	public float speed;
	private int life;
	private float myTime;
	private bool walking;
	private Color blue;
	private Text time;
	public Sprite[] lifeSprite;
	public GameObject lifebar;
	public GameObject _win;
	public GameObject _gameOver;
	public GameObject _bossFight;
	public GameObject map;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		groundedDetector = transform.GetChild (0);
		rB = GetComponent<Rigidbody2D> ();
		sPR = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		time = GameObject.Find("time").GetComponent<Text> ();
		speed = 1f;
		life = 3;
		myTime = 100;
		//map.SetActive (true);
		//_bossFight.SetActive (false);
	}

	void Update () {
		myTime -= Time.deltaTime;
		time.text ="Timeleft: "+myTime.ToString("F1");
		if (myTime <=0) {
			_gameOver.SetActive (true);
			Time.timeScale = 0;
		}

		isGroundedL = Physics2D.OverlapPoint (transform.GetChild(0).transform.position, groundLayer);
		isGroundedR = Physics2D.OverlapPoint (transform.GetChild(1).transform.position, groundLayer);
		if(Input.GetKeyDown(KeyCode.Space)&& (isGroundedL || isGroundedR)){
			Jump (jumpSpeed);
		}
		if(Input.GetKey(KeyCode.Space)){
			anim.SetBool ("jump", true);
		}

		if (isGroundedL || isGroundedR) {
			anim.SetBool ("jump", false);

		}
		if (Input.GetKeyDown (KeyCode.LeftShift) && (isGroundedL || isGroundedR)) {
			Invoke ("MoreSpeed", 0.35f);
		}
		if (Input.GetKeyUp (KeyCode.LeftShift) && (isGroundedL || isGroundedR)) {
			speed = 1;
			Invoke ("LessSpeed", 0.35f);
		}
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			speed = 1;
			Invoke ("LessSpeed", 0.35f);
		} 
		lifebar.transform.localScale = new Vector2 (0.57f,0.6f); 
		if (life == 3) {
			lifebar.GetComponent<SpriteRenderer>().sprite = lifeSprite [0];
		} if (life == 2) {
			lifebar.GetComponent<SpriteRenderer>().sprite = lifeSprite [1];
		} if (life == 1) {
			lifebar.GetComponent<SpriteRenderer>().sprite = lifeSprite [2];
		} if (life == 0) {
			lifebar.GetComponent<SpriteRenderer>().sprite = lifeSprite [3];
			_gameOver.SetActive (true);
			Time.timeScale = 0;
		}
	}
	void FixedUpdate(){
		Move(speed);
	}

	void Move(float velocity){
		float h = Input.GetAxisRaw ("Horizontal");
		rB.position += new Vector2 (h*velocity*Time.fixedDeltaTime,0);
		if(h>0){
			walking = true;
			if(walking == true){
				sPR.flipX = false;
			}
			anim.SetBool ("isWalking", true);
			anim.SetBool ("damaged", false);
		}
		if(h<0){
			walking = true;
			if(walking == true){
				sPR.flipX = true;
			}
			anim.SetBool ("isWalking", true);
			anim.SetBool ("damaged", false);
		}
		if(h==0){
			walking = false;
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isWalking", false);
			anim.SetBool ("damaged", false);
		}
	}

	float Jump(float jumpForce){
		rB.velocity = new Vector2 (0,jumpForce);
		return jumpForce*2;
	}

	public void MoreSpeed(){
		anim.SetBool ("isRunning", true);
		speed = speed*2;
	}

	public void LessSpeed(){
		anim.SetBool ("isRunning", false);
		speed = 1;
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "water") {
			blue = new Color (0, 255, 255, 255);
			sPR.color = blue;
		} else {
			sPR.color = new Color (255, 255, 255, 255);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "bola") {
			life--;
			anim.SetBool ("damaged", true);
			gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 
				(transform.position.x * -0.5f, transform.position.y), ForceMode2D.Impulse);
		}
		if (coll.gameObject.tag== "enemy") {
			life--;
			gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 
				(transform.position.x * -0.5f, transform.position.y), ForceMode2D.Impulse);
			anim.SetBool ("damaged", true);
		}
		if (coll.gameObject.tag== "finish") {
			_win.SetActive (true);
			Time.timeScale = 0;
		} 
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.name== "door") {
			if(Input.GetKeyUp (KeyCode.Space)){
				_bossFight.SetActive (true);
				map.SetActive (false);
			}
		} 
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name== "coll") {
			life--;
			gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 
				(transform.position.x * -0.5f, transform.position.y), ForceMode2D.Impulse);
			anim.SetBool ("damaged", true);
		} 
	}
}
