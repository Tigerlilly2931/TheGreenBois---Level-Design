using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public GameObject enemySpawnPoint;
    public Transform goal;
    public int enemyHealth = 10;
    public bool playerInRange = false;
    [SerializeField] CapturetheFlag ctf;
    [SerializeField] float range = 20f;
    [SerializeField] float TimeToSwitch = 20f;
    public float timetoRespawn = 10f;
    [SerializeField] GameObject[] goals = new GameObject[5];
    List<GameObject> movingPoints = new List<GameObject>();
    bool isDead = false;
    float timeToShoot = 0;
    [SerializeField] GameObject enemyHolder;
    NavMeshAgent agent;
    [SerializeField] AudioClip enemypow;
    [SerializeField] AudioSource enemysource;
    void Start()
    {
        enemysource = GetComponent<AudioSource>();
        isDead = false;
        ctf = FindObjectOfType<CapturetheFlag>();
        playerInRange = false;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
        enemyHealth = 10;
    }

    private void Update()
    {
        playerInRange = Vector3.Distance(transform.position, ctf.gameObject.transform.position) < range;
        if (playerInRange && Time.time >= timeToShoot && !isDead)
        {
            timeToShoot = Time.time + 1f / .8f;
            transform.LookAt(ctf.gameObject.transform.position);
            EnemyShoot();
        }
        if(enemyHealth <= 0)
        {
            enemyHealth = 10;
            //die enemy
            isDead = true;
            //gameObject.SetActive(false);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            //respawn?
            TimeToSwitch = 10f;
            gameObject.transform.position = enemySpawnPoint.transform.position;
            goal = null;
            agent.destination = enemySpawnPoint.transform.position;
        }
        if (isDead)
        {
            timetoRespawn -= Time.deltaTime;
            if(timetoRespawn < 0)
            {
                isDead = false;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                gameObject.GetComponent<CapsuleCollider>().enabled = true;
                goal = goals[Random.Range(0, goals.Length)].transform;
                timetoRespawn = 10f;
                if(TimeToSwitch > TimeToSwitch / 2f)
                {
                    agent.destination = goal.position;
                }
            }
        }
        TimeToSwitch -= Time.deltaTime;
        if (TimeToSwitch < 0)
        {
            SwitchGoals();
            TimeToSwitch = 10f;
        }
    }

    void EnemyShoot()
    {
        enemysource.PlayOneShot(enemypow);
        Debug.Log("I'm shooting at the player");
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, range, ~8))
        {
            //Debug.DrawRay(mainCam.position, mainCam.forward, Color.red, Mathf.Infinity);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Hit the player!");
                    hit.collider.gameObject.GetComponent<CapturetheFlag>().health -= 5;
                }
                else
                {
                    Debug.Log("Enemy hit a wall");
                    //Debug.Log(hit.collider.gameObject.name);
                    //Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
                    //GameObject bH = Instantiate(bulletHole, hit.point, impactRotation);
                    //Destroy(bH, 5f);
                }
            }
        }
    }
    void SwitchGoals()
    {
        for(int i = 0; i < goals.Length; i++)
        {
            movingPoints.Add(goals[i]);
            Debug.Log(movingPoints[i]);
        }
        foreach (Transform enemy in enemyHolder.transform)
        {
            Transform tempPont = movingPoints[Random.Range(0, movingPoints.Count)].transform;
            enemy.gameObject.GetComponent<EnemyMovement>().goal = tempPont;
            movingPoints.Remove(tempPont.gameObject);
            agent.destination = goal.position;
        }
    }


}
