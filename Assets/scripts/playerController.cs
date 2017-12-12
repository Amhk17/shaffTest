using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;
	public LayerMask groundLayer;
	private Transform groundDetector;
	private Animator animator;

	public float speedMove;
	public float speedJump;
	public static float life;

	public bool isGrounded;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		groundDetector = transform.GetChild (0);
		speedMove = 6f;
		speedJump = 16f;
	}
	
	// Update is called once per frame
	void Update () {
		SpacialStatus ();
	}

	void FixedUpdate() {
		Move (speedMove);
	}

	private void Move(float velocity){
		float horizontalAxis = Input.GetAxisRaw ("Horizontal");
		rigidBody.position += new Vector2(horizontalAxis*velocity*Time.fixedDeltaTime, 0);
		HorizontalAnimations (horizontalAxis);
	}

	private void SpacialStatus (){
		isGrounded = Physics2D.OverlapPoint (groundDetector.transform.position, groundLayer);
		if(Input.GetButtonDown("Jump") && isGrounded){
			JumpAnimations ();
			JumpCalculator (speedJump);
		}

		if (!isGrounded) {
			OnAir ();
		}
	}	

	private void OnAir (){

	}

	private float JumpCalculator(float jumpForce){
		float jumpMultiplier = 10;
		rigidBody.velocity = new Vector2 (0, jumpForce);
		return jumpForce * jumpMultiplier;
	}

	private void JumpAnimations(){
		
	}

	private void HorizontalAnimations(float horizontalAxis){
		if(horizontalAxis>0){
			spriteRenderer.flipX = false;
		}
		if(horizontalAxis<0){
			spriteRenderer.flipX = true;
		}
		if(horizontalAxis==0){

		}
	}

	void OnCollisionEnter2D(Collision2D coll) {

	}

	void OnTriggerEnter2D(Collider2D other) {

	}
}
