using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class user : MonoBehaviour {


    public ParticleSystem Cmarker1;
    public GameObject mouth;
    public GameObject FireR;
    public float CDtime;
    public Slider Magic;
    public Slider Health;
    public AudioSource Fireball;

    public List<ParticleCollisionEvent> collisionEvents;
    // Use this for initialization
    void Start () {

        FireR.SetActive(false);
        mouth.SetActive(false);
        CDtime = 3;
    }
	
	// Update is called once per frame
	void Update () {
        if (Magic.value < 2.0f)
        {
            Magic.value += 0.0001f;
        }
        
        // Debug.Log(FireR.activeSelf);

        CDtime += Time.deltaTime;
      if (OVRInput.Get(OVRInput.RawButton.RThumbstick) && FireR.activeSelf == false && CDtime >= 4&&Magic.value>0)
        {
            Fireball.Play();
            Fireball.loop = false;
            FireR.SetActive(true);
            mouth.SetActive(true);
            Magic.value -= 0.8f;
            CDtime = 0;
        }

       if (CDtime >= 1.5)
        {
            FireR.SetActive(false);
            mouth.SetActive(false);
        }
       
    

    }

void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.name == "Object1")
        {
            Debug.Log("Sword!");
        }
    }

private void OnParticleCollision(GameObject other) {
        Debug.Log("triggered");
        int numCollisionEvents = Cmarker1.GetCollisionEvents(other, collisionEvents);
         for(int i =0; i < numCollisionEvents; i++) { 
             Debug.Log("triggered");
              
             FireR.SetActive(true);
             mouth.SetActive(true);
         }   
     }
}
