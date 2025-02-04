﻿using UnityEngine;
using System.Collections;

public class Unicorn : MonoBehaviour {

    [SerializeField]
    private int value;

    private bool destroyable;
    [SerializeField]
    public GameObject PartExpl;
    [SerializeField]
    public GameObject PartExpl2;

    private AudioSource[] audio;
    private AudioSource uuhuhuhu, boooooom;

    // Use this for initialization
    void Start()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 10);
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 17);

        audio = GetComponents<AudioSource>();

        if(audio[0].priority == 255)
        {
            uuhuhuhu = audio[0];
            boooooom = audio[1];
        }
        else
        {
            uuhuhuhu = audio[1];
            boooooom = audio[0];
        }

        uuhuhuhu.Play();

            if (GameObject.FindGameObjectsWithTag("Unicorn").Length > 1)
            {
                gameObject.tag = "Unicorned";
                destroyable = true;
            }
            else
            {
                destroyable = false;
            }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.GetComponent<EnemyHealth>().TakeDamage(value);
        }
        
        if(destroyable)
        {
            gameObject.collider2D.enabled = false;
            gameObject.rigidbody2D.gravityScale = 0;
            gameObject.rigidbody2D.velocity = new Vector2(0, 0);
            gameObject.rigidbody2D.fixedAngle = true;
            boooooom.Play();
            PartExpl.SetActive(true);
            PartExpl2.SetActive(true);
            renderer.enabled = false;
            Destroy(gameObject, 2);
        }
    }
}
