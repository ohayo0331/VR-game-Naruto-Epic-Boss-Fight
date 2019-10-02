// http://luminaryapps.com/blog/arcing-projectiles-in-unity/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSphere : MonoBehaviour {
    
    public GameObject sphere;
    public GameObject smoke;
    private bool inAir;
    private Vector3 prevPos;
    private float speed;
    public GameObject leftHand;
    private Vector3 prevLeftHandPos;
    private float sphereScale = 1.0f;
    public AudioSource Rasen_S;
    public AudioClip raseb_START;
    public AudioClip raseb_rub;
    public AudioClip raseb_exp;

    private int prev_x, prev_z;

    private ParticleSystem ps_sphere;
    private ParticleSystem ps_smoke;

    // Use this for initialization
    void Start () {

        ps_sphere = sphere.GetComponent<ParticleSystem>();
        ps_smoke = smoke.GetComponent<ParticleSystem>();

        sphere.SetActive(false);
        inAir = true;
        Physics.gravity = new Vector3(0, -3f, 0);
        prevPos = this.transform.position;
        speed = 500.0f;

        sphereScale = 0.1f;
        sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
        prev_x = 0;
        prev_z = 0;
        Queue<Vector3> trajectory = new Queue<Vector3>();
    }
	
	// Update is called once per frame
	void Update () {
        //float i = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
        //Debug.Log(i);

        if (inAir)
        {
            // nothing here
        }
        else
        {
            // gesture
            if(Vector3.Dot((leftHand.transform.position - this.transform.position), this.transform.up) > 0)
            {
                // left hand is above right hand2
                int pos_x = Math.Sign(Vector3.Dot((leftHand.transform.position - this.transform.position), this.transform.right));
                int pos_z = Math.Sign(Vector3.Dot((leftHand.transform.position - this.transform.position), this.transform.forward));

                if(prev_x * pos_x < 0 || prev_z * pos_z < 0 && sphereScale < 3)
                {
                    sphereScale += 0.1f;
                    var smoke_emission = ps_smoke.emission;
                    smoke_emission.rateOverTime = sphereScale * 3 + 1;

                    var sphere_emission = ps_sphere.emission;
                    sphere_emission.rateOverTime = sphereScale * 2 + 1;
                }
                if (sphereScale >= 3)
                {
                    // display text "Ready"
                }
                prev_x = pos_x;
                prev_z = pos_z;


            }
        }
        //Debug.Log(inAir);
        //Debug.Log(OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger));
        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) == 1.0f && inAir)
        {
            //Debug.Log("Sphere created");
            ps_sphere.Play();
            inAir = false;
            Rasen_S.clip = raseb_START;
            Rasen_S.Play();
            Rasen_S.loop = true;
            sphere.transform.SetParent(this.transform);
            sphere.transform.localPosition = new Vector3(-0.073f, -0.065f, -0.026f);
            sphere.GetComponent<Rigidbody>().useGravity = false;
            sphere.GetComponent<Rigidbody>().isKinematic = true;
            
            var smoke_emission = ps_smoke.emission;
            smoke_emission.rateOverTime = 0;

            var sphere_emission = ps_sphere.emission;
            sphere_emission.rateOverTime = 0;

            sphere.SetActive(true);
            sphereScale = 1;

        }
        else if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) <0.99f && !inAir)
        {
            //Debug.Log("Sphere leaves hand");
            inAir = true;
            //startPos = transform.position;
            if(sphere.transform.parent != null)
                sphere.transform.parent = null;
            Rasen_S.clip = raseb_rub;
            Rasen_S.Play();
            Rasen_S.loop = false;
            sphere.GetComponent<Rigidbody>().useGravity = true;
            sphere.GetComponent<Rigidbody>().isKinematic = false;
            //sphere.transform.position = updateVelocity(sphere.transform.position);
            Vector3 dir = sphere.transform.position - prevPos;
            sphere.GetComponent<Rigidbody>().AddForce(dir * speed, ForceMode.Impulse);
            Debug.Log(dir);
            //sphere.GetComponent<Rigidbody>().AddForce(this.transform.forward * 1, ForceMode.Impulse);

        }
        prevPos = sphere.transform.position;

    }

    /*Vector3 updateVelocity(Vector3 oldPosition)
    {
        Vector3 velocity = oldPosition - prevPosition;
        return oldPosition + velocity - gravity;
    }*/
}
