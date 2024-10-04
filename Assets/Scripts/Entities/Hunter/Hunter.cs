using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Hunter : Agent
{
    [Header("Hunter")]
    public float energy;
    public float maxEnergy = 100f;
    public float destroyDistance = 1f;
    public Transform[] patrolPoints;


    public List<Boid> target = new();

    public FSM stateMachine;


    private void Awake()
    {
        stateMachine = new();

        
        stateMachine.AddNewState<HunterIdle>().SetAgent(this);
        stateMachine.AddNewState<HunterPatrol>().SetAgent(this);
        stateMachine.AddNewState<HunterChase>().SetAgent(this);

        stateMachine.SetInitialState<HunterPatrol>();
        
    }

    private IEnumerator FindBoidsDelayed()
    {
        yield return new WaitForSeconds(1f);  // Delay to give time for boids to instantiate

        Boid[] foundBoids = GameObject.FindObjectsOfType<Boid>();
        Debug.Log("Number of Boid objects found: " + foundBoids.Length);

        target.AddRange(foundBoids);
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(FindBoidsDelayed());

        energy = maxEnergy;
    }

    private void Update()
    {
        stateMachine.OnUpdate();

        if (_directionalVelocity != Vector3.zero)  // Hunter rotation to objective
        {
            Quaternion targetRotation = Quaternion.LookRotation(_directionalVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        foreach (Boid boid in target)
        {
            if (boid != null) 
            {
                
                float distance = Vector3.Distance(transform.position, boid.transform.position);

                
                if (distance < destroyDistance)
                {
                    Destroy(boid.gameObject);
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _visionRadius);
    }
}
