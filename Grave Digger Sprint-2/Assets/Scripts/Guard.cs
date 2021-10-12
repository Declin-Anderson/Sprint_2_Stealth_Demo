using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    // An array of map points the guard will walk through, loops back to start after completion
    public Transform[] Goals;
    // An array of the points in the guard patrol route they will stop at to look for the player
    public int[] LookoutGoals;

    private UnityEngine.AI.NavMeshAgent Agent;
    private Transform CurrentGoal;
    private int RouteCheckpoint = 1;

    public float DetectionRange = 5;
    private float BaseDetectionRange;
    private bool IsAlerted = false;
    private bool AlertCooldown = false;

    private IEnumerator CoStop;
    private IEnumerator CoChase;

    private GameObject Player;
    private Transform PlayerTransform;

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

            CoChase = Alerted();
            if (CoStop != null)
            {
                StopCoroutine(CoStop);
            }
            StartCoroutine(CoChase);
        }
        else if (DistToPlayer.magnitude < 1f && IsAlerted)
        {
            CoChase = Caught();
            StartCoroutine(CoChase);
        }

        Vector3 DistToGoal = CurrentGoal.position - transform.position;
        DistToGoal.y = 0;
        if (DistToGoal.magnitude < 0.5 && !IsAlerted)
        {
            var CheckLookout = System.Array.Exists(LookoutGoals, x => x == RouteCheckpoint);

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
    }

    private IEnumerator ShortGoalStop()
    {
        yield return new WaitForSeconds(0.75f);
        Agent.destination = CurrentGoal.position;
    }

    private IEnumerator LongGoalStop()
    {
        yield return new WaitForSeconds(1.0f);
        DetectionRange = BaseDetectionRange * 2;
        yield return new WaitForSeconds(2.0f);
        DetectionRange = BaseDetectionRange;
        Agent.destination = CurrentGoal.position;
    }

    private IEnumerator Alerted()
    {
        IsAlerted = true;
        Agent.destination = transform.position;
        Agent.areaMask = -1;
        DetectionRange = BaseDetectionRange;
        yield return new WaitForSeconds(1.0f);
        Agent.destination = PlayerTransform.position;
    }

    private IEnumerator Caught()
    {
        Agent.destination = transform.position;
        AlertCooldown = true;
        IsAlerted = false;
        yield return new WaitForSeconds(1.0f);
        Debug.Log("got you");
        Debug.Log("returning to route");
        Agent.destination = CurrentGoal.position;
        yield return new WaitForSeconds(5.0f);
        AlertCooldown = false;
    }
    
    private IEnumerator Evidence()
    {
        
        Debug.Log("i got you now");
        ST.AlertIncrease(1);

        yield return new WaitForSeconds(0);
    }

    public void GenerateGuard(Transform[] InGoals, int[] InLookoutGoals)
    {
        Goals = InGoals;
        LookoutGoals = InLookoutGoals;

        BaseDetectionRange = DetectionRange;

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = Player.GetComponent<Transform>();
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        CurrentGoal = Goals[RouteCheckpoint];
        Agent.destination = CurrentGoal.position;
    }
    
    //private PowerUp()
    //{

    //}
}
