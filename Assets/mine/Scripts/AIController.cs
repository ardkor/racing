using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private Transform[] waypoints;

    [SerializeField] private WaysContainer _waysContainer;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Rigidbody rb;

    private LevelManager _levelManager;

    //private bool _isStarted = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //rb = GetComponentInChildren<Rigidbody>();

        agent.updateRotation = true;
        agent.updatePosition = false;
        agent.avoidancePriority = 0;// Random.Range(30, 70);
        agent.speed = Random.Range(14f, 16f);
    }
    private void OnEnable()
    {
        _levelManager = FindAnyObjectByType<LevelManager>();
        _levelManager.levelStarted += StartMoving;
    }
    private void OnDisable()
    {
        _levelManager.levelStarted -= StartMoving;
    }
    public void IntallWaypoints(int levelNum) 
    {
        switch (levelNum)
        {
            case 0:
                waypoints = _waysContainer.firstLevelPath;
                break;
            case 1:
                waypoints = _waysContainer.secondLevelPath;
                break;
        }
    }
    private void StartMoving()
    {
        agent.updatePosition = true;
        MoveToNextWaypoint();
    }
    private void EndMoving()
    {
        agent.updatePosition = false;
    }
    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        if (agent.updatePosition && currentWaypointIndex == waypoints.Length)
        {
            EndMoving();
            return;
        }
        float rand = Random.Range(-2, 0);
        Debug.Log(rand);
        agent.SetDestination(waypoints[currentWaypointIndex].position + new Vector3(0, 0, rand));

        //agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        
        if (agent.updatePosition && !agent.pathPending && agent.remainingDistance < 1f)
        {
            currentWaypointIndex += 1;//= (currentWaypointIndex + 1) % waypoints.Length;
            MoveToNextWaypoint();
        }
    }
}
