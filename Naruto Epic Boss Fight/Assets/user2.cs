using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class user2 : MonoBehaviour {

    public GameObject Dart;
    public ParticleSystem Cmarker1;
    public GameObject Cmarker;
    public float CDtime;

    public int in_tutorial;
    public GameObject mouth;
    public ParticleSystem Rasneg;
    public GameObject menu;
    public GameObject Teleport_1;
    public GameObject Teleport_2;
    public GameObject Teleport_3;
    public GameObject Rasengan_1;
    public GameObject Rasengan_2;
    public GameObject Fire_1;
    public GameObject Fire_2;
    public bool donewithFire = false;
    public bool donewithRas = false;

    public List<ParticleCollisionEvent> collisionEvents;
    // Use this for initialization
    void Start () {
        
        CDtime = 3;
        ParticleSystem.EmissionModule emission;
        in_tutorial = 0;
        menu.SetActive(true);
        Teleport_1.SetActive(false);
        Teleport_2.SetActive(false);
        Teleport_3.SetActive(false);
        Rasengan_1.SetActive(false);
        Rasengan_2.SetActive(false);
        Fire_1.SetActive(false);
        Fire_2.SetActive(false);
        mouth.SetActive(false);
        Cmarker.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.Get(OVRInput.Button.One)){
            in_tutorial = 2;
            SceneManager.LoadScene(sceneName: "Main");
            menu.SetActive(false);
        }

        if (OVRInput.Get(OVRInput.Button.Two))
        {
            in_tutorial = 1;
            menu.SetActive(false);
        }
        if (OVRInput.Get(OVRInput.Button.Start))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        
        // Debug.Log(FireR.activeSelf);

        CDtime += Time.deltaTime;

        if (OVRInput.Get(OVRInput.RawButton.RThumbstick) && CDtime >= 3 )
        {
            Cmarker.SetActive(true);
            mouth.SetActive(true);
            
            CDtime = 0;
        }

        if (CDtime >= 1.5)
        {
           Cmarker.SetActive(false);
            mouth.SetActive(false);
        }

        if (in_tutorial == 1)
        {
            menu.SetActive(false);
            Teleport_1.SetActive(true);
            if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) == 1)
            {
                in_tutorial = 2;
            }
        }

        if (in_tutorial == 2)
        {
            Teleport_1.SetActive(false);
            Teleport_2.SetActive(true);
            if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > 0.2f)
            {
                in_tutorial = 3;
            }
        }

        if (in_tutorial == 3)
        {
            Teleport_2.SetActive(false);
            Teleport_3.SetActive(true);
            if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) == 1)
            {
                in_tutorial = 4;
            }
        }

        if (in_tutorial == 4)
        {
            Teleport_3.SetActive(false);
            Rasengan_1.SetActive(true);
            // emission
            if (Rasneg.emission.rate.constant > 4)
            {
                in_tutorial = 5;
            }
        }

        if (in_tutorial == 5)
        {
            Rasengan_1.SetActive(false);
            Rasengan_2.SetActive(true);
            if (OVRInput.Get(OVRInput.RawButton.RThumbstick))
            {
                in_tutorial = 6;
            }
        }

        if (in_tutorial == 6)
        {
            Rasengan_2.SetActive(false);
            Fire_1.SetActive(true);
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
              
             //FireR.SetActive(true);
             mouth.SetActive(true);
         }   
     }
}
