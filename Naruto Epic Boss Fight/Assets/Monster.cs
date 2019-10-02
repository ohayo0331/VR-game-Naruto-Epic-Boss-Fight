using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BreadcrumbAi;
public class Monster : MonoBehaviour {

    public ParticleSystem Cmarker1;
    public List<ParticleCollisionEvent> collisionEvents;
    public Animator anim;
    public Slider blood;
    public ParticleSystem MB;
    public Slider Player_Health;
    public AudioSource monster;
    public AudioSource ambient;
    public AudioClip Monster_WALK;
    public AudioClip Monster_attack;
    public AudioClip Monster_attacked_by_U;
    public AudioClip Monster_Dead;
    public AudioClip Monster_Slayed;
    public AudioSource m_HIT_BY_R;
    public AudioClip R_hit_M;
    bool dead = false;
    float passedTime;
    float CDtime_Walk;
    float CDtime_Attack;
    float CDtime_MagicEffect;
    
    private Ai ai;
    private string animRun = "Walk";
    private string animDeath1 = "Death1";
    private string animAttack = "Attack";
    float CDtime_End;

    //public GameObject MagicHit;
    // Use this for initialization
    void Start () {
        collisionEvents = new List<ParticleCollisionEvent>();
        anim = GetComponent<Animator>();
        ai = GetComponent<Ai>();
        CDtime_Walk = 1.7333f;
        CDtime_Attack = 2.767f;
        CDtime_MagicEffect = 3.2f;
        CDtime_End = 0f;
    }
	
	// Update is called once per frame
	void Update () {

        Animation();
        passedTime += Time.deltaTime;
        if (dead)
        {
            ai.enabled = false;
        }
        
    }

    void FixedUpdate()
    {
       // Animation();
    }


    private void Animation()
    {
        CDtime_Walk += Time.deltaTime;
        CDtime_End += Time.deltaTime;
        if (CDtime_End >= 3 && dead)
        {
            SceneManager.LoadScene(sceneName: "Credit");
        }
        if (ai.moveState != Ai.MOVEMENT_STATE.IsIdle && CDtime_Walk >= 1.733f&&blood.value >0.0f)
        {
            monster.clip = Monster_WALK;
            monster.Play();
            anim.Play("Walk", -1, 0f);
            CDtime_Walk = 0;
        }

        CDtime_Attack += Time.deltaTime;

        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] animClip = anim.GetCurrentAnimatorClipInfo(0);
        float mytime = animClip[0].clip.length * animInfo.normalizedTime;
        //
        if (mytime >= 1.1f && mytime <= 1.2f && animClip[0].clip.name == "Attack")
        {
            Player_Health.value -= 0.1f;
        }

        if (ai.attackState == Ai.ATTACK_STATE.CanAttackPlayer && CDtime_Attack >= 2.767f&&!dead)
        {
           
            monster.clip = Monster_attack;
            monster.Play();
            if (!dead)
            {
                anim.Play("Attack", -1, 0f); 

            }


 
            if (Player_Health.value == 0) {
                SceneManager.LoadScene(sceneName: "Tutorial");
            }
            CDtime_Attack = 0;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
       
        if (!dead&&other.gameObject.name=="ErekiBall2"&&passedTime>=0.3f)
        {
            m_HIT_BY_R.Play();
            m_HIT_BY_R.loop = false;
            Debug.Log("Ereki");
            passedTime = 0;
            anim.Play("Damage", -1, 0f);
            blood.value -= 0.3f;
        }

        else if (!dead && other.tag == "Fire" && passedTime >= 0.3f)
        {
            Debug.Log("Flamethrower");
            passedTime = 0;
            anim.Play("Damage", -1, 0f);
            blood.value -= 0.05f;
        }

        if (blood.value == 0.0f && !dead)
        {
            dead = true;
            anim.Play("Death", -1, 0f);
            monster.clip = Monster_Dead;
            monster.loop = false;
            monster.Play();
            CDtime_End = 0;
            
            ambient.clip = Monster_Slayed;
            ambient.Play();
            ambient.loop = false;
            blood.value = -0.1f;
        }

        

        if (other.gameObject.name == "Spark")
        {
            Debug.Log("Spark collide");
            passedTime = 0;
            anim.Play("Damage", -1, 0f);
            blood.value -= 0.1f;
            //MagicHit.SetActive(true);

        }

        int numCollisionEvents = MB.GetCollisionEvents(other, collisionEvents);
        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        // Debug.Log(numCollisionEvents);
        while (i < numCollisionEvents)
        {
           // Debug.Log("particle");
            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = collisionEvents[i].velocity * 10;
                Debug.Log(pos);
            }
            i++;
        }
        //int numCollisionEvents = Cmarker1.GetCollisionEvents(other, collisionEvents);
        //for (int i = 0; i < numCollisionEvents; i++)
        //{
        //}
    }
}
