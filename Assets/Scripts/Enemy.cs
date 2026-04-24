using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//state machine
enum EnemyStates
{
    wander,
    pursue,
    attack,
    recovery    
}

public class Enemy : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    //Vector3 origin;

    //current state
    [SerializeField]EnemyStates enemyCurrentState;

    //AI variables
    [SerializeField] float wanderRange = 5f;//how far from the starting point should the enemy be allowed to wander
    Vector3 startingLocation; //enemy’s starting location, set on start/spawn
    [SerializeField] float playerSightRange =10f; //how close should the player have to be for the enemy to see 
    [SerializeField] float playerAttackRange = 2f;//how close should the player be before the enemy will lunge at them
    [SerializeField] float currentStateElapsed = 0f; //how much time has passed in the current state (reset on changing state)
    [SerializeField] float recoveryTime = 3f;//how long should the recovery state last before the enemy switches back
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        startingLocation = transform.position;
        //origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //count time
        currentStateElapsed += Time.deltaTime;
        //AI 
        switch (enemyCurrentState)
        {
            case EnemyStates.wander:
                UpdateWander();
                break;

            case EnemyStates.pursue:
                UpdatePursue();
                break;

            case EnemyStates.recovery:
                UpdateRecovery();
                break;

            case EnemyStates.attack:
                UpdateAttack();
                break;
        }
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        GetComponent<Rigidbody>().AddForce(knockback, ForceMode.Impulse);
    }

    public void Respawn()
    {
        //transform.position = origin;
        transform.position = startingLocation;
    }

    //update functions for each state
    void UpdateWander()
    {

    }

    void UpdatePursue()
    {
        agent.isStopped = false;//moving again

        //distance of enemy to player
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        
        // Move toward player
        agent.SetDestination(target.position);

        //enemy is close enough to attack
        if (distanceToTarget <= playerAttackRange)
        {
            PursueToAttack();
        }

        //enemy is not close to player
        else if(distanceToTarget > playerSightRange)
        {
            PursueToWander();
        }
    }

    void UpdateRecovery()
    {
        //stop movement for recovery
        agent.isStopped = true; 

        //recovery time is over, cont pursue
        if (currentStateElapsed >= recoveryTime)
        {
            RecoveryToPursue();
        }
    }

    void UpdateAttack()
    {

    }

    //transition states
    void WanderToPursue()
    {
        enemyCurrentState = EnemyStates.pursue;
    }

    void PursueToAttack()
    { 
        enemyCurrentState = EnemyStates.attack;
    }

    void AttackToRecovery()
    {
        enemyCurrentState = EnemyStates.recovery;
    }

    void RecoveryToPursue()
    {
        enemyCurrentState = EnemyStates.pursue;
    }

    void PursueToWander()
    {
        enemyCurrentState = EnemyStates.wander;
    }
}
