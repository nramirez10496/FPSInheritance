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
    [SerializeField] float playerSightRange =4f; //how close should the player have to be for the enemy to see 
    [SerializeField] float playerAttackRange = 1f;//how close should the player be before the enemy will lunge at them
    [SerializeField] float currentStateElapsed = 0f; //how much time has passed in the current state (reset on changing state)
    [SerializeField] float recoveryTime = 1f;//how long should the recovery state last before the enemy switches back
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    float distanceToTarget;//distance of enemy to player 
    Vector3 wanderPoint;//point enemy wanders to

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        startingLocation = transform.position;
        wanderPoint = startingLocation;//initialize wander point
        NewWanderPoint();
        //origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //distance of enemy to player
        distanceToTarget = Vector3.Distance(transform.position, target.position);

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

    void NewWanderPoint()
    {
        for (int i = 0; i < 30; i++) // Try multiple times to find a valid spot
        {
            //create random point to go to 
            Vector3 randomPoint = Random.insideUnitSphere * wanderRange + startingLocation;

            NavMeshHit hit;

            //check randompoint is on surface
            if (NavMesh.SamplePosition(randomPoint, out hit, wanderRange, NavMesh.AllAreas))
            {
                //update new point and move towards it
                wanderPoint = hit.position;
                agent.SetDestination(wanderPoint);
                return;
            }
        }
    }

    //update functions for each state
    void UpdateWander()
    {
        agent.isStopped = false;//moving again
        //enemy is close to player, pursue
        if (distanceToTarget <= playerSightRange)
        {
            WanderToPursue();
            return;
        }

        //enemy reacher random point, set new one
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            NewWanderPoint();
        }
    }

    void UpdatePursue()
    {
        agent.isStopped = false;//moving again

        // Move toward player
        agent.SetDestination(target.position);

        //enemy is close enough to attack, attack
        if (distanceToTarget <= playerAttackRange)
        {
            PursueToAttack();
        }

        //enemy is not close to player, return to wander
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
        Debug.Log("ATTACK!");

        AttackToRecovery();
    }

    //transition states
    void WanderToPursue()
    {
        enemyCurrentState = EnemyStates.pursue;
        currentStateElapsed = 0f;
    }

    void PursueToAttack()
    { 
        enemyCurrentState = EnemyStates.attack;
        currentStateElapsed = 0f;
    }

    void AttackToRecovery()
    {
        enemyCurrentState = EnemyStates.recovery;
        currentStateElapsed = 0f;
    }

    void RecoveryToPursue()
    {
        enemyCurrentState = EnemyStates.pursue;
        currentStateElapsed = 0f;
    }

    void PursueToWander()
    {
        enemyCurrentState = EnemyStates.wander;
        currentStateElapsed = 0f;
    }
}
