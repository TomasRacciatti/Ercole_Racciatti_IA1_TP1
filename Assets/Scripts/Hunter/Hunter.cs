using UnityEngine;

public class Hunter : Agent
{
    [Header("Hunter")]
    public float energy;
    public float maxEnergy = 100f;
    public Transform[] patrolPoints;

    public Boid target;

    public FSM stateMachine;


    private void Awake()
    {
        stateMachine = new();

        
        stateMachine.AddNewState<HunterIdle>().SetAgent(this);
        stateMachine.AddNewState<HunterPatrol>().SetAgent(this);
        stateMachine.AddNewState<HunterChase>().SetAgent(this);

        stateMachine.SetInitialState<HunterPatrol>();
        
    }

    private void Start()
    {
        target = GameObject.FindAnyObjectByType<Boid>();

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
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _visionRadius);
    }
}
