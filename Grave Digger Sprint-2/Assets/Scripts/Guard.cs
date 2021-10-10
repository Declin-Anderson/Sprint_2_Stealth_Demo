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

    private IEnumerator CoStop;
    private IEnumerator CoChase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 DistToGoal = CurrentGoal.position - transform.position;
        DistToGoal.y = 0;
        if (DistToGoal.magnitude < 0.5)
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
        //DetectionRange = BaseDetectionRange * 2;
        yield return new WaitForSeconds(2.0f);
        //DetectionRange = BaseDetectionRange;
        Agent.destination = CurrentGoal.position;
    }

    public void GenerateGuard(Transform[] InGoals, int[] InLookoutGoals)
    {
        Goals = InGoals;
        LookoutGoals = InLookoutGoals;

        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        CurrentGoal = Goals[RouteCheckpoint];
        Agent.destination = CurrentGoal.position;
    }
}
