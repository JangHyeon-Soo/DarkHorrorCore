using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed;
    public float runSpeed;

    public float Radius;
    public Transform ZombieHead;
    public NavMeshAgent agent;
    public int RandomRate;
    public int MovingRate = 60;
    [Space(10)]

    [Header("PlayerDetect")]
    public bool PlayerChase;
    public Vector3 Destination;
    public bool IsPlayerDetected;
    public float ZombieSightDistance;
    float currentTime = 0;
    float waitingTime;
    [Space(10)]

    [Header("Audio")]
    AudioSource[] audioSources;
    public AudioClip AttackScream;
    public AudioClip RunBGM;
    [Space(10)]

    public int[] layerIndicesToIgnore;
    private int ignoreLayerMask;
    Animator animator;

    AudioSource ZombieAttackSoundSource;
    AudioSource RunSFX;

    // Start is called before the first frame update
    void Start()
    {
        if (agent == null)
        { 
            agent = GetComponent<NavMeshAgent>(); 
        }
        animator = GetComponent<Animator>();
        Destination = GetRandomPosition();
        //agent.SetDestination(Destination);
        waitingTime = Random.Range(0.5f, 3.0f);
        RandomRate = Random.Range(0, 100);
        ignoreLayerMask = GameManager.Instance.CreateIgnoreLayerMask(layerIndicesToIgnore);
        audioSources = GetComponents<AudioSource>();
        ZombieAttackSoundSource = audioSources[0];
        RunSFX = audioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameManager.Instance.playerState != GameManager.StateOfPlayer.Die)
        {
            IsPlayerDetected = PlayerDetect();
            if (IsPlayerDetected)
            {
                //transform.LookAt(GameManager.Instance.playerTF.position);

                if(PlayerChase)
                {
                    if (Vector3.Distance(GameManager.Instance.playerTF.position, agent.transform.position) <= 3.5f)
                    {
                        if(RunSFX.isPlaying == true)
                        {
                            RunSFX.Stop();
                        }
                        animator.SetFloat("Speed", 0);
                        agent.isStopped = true;
                        agent.speed = 0;
                        agent.SetDestination(agent.transform.position);
                        animator.Play("zombie neck bite");
                        
                    }

                    else
                    {
                        if(RunSFX.isPlaying == false)
                        {
                            RunSFX.PlayOneShot(RunBGM);
                        }
                        agent.speed = runSpeed;
                        animator.SetFloat("Speed", runSpeed);
                        agent.isStopped = false;
                        agent.SetDestination(GameManager.Instance.playerTF.position);
                        
                    }
                }
                
            }

            else
            {
                if (RandomRate < MovingRate)
                {
                    agent.speed = walkSpeed;

                    animator.SetFloat("Speed", walkSpeed);
                    agent.SetDestination(Destination);
                    if (agent.remainingDistance < 1f)
                    {
                        Destination = GetRandomPosition();
                        SetRandomRate();
                    }
                }


                else
                {
                    animator.SetFloat("Speed", 0);
                    if (currentTime > waitingTime)
                    {
                        currentTime = 0;
                        SetRandomRate();
                    }

                    else
                    {
                        currentTime += Time.deltaTime;
                    }

                }
            }
        }

        else
        {
            animator.SetFloat("Speed", 0);
            agent.isStopped = true;
            agent.speed = 0;
            agent.SetDestination(agent.transform.position);
            animator.Play("zombie neck bite");

            if(!ZombieAttackSoundSource.isPlaying)
            {
                ZombieAttackSoundSource.PlayOneShot(AttackScream);
            }
            
        }
    }



    public void SetRandomRate()
    {
        RandomRate = Random.Range(0, 100);
    }


    bool IsOnNavMesh(Vector3 tempDest)
    {

        NavMeshHit hit;

        if (NavMesh.SamplePosition(tempDest, out hit, Radius, NavMesh.AllAreas))
        {
            //Debug.Log(true);
            return true;
        }
       // Debug.Log(false);
        return false;
    }
    Vector3 GetRandomPosition()
    {
        Vector3 result = Random.insideUnitSphere * Radius;
        result += GameManager.Instance.playerTF.position;

        if(IsOnNavMesh(result))
        {
            //Debug.Log(result);
            return result;
        }

        else
        {
            //Debug.Log(Vector3.zero);
            return Vector3.zero;
        }
        

    }

    bool PlayerDetect()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, GameManager.Instance.playerTF.position - transform.position, out hit, ZombieSightDistance, ignoreLayerMask))
        {
            if(hit.transform.gameObject.layer == 7)
            {
                if (hit.collider is not SphereCollider)
                {
                    if (hit.distance <= 3f)
                    {

                        //Debug.Log("Zombie");
                        GameManager.Instance.playerState = GameManager.StateOfPlayer.Die;
                        hit.transform.GetComponent<PlayerMovement>().shake.lookAtTransform = ZombieHead;

                        //StartCoroutine(GameManager.Instance.GameOverAndRespawn());
                        return true;

                    }

                    //Debug.Log(true);
                    return true;
                }
                
                
            }

            else
            {
                //Debug.Log(false);
                return false;
            }
        }

        return false;
    }
}
