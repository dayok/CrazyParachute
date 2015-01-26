﻿using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	int score = 0;
	int highscore = 0;
	int life = 3;
	float time = 60f;
	float timeToTransformation = 2.0f;
	float timeJetack = 10f;
	bool isJetpack;
	float timeBlinking = 0;
	private float fingerStartTime  = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	
	private bool isSwipe = false;
	private float minSwipeDist  = 50.0f;
	private float maxSwipeTime = 0.5f;
	
	// The force which is added when the player jumps
	// This can be changed in the Inspector window
	public Vector2 left = new Vector2(-150, 0);
	public Vector2 right = new Vector2(150, 0);
	public Vector2 up = new Vector2(0, 20);
	public Vector2 down = new Vector2(0, -150);
	GameObject player;
	int rotation = 0;
	public Sprite[] playerSpritesParachute;
	public Sprite[] playerSpritesJP;
	public Sprite[] playerSpritesSki;
	
	GUIStyle largeFont;


	void Start()
	{
		score = PlayerPrefs.GetInt("score");
		isJetpack = false;
		HideChildren ();
		HideExplode ();
		player = GameObject.FindGameObjectWithTag("Player");
		largeFont = new GUIStyle();

		largeFont.fontSize = 30;
		largeFont.normal.textColor = Color.red;

		if(Application.loadedLevel == 5){
			playerSpritesJP = Resources.LoadAll<Sprite>("jetpack_man");
			foreach(Sprite s in playerSpritesJP)
			if (s.name.Equals("jetpack_man")){
				player.GetComponent<SpriteRenderer>().sprite = s;
				break;
			}
			ShowChildren();
			left = new Vector2(-100, 0);
			right = new Vector2(100, 0);
			up = new Vector2(0, 100);
			down = new Vector2(0, -200);
		}
	}

	void Update ()
	{
		Vector2 viewPos = Camera.main.WorldToViewportPoint(transform.position);

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if ((Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown(0))) 
		{
			if (Physics.Raycast(ray, out hit)) {
				if (hit.transform.tag == "arrow_bottom" ){
					MoveDown();
				}

				if (hit.transform.tag == "arrow_top" ){
					MoveUp();
				}

				if (hit.transform.tag == "arrow_left" ){
					MoveLeft();
				}

				if (hit.transform.tag == "arrow_right" ){
					MoveRight();
				}

			}
		}

		//make screen into ray poin	t
//		Ray touchPos = Camera.main.ScreenPointToRay(Input.mousePosition);
//		float speed = 0;
//		if (isJetpack){
//			speed = 1.5f;
//		}
//		else{
//			speed = 0.7f;
//		}
//
//		if(Input.touchCount>0 || Input.GetMouseButton (0)) {
//			rigidbody2D.transform.Translate(touchPos.origin.x * speed * Time.deltaTime, touchPos.origin.y * speed * Time.deltaTime, 0);  
//		}


		// Left
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			MoveLeft();
		
		}

		//Right
		if (Input.GetKey(KeyCode.RightArrow))
		{
			MoveRight();
		
		}

		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			MoveUp();
		}

		if (Input.GetKey (KeyCode.DownArrow)) 
		{
			MoveDown();
		}



