using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Effect : MonoBehaviour {

    // Use this for initialization
    public ParticleSystem part;
    public ParticleSystem exp;
    public List<ParticleCollisionEvent> collisionEvents;

    // Update is called once per frame
    void Update () {
        // exp.Play();
    }

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        exp.Pause();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        //Debug.Log(numCollisionEvents);
        while (i < 1)
        {
            if (rb)
            {
                // Debug.Log("effect");
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = collisionEvents[i].velocity * 10;
                exp.transform.position = pos;
                exp.Play(true);
                
                rb.AddForce(force);
                part.Stop();
            }
            i++;
        }
    }
}
