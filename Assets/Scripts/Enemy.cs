using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//state machine
enum EnemyStates
{
    wander,
    pursue,
    wait,
    attack
}

public class Enemy : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    Vector3 origin;

    //current state
    [SerializeField]EnemyStates enemyCurrentState;

    //AI variables
    [SerializeField] float wanderRange;//how far from the starting point should the enemy be allowed to wander
    [SerializeField] Vector3 startingLocation; //enemy’s starting location, set on start/spawn
    [SerializeField] float playerSightRange; //how close should the player have to be for the enemy to see 
    [SerializeField] float playerAttackRange;//how close should the player be before the enemy will lunge at them
    [SerializeField] float currentStateElapsed; //how much time has passed in the current state (reset on changing state)
    [SerializeField] float recoveryTime;//how long should the recovery state last before the enemy switches back


    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //AI 
        switch(enemyCurrentState)
        {
            case EnemyStates.wander:
                UpdateWander();
                break;

            case EnemyStates.pursue:
                UpdatePursue();
                break;

            case EnemyStates.wait:
                UpdateWait();
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
        transform.position = origin;
    }

    //update functions for each state
    void UpdateWander()
    {

    }

    void UpdatePursue()
    {

    }

    void UpdateWait()
    {

    }

    void UpdateAttack()
    {

    }
}