//		if (Input.touchCount > 0){
//			foreach (Touch touch in Input.touches)
//			{
//				switch (touch.phase)
//				{
//				case TouchPhase.Began :
//					/* this is a new touch */
//					isSwipe = true;
//					fingerStartTime = Time.time;
//					fingerStartPos = touch.position;
//					break;
//					
//				case TouchPhase.Canceled :
//					/* The touch is being canceled */
//					isSwipe = false;
//					break;
//					
//				case TouchPhase.Ended :
//					float gestureTime = Time.time - fingerStartTime;
//					float gestureDist = (touch.position - fingerStartPos).magnitude;
//					
//					if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist){
//						Vector2 direction = touch.position - fingerStartPos;
//						Vector2 swipeType = Vector2.zero;
//						
//						if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
//							// the swipe is horizontal:
//							swipeType = Vector2.right * Mathf.Sign(direction.x);
//						}else{
//							// the swipe is vertical:
//							swipeType = Vector2.up * Mathf.Sign(direction.y);
//						}
//						if(swipeType.x != 0.0f){
//							if(swipeType.x > 0.0f){
//								rigidbody2D.velocity = Vector2.zero;
//								rigidbody2D.AddForce(right);
//								if(rotation < 0 ){
//									transform.Rotate(0, 0, -10.0f);
//									rotation += 10;
//								}
//								else if(rotation == 0){
//									transform.Rotate(0, 0, -5.0f);
//									rotation += 5;
//								}
//
//							}else{
//								rigidbody2D.velocity = Vector2.zero;
//								rigidbody2D.AddForce(left);
//								if(rotation > 0){
//									transform.Rotate(0, 0, 10.0f);
//									rotation -= 10;
//								}
//								else if(rotation == 0){
//									transform.Rotate(0, 0, 5.0f);
//									rotation -= 5;
//								}
//							}
//						}
//						if(swipeType.y != 0.0f ){
//							if(swipeType.y > 0.0f){
//								rigidbody2D.AddForce(up);
//							}else{
//								rigidbody2D.AddForce(down);
//							}
//						}
//					}
//					
//					break;
//				}
//			}
//		}

		if(Application.loadedLevel == 5){
			if(transform.position.y > 8 || transform.position.y < -8 || transform.position.x > 7 || transform.position.x < -7){
				Die ();
			}
		}
		else if(Application.loadedLevel == 9){
			if(transform.position.y > 8 || transform.position.y < -8 || transform.position.x > 6 || transform.position.x < -6){
					transform.position = new Vector2(0.0f, 6.0f);
			}
		}
		else{
			if(transform.position.y > 7)
			{
				transform.position = new Vector2(transform.position.x, -6);
			}
			if(transform.position.y < -7)
			{
				transform.position = new Vector2(transform.position.x, 6);
			}
			if (transform.position.x > 6)
			{
				transform.position = new Vector2(-5, transform.position.y);
			}
			if (transform.position.x < -6) 
			{
				transform.position = new Vector2(5, transform.position.y);
			}
		}




		time -= Time.deltaTime;
		if ( time < 0 )
		{
			Application.LoadLevel(Application.loadedLevel + 1);
			PlayerPrefs.SetInt("score",score);
		}
		if(Application.loadedLevel != 5){
		if (isJetpack) {
			left = new Vector2(-250, 0);
			right = new Vector2(250, 0);
			up = new Vector2(0, 250);
			down = new Vector2(0, -250);
						timeJetack -= Time.deltaTime;
						if (timeJetack < 0) {
								isJetpack = false;
								HideChildren ();
								HideExplode();
								timeJetack = 0;
								foreach (Sprite s in playerSpritesParachute)
										if (s.name.Equals ("player")) {
												player.GetComponent<SpriteRenderer> ().sprite = s;
												player.GetComponent<SpriteRenderer>().sprite = s;
												Destroy(GetComponent<PolygonCollider2D>());
												player.AddComponent<PolygonCollider2D>();
												break;
										}
								//rigidbody2D.velocity = Vector2.zero;

						}
				} else {
			up = new Vector2(0, 20);
			left = new Vector2(-150, 0);
			right = new Vector2(150, 0);
			down = new Vector2(0, -150);
				}
		}

		timeBlinking -= Time.deltaTime;
		if (timeBlinking <= 0) {
			CancelInvoke();
		}

		if (Application.loadedLevel == 9){
			timeToTransformation -= Time.deltaTime;
			if ( timeToTransformation < 0 )
			{
				playerSpritesSki = Resources.LoadAll<Sprite>("ski_man");
				foreach(Sprite s in playerSpritesSki)
				if (s.name.Equals("ski_man")){
					player.GetComponent<SpriteRenderer>().sprite = s;
					Destroy(GetComponent<PolygonCollider2D>());
					player.AddComponent<PolygonCollider2D>();
					break;
				}
			}

			float maxSpeed = 3.0f;
			if(rigidbody2D.velocity.magnitude > maxSpeed)
			{
				rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxSpeed;
			}

			if(transform.rotation.z >= 80 &&  transform.rotation.z <= 330){
				Die();
			}
		}

	}

	
	void Die()
	{
		if (life > 0){
			life--;
			transform.position = new Vector2(0.0f, 5.0f);
			timeBlinking = 3;
			InvokeRepeating("Blinking", 0.0f, 0.2f);
		}
		else{
			if (score > highscore){
				highscore = score;
				PlayerPrefs.SetInt("highscore", highscore);
			}
			PlayerPrefs.SetInt("score", 0);
			Application.LoadLevel(0);
		}
	}
	

	void OnGUI () 
	{
		GUILayout.Label(" Score: " + score.ToString(), largeFont);
		GUILayout.Label(" Time: " + time.ToString("0"), largeFont);
		GUILayout.Label(" Life: " + life.ToString(), largeFont);
		if(Application.loadedLevel != 5){
			GUILayout.Label(" Jetpack: " + timeJetack.ToString("0"), largeFont);
		}
	}


	void Awake()
	{
		// load all frames in fruitsSprites array
		playerSpritesJP = Resources.LoadAll<Sprite>("jetpack_man");
		playerSpritesParachute = Resources.LoadAll<Sprite>("player");
		playerSpritesSki = Resources.LoadAll<Sprite>("ski_man");
	}
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "coin") { 
						score += 10; 
						Destroy (other.gameObject); 
		} 
		else if (other.tag == "jetpack") {
			playerSpritesJP = Resources.LoadAll<Sprite>("jetpack_man");
			foreach(Sprite s in playerSpritesJP)
			if (s.name.Equals("jetpack_man")){
				player.GetComponent<SpriteRenderer>().sprite = s;
				player.GetComponent<SpriteRenderer>().sprite = s;
				Destroy(GetComponent<PolygonCollider2D>());
				player.AddComponent<PolygonCollider2D>();
				break;
			}
			Destroy (other.gameObject); 
			isJetpack = true;
			timeJetack = 10f;
			ShowChildren();
		}
		else if (other.tag == "life"){
			life++;
			Destroy (other.gameObject);
		}
		else if (other.tag == "wall"){
			if(timeBlinking<=0)
			{
				Die ();
			}
		}
		else {
			if(!isJetpack){
				if(timeBlinking<=0)
				{
					Die ();
				}
			}
			else
			{
				score += 10; 
				Destroy (other.gameObject);
				InvokeRepeating("Explosion", 0.0f, 1.0f);
			}
		}
	}

	void HideChildren()
	{
		Renderer[] lChildRenderers=gameObject.GetComponentsInChildren<Renderer>();
		foreach ( Renderer lRenderer in lChildRenderers)
		{
			if(lRenderer.tag == "jetpackFlamesTag"){
				lRenderer.enabled=false;
			}
		}
	}

	void ShowChildren()
	{
		Renderer[] lChildRenderers=gameObject.GetComponentsInChildren<Renderer>();
		foreach ( Renderer lRenderer in lChildRenderers)
		{
			if(lRenderer.tag == "jetpackFlamesTag"){
				lRenderer.enabled=true;
			}
		}
	}

	void Explode()
	{
		Renderer[] lChildRenderers=gameObject.GetComponentsInChildren<Renderer>();
		foreach ( Renderer lRenderer in lChildRenderers)
		{
			if(lRenderer.tag == "explode"){
				lRenderer.enabled=true;
			}
		}
	}

	void HideExplode()
	{
		Renderer[] lChildRenderers=gameObject.GetComponentsInChildren<Renderer>();
		foreach ( Renderer lRenderer in lChildRenderers)
		{
			if(lRenderer.tag == "explode"){
				lRenderer.enabled=false;
			}
		}
		//renderer.enabled = true;
	}

	IEnumerator Wait(float seconds)
	{
		transform.renderer.enabled = false;
		yield return new WaitForSeconds(seconds); 
		transform.renderer.enabled = true;
	}

	IEnumerator WaitForExplode() {
		Explode ();
		yield return new WaitForSeconds(0.3f);
		HideExplode ();
	}

	void Blinking()
	{
		StartCoroutine( Wait (.1f));
	}

	void Explosion()
	{
		StartCoroutine( WaitForExplode ());
	}
	

	private void MoveLeft()
	{
		if (Application.loadedLevel != 9){
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.AddForce(left);
			if(rotation > 0 ){
				transform.Rotate(0, 0, 10.0f);
				rotation -= 10;
			}
			else if(rotation == 0){
				transform.Rotate(0, 0, 2.0f);
				rotation -= 5;
			}
		}
		else{
			transform.Rotate(0, 0, 5.0f);
			left = new Vector2(-20,0);
			rigidbody2D.AddForce(left);
		}
	}

	private void MoveRight()
	{
		if (Application.loadedLevel != 9){
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.AddForce(right);
			if(rotation < 0 ){
				transform.Rotate(0, 0, -10.0f);
				rotation += 10;
			}
			else if(rotation == 0){
				transform.Rotate(0, 0, -2.0f);
				rotation += 5;
			}
		}
		else{
			transform.Rotate(0, 0, -5.0f);
			right = new Vector2(20,0);
			rigidbody2D.AddForce(right);
		}
	}

	private void MoveUp()
	{
		if (Application.loadedLevel != 9){
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.AddForce(up);
		}
		else{
			up = new Vector2(0,1000);
			//rigidbody2D.velocity = new Vector2(0,50);
			rigidbody2D.AddForce(up, ForceMode2D.Impulse);
		}
	}

	private void MoveDown()
	{
		if (Application.loadedLevel != 9){
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.AddForce(down);
		}
		else{
			rigidbody2D.AddForce(down);
		}
	}

}