// http://luminaryapps.com/blog/arcing-projectiles-in-unity/



using System;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class CreateDart : MonoBehaviour

{

    public GameObject dart;

    public AudioSource ding;

    public GameObject player;



    public GameObject lefthand;



    private bool inAir;

    private float speed;    // initial speed of dart in air

    private float dartScale;

    private int prev_x, prev_z; //sign of x, z component of leftHand.transform.position (+->1, 0->0, -->-1)



    private Queue<Vector3> trajectory; // used for extrapolate initial velocity of dart in air

    private int maxQueue; // max count in trajectory

    private bool hasCollide;



    private LineRenderer lineRenderer;

    private float range;

    private float maxJumpDistance;

    public GameObject kunai_direction;

    // Use this for initialization

    void Start()

    {



        lineRenderer = GetComponent<LineRenderer>();



        dart.SetActive(false);

        Physics.gravity = new Vector3(0, -3f, 0);

        dart.GetComponent<Rigidbody>().mass = 1.0f;

        inAir = true;

        speed = 50.0f;

        range = 500000000;

        maxJumpDistance = 1000;



        dartScale = 0.1f;

        dart.transform.localScale = new Vector3(dartScale, dartScale, dartScale);



    }



    // Update is called once per frame

    void Update()

    {

        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);



        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.2 && inAir)

        {

            //Debug.Log("Dart created");

            inAir = false;

            if (dart.transform.parent != lefthand.transform)

            {

                dart.transform.SetParent(lefthand.transform);

            }

            dart.transform.localPosition = new Vector3(0, 0, 0);

            dart.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            dart.GetComponent<Rigidbody>().useGravity = false;

            dart.GetComponent<Rigidbody>().isKinematic = true;

            dart.SetActive(true);

            dartScale = 1;



        }

        else if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) < 0.2 && !inAir)

        {

            //Debug.Log("Release index finger");
            if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) == 0)
            {
                ding.Play();

            }

            
           
            // If hit terrain, shoot dart 

            RaycastHit hit;

            Vector3 dir = lefthand.transform.forward;

            if (Physics.Raycast(kunai_direction.transform.position, kunai_direction.transform.forward, out hit, range))

            {

                if (hit.transform.gameObject.name == "Terrain")

                {

                    Debug.Log("Terrain hit");
                    Vector3 hitPosition = hit.point;

                    double distance = getDistance(hitPosition, lefthand.transform.position);

                    //if (distance < maxJumpDistance)

                    if(distance <= 50)

                    {

                        inAir = true;

                        if (dart.transform.parent != null)

                            dart.transform.parent = null;



                        dart.GetComponent<Rigidbody>().useGravity = false;

                        dart.GetComponent<Rigidbody>().isKinematic = false;

                        dart.GetComponent<Rigidbody>().AddForce(dir * speed, ForceMode.Impulse);

                    }

                }

            }

            if (!inAir)

            {

                inAir = false;

                dart.SetActive(false);

                lineRenderer.SetPosition(0, lefthand.transform.position);

                lineRenderer.SetPosition(1, lefthand.transform.position);

                OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);

            }



        }

        else if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > 0.2f && hasCollide)

        {

            //Debug.Log("Dart collides terrain");

            hasCollide = false;

            dart.GetComponent<Rigidbody>().isKinematic = false;

            player.transform.position = dart.transform.position;

        }





        if (!inAir)

        {   

            lineRenderer.SetPosition(0, lefthand.transform.position + lefthand.transform.forward / 5);

            lineRenderer.SetPosition(1, lefthand.transform.position + lefthand.transform.forward * 100);

            dart.transform.eulerAngles = new Vector3(lefthand.transform.eulerAngles.x, lefthand.transform.eulerAngles.y, lefthand.transform.eulerAngles.z);

            dart.transform.Rotate(90, 0, 0, Space.Self);

        }

        else

        {

            lineRenderer.SetPosition(0, lefthand.transform.position);

            lineRenderer.SetPosition(1, lefthand.transform.position);

        }









    }





    void OnCollisionEnter(Collision col)

    {

        if (col.gameObject.name == "kunai")

        {

            // player.transform.position = dart.transform.position;

            hasCollide = true;

            col.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            dart.SetActive(false);

            inAir = false;

        }

    }



    double getDistance(Vector3 v1, Vector3 v2)

    {

        double distance = Math.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));

        return distance;

    }

}