using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    // An array of map points the guard will walk through, loops back to start after completion
    public Transform[] Goals;
    // An array of the points in the guard patrol route they will stop at to look for the player
    public int[] LookoutGoals;

    // The guard's navmesh agent component
    private UnityEngine.AI.NavMeshAgent Agent;

    // The position of the tile the guard currently has in their patrol route
    private Transform CurrentGoal;

    // Used to hold the point that the guard is currently investigating
    private Vector3 CurrentInvestigation = new Vector3();

    // The position of the tile in the guard's patrol route array that they are currently moving to
    private int RouteCheckpoint = 1;

    // The distance the guard has to be from the player to be alerted (Does not catch the player yet)
    public float DetectionRange = 4.0f;

    // The base detection range without any modifiers
    private float BaseDetectionRange;

    // The distance the guard has to be from the player to catch them
    private float CatchRange = 1.0f;

    // The base catch range without any modifiers
    private float BaseCatchRange;

    // Multiplier for how long it will take a guard to react to noise
    private float ReactionMultiplier = 1.0f;

    // Used to signify that the guard is currently alerted by a noise
    private bool IsAlerted = false;

    // Used to check if guard is currently moving to investigate a noise
    private bool IsInvestigating = false;

    // A cooldown boolean to prevent a guard from immediatly investigating a new noise after finishing another investigation
    private bool AlertCooldown = false;

    private IEnumerator CoStop;
    private IEnumerator CoChase;
    private IEnumerator CoInvestigate;

    private GameObject Player;
    private Transform PlayerTransform;

    // Reference to the stealth script
    private Stealth2 ST;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 DistToPlayer = PlayerTransform.position - transform.position;
        DistToPlayer.y = 0;
        if (DistToPlayer.magnitude < DetectionRange && !IsAlerted && !AlertCooldown)
        {
            Debug.Log("Found");

            StopAllCoroutines();

            CoChase = Alerted();
            StartCoroutine(CoChase);
        }
        else if (DistToPlayer.magnitude < CatchRange && IsAlerted)
        {
            if (CoChase != null)
            {
                StopCoroutine(CoChase);
            }
            CoChase = Caught();
            StartCoroutine(CoChase);
        }

        Vector3 DistToGoal = CurrentGoal.position - transform.position;
        DistToGoal.y = 0;
        if (DistToGoal.magnitude < 0.5 && !IsAlerted)
        {
            var CheckLookout = System.Array.Exists(LookoutGoals, x => x == RouteCheckpoint);
            Agent.areaMask = 15;

            if (RouteCheckpoint + 1 < Goals.Length)
            {
                RouteCheckpoint++;
            }
            else
            {
                RouteCheckpoint = 0;
            }

            CurrentGoal = Goals[RouteCheckpoint];

            if (CheckLookout)
            {
                CoStop = LongGoalStop();
            }
            else
            {
                CoStop = ShortGoalStop();
            }

            StartCoroutine(CoStop);
        }

        if (IsInvestigating)
        {
            Vector3 DistToInvestigation = CurrentInvestigation - transform.position;
            if (DistToInvestigation.magnitude < 0.5f)
            {
                StopAllCoroutines();

                CoInvestigate = Investigate();
                StartCoroutine(CoInvestigate);
            }
        }
    }

    private IEnumerator ShortGoalStop()
    {
        yield return new WaitForSeconds(0.75f);
        Agent.destination = CurrentGoal.position;
    }

    // Used to simulate the guard "looking around." Will also flip the sprite once it is implemented
    private IEnumerator LongGoalStop()
    {
        yield return new WaitForSeconds(1.0f);
        DetectionRange = BaseDetectionRange * 1.5f;
        yield return new WaitForSeconds(2.0f);
        DetectionRange = BaseDetectionRange;
        Agent.destination = CurrentGoal.position;
    }

    // Setting areaMask to -1 allows the guard to walk off the path
    private IEnumerator Alerted()
    {
        IsInvestigating = false;
        IsAlerted = true;
        Agent.destination = transform.position;
        Agent.areaMask = -1;
        CurrentInvestigation = PlayerTransform.position;
        yield return new WaitForSeconds(1.0f * ReactionMultiplier);
        IsInvestigating = true;
        CatchRange = BaseCatchRange * 2.0f;
        DetectionRange = 0;
        Agent.destination = CurrentInvestigation;
    }

    private IEnumerator Caught()
    {
        Agent.destination = transform.position;
        AlertCooldown = true;
        IsAlerted = false;
        IsInvestigating = false;
        Debug.Log("got you");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("returning to route");
        Agent.destination = CurrentGoal.position;
        yield return new WaitForSeconds(5.0f);
        AlertCooldown = false;
    }

    private IEnumerator Investigate()
    {
        Debug.Log("Investigate Start");
        IsAlerted = false;
        IsInvestigating = false;
        yield return new WaitForSeconds(0.5f);
        DetectionRange = BaseDetectionRange * 1.5f;
        yield return new WaitForSeconds(3.0f);
        DetectionRange = BaseDetectionRange;
        Agent.destination = CurrentGoal.position;
        Debug.Log("Investigate End");
    }
    
    private IEnumerator Evidence()
    {
        
        Debug.Log("i got you now");
        ST.AlertIncrease(1);

        yield return new WaitForSeconds(0);
    }

    // Called when instantiating the guard prefab to populate its patrol route arrays
    public void GenerateGuard(Transform[] InGoals, int[] InLookoutGoals)
    {
        Goals = InGoals;
        LookoutGoals = InLookoutGoals;

        BaseDetectionRange = DetectionRange;
        BaseCatchRange = CatchRange;

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = Player.GetComponent<Transform>();
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        CurrentGoal = Goals[RouteCheckpoint];
        Agent.destination = CurrentGoal.position;
    }
}
