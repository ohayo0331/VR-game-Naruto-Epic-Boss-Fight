// http://luminaryapps.com/blog/arcing-projectiles-in-unity/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateSphere2 : MonoBehaviour
{

    public GameObject sphere;
    public GameObject smoke;
    private bool inAir;
    private Vector3 prevPos;
    private float speed;
    public GameObject leftHand;
    private Vector3 prevLeftHandPos;
    private float sphereScale = 1.0f;
    public int in_tutorial = 0;
    public AudioSource Rasen_S;
    public AudioClip raseb_START;
    public AudioClip raseb_rub;
    public AudioClip raseb_exp;
    private int prev_x, prev_z;

    private ParticleSystem ps_sphere;
    private ParticleSystem ps_smoke;

    // Use this for initialization
    void Start()
    {

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
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.One))
        {
            in_tutorial = 2;
            SceneManager.LoadScene(sceneName: "Main");
        }

        if (OVRInput.Get(OVRInput.Button.Two))
        {
            in_tutorial = 1;
        }
        //float i = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
        //Debug.Log(i);
        if (in_tutorial > 0)
        {
            if (inAir)
            {
                // nothing here
            }
            else
            {
                // gesture
                if (Vector3.Dot((leftHand.transform.position - this.transform.position), this.transform.up) > 0)
                {
                    
                    // left hand is above right hand2
                    int pos_x = Math.Sign(Vector3.Dot((leftHand.transform.position - this.transform.position), this.transform.right));
                    int pos_z = Math.Sign(Vector3.Dot((leftHand.transform.position - this.transform.position), this.transform.forward));

                    if (prev_x * pos_x < 0 || prev_z * pos_z < 0 && sphereScale < 3)
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
                Debug.Log("Sphere created");
               
                ps_sphere.Play();
                inAir = false;
                sphere.transform.SetParent(this.transform);
                sphere.transform.localPosition = new Vector3(-0.073f, -0.065f, -0.026f);
                sphere.GetComponent<Rigidbody>().useGravity = false;
                sphere.GetComponent<Rigidbody>().isKinematic = true;
                Rasen_S.clip = raseb_START;
                Rasen_S.Play();
                Rasen_S.loop = true;
                var smoke_emission = ps_smoke.emission;
                smoke_emission.rateOverTime = 0;

                var sphere_emission = ps_sphere.emission;
                sphere_emission.rateOverTime = 0;

                sphere.SetActive(true);
                sphereScale = 1;

            }
            else if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) < 0.99f && !inAir)
            {
                //Debug.Log("Sphere leaves hand");
                inAir = true;
                Rasen_S.clip = raseb_rub;
                Rasen_S.Play();
                Rasen_S.loop = false;
                //startPos = transform.position;
                if (sphere.transform.parent != null)
                    sphere.transform.parent = null;

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
}
