using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbedBehavior : MonoBehaviour {
    Rigidbody rigidB;
    [SerializeField]
    AudioSource impactSound;
	// Use this for initialization
	void Start () {
        rigidB = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider coll)
    {
        Embed();
        transform.parent = coll.transform.parent;
    }
    void Embed()
    {
       GetComponent<Collider>().enabled = false;
        impactSound.Play();
        transform.GetComponent<ProjectileAddForce>().enabled = false;
        rigidB.velocity = Vector3.zero;
        rigidB.useGravity = false;
       rigidB.isKinematic = true;
    }
}
