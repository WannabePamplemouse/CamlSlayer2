﻿using UnityEngine;
using System.Collections;

public class RobotControllerScript : MonoBehaviour {

	private float maxSpeed = 10f;
	public bool facingRight = true;
	
	Animator anim;
	
	bool grounded = false;
	private Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

    [SerializeField]
	private float jumpForce = 700f;
    [SerializeField]
    private float dashSpeed, MaxDistanceSword;
    [SerializeField]
    private int dashEnergyCost, attackDamageSword, attackEnergyCost;
    [SerializeField]
    private int damageOnCollision = 0;
    [SerializeField]
    private Vector2 forceBigBullet, forcePoulet;

    private PlayerEnergy energy;
    private PlayerH playerHealth;
    private bool doDamageOnHit;
	
	float timer;

	public bool isWorld1finished = false;
	public bool isWorld2finished = false;
	public bool isWorld3finished = false;
	public bool isWorld4finished = false;

	public bool haveSword = true;
	public bool haveBomb = false;
	public bool haveTromblon = false;


	

	void Start () 
	{
        playerHealth = GetComponent<PlayerH>();
        energy = GetComponent<PlayerEnergy>();
		anim = GetComponent<Animator> ();
		groundCheck = transform.Find("GroundCheck");
	}
	
	
	void FixedUpdate () 
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("Ground", grounded);
		
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);
		
		
		float move = Input.GetAxis ("Horizontal");
		
		anim.SetFloat ("Speed", Mathf.Abs (move));
		
		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
		
		if (move > 0 && !facingRight)
			Flip ();
		else if (move < 0 && facingRight)
			Flip ();
	}
	
	void Update()
	{
        
		if (grounded && Input.GetKeyDown (KeyCode.Space)) 
		{
			grounded = false;
			anim.SetBool("Ground",false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
		}

		if(Input.GetKeyDown(KeyCode.B))
			SwitchBomb();

		if (isWorld1finished && Input.GetKeyDown (KeyCode.T))
			SwitchTromblon();

		if (Input.GetKeyDown (KeyCode.S))
			SwitchSword ();

		if (Input.GetKeyDown (KeyCode.Q)) 
		{
			timer = 0;
			anim.SetBool ("isAttacking", true);
		}
		
		timer += Time.deltaTime;
		
		if(timer > 0.50)
			anim.SetBool ("isAttacking", false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 19, !grounded || rigidbody2D.velocity.y > 0);
        
	}
	
	void Flip()
	{
		facingRight = !facingRight;
		
		Vector3 theScale = transform.localScale;	
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    private void attackSword()
    {
        if (energy.currentEnergy >= attackEnergyCost)
        {
            energy.UseEnergy(attackEnergyCost);

            timer = 1;

            RaycastHit2D hit;

            if (facingRight)
                hit = Physics2D.Raycast(transform.position, Vector2.right, MaxDistanceSword);
            else
                hit = Physics2D.Raycast(transform.position, -Vector2.right, MaxDistanceSword);


            if (hit.collider != null && hit.transform.gameObject.tag == "Enemy")
            {
                GameObject enemy = hit.transform.gameObject;
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(attackDamageSword);
            }
        }
    }

    IEnumerator dash(float dur)
    {
        if (energy.currentEnergy >= dashEnergyCost)
        {
            Physics2D.IgnoreLayerCollision(9, gameObject.layer);
            doDamageOnHit = true;
            playerHealth.canTakeDamage = false;
            damageOnCollision = 50;
            playerHealth.canDash = false;
            float time = 0;

            energy.UseEnergy(dashEnergyCost);

            float realDashSpeed;
            if (facingRight)
                realDashSpeed = dashSpeed;
            else
                realDashSpeed = -dashSpeed;

            while (time < dur)
            {
                time += Time.deltaTime;
                rigidbody2D.velocity = new Vector2(realDashSpeed, 0);
                yield return 0;
            }

            doDamageOnHit = false;
            playerHealth.canTakeDamage = true;
            playerHealth.canDash = true;
            damageOnCollision = 0;
            Physics2D.IgnoreLayerCollision(9, gameObject.layer, false);
        }
    }

    private void shootPoulet()
    {
        timer = 1f;
        GameObject spike = GameObject.FindGameObjectWithTag("Poulet");
        spike = (GameObject)Instantiate(spike, gameObject.transform.position, new Quaternion(0, 0, 0, 0));
        spike.GetComponent<AudioSource>().Play();
        if (facingRight)
        {
            spike.rigidbody2D.AddForce(forcePoulet);
        }
        else
        {
            spike.rigidbody2D.AddForce(new Vector2(-forcePoulet.x, forcePoulet.y));
        }
    }

    private void shootBigBullet()
    {
        timer = 0.1f;
        GameObject spike = GameObject.FindGameObjectWithTag("Boooom");
        spike = (GameObject)Instantiate(spike, gameObject.transform.position, new Quaternion(0, 0, 0, 0));
        if (facingRight)
        {
            spike.rigidbody2D.AddForce(forceBigBullet);
        }
        else
        {
            spike.rigidbody2D.AddForce(new Vector2(-forceBigBullet.x, forceBigBullet.y));
        }
    }

    public void SwitchBomb()
    {
        haveBomb = true;
        haveSword = false;
        haveTromblon = false;
        anim.SetBool("haveBomb", true);
    }

    public void SwitchTromblon()
    {
        haveTromblon = true;
        haveBomb = false;
        haveSword = false;
        anim.SetBool("haveTromblon", true);
    }

    public void SwitchSword()
    {
        haveSword = true;
        haveBomb = false;
        haveTromblon = false;
        anim.SetBool("haveSword", true);
    }
}
