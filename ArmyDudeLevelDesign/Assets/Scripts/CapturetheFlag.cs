using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class CapturetheFlag : MonoBehaviour
{
    public GameManager GM;
    bool getFlag = false;
    bool getGoal = false;
    bool isShooting;
    public Transform mainCam;
    float fireTime = 0;
    int fireRate = 2;
    int layerMask = 1 << 8;
    bool isCrouching;
    [SerializeField] GameObject flagHolder, flagSpawn;
    public CharacterController characterController;
    public FirstPersonController firstPersonController;
    [SerializeField] GameObject bulletHole;
    //List<GameObject> ammoList = new List<GameObject>();
    public float health = 25;
    int maxHealth = 25;
    float flagToSpawn = 15f;
    bool flagSitting = false;
    public float timeforHealthRegain = 5f;
    bool isWalking;
    // Start is called before the first frame update
    void Start()
    {
        isWalking = false;
        isCrouching = false;
        layerMask = ~layerMask;
        GM = FindObjectOfType<GameManager>();
        getFlag = false;
        getGoal = false;
        isShooting = false;
        flagSitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Debug.Log("You Died");
            //respawn?
            if (getFlag)
            {
                flagHolder.transform.position = gameObject.transform.position;
                flagSitting = true;
            }
            GM.PlayerDied();
            getFlag = false;
            flagHolder.SetActive(true);
            health = 25;
        }
        if(firstPersonController.desiredMove != Vector3.zero)
        {
            isWalking = true;
            
        }
        else
        {
            isWalking = false;
        }
        if (isWalking)
        {
            GM.gunPlaces[GM.HoldingGun].GetComponent<Animator>().SetBool("walking", true);
        }
        else
        {
            GM.gunPlaces[GM.HoldingGun].GetComponent<Animator>().SetBool("walking", false);
        }
        if (flagSitting)
        {
            Debug.Log("Flag still in play!");
            flagToSpawn -= Time.deltaTime;
            if(flagToSpawn < 0)
            {
                flagSitting = false;
                flagToSpawn = 15f;
                flagHolder.transform.position = flagSpawn.transform.position;
            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Is firing");
            isShooting = true;
        }
        if (isShooting && Time.time >= fireTime)
        {
            fireTime = Time.time + 1f / fireRate;
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            if (isCrouching)
            {
                mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y - 1, mainCam.transform.position.z);
                characterController.height /= 2;
                //characterController.center = new Vector3(0, -.5f, 0);
            }
            else
            {
                mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + 1, mainCam.transform.position.z);
                //characterController.center = new Vector3(0, 0, 0);
                characterController.height *= 2;
            }
        }
        if(health < maxHealth)
        {
            timeforHealthRegain -= Time.deltaTime;
            if(timeforHealthRegain <= 0)
            {
                health += Time.deltaTime * 5;
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Flag")
        {
            getFlag = true;
            flagSitting = false;
            flagToSpawn = 15f;
            other.gameObject.SetActive(false);
        }
        if(other.tag == "Goal" && getFlag == true)
        {
            getGoal = true;
            //You win!
            SceneManager.LoadScene(0);
        }
        if(other.tag == "Ammo")
        {
            GM.AmmoNum += 10;
            StartCoroutine(RespawnAmmo(other.gameObject));
            other.gameObject.SetActive(false);
        }
        if(other.tag == "Gun")
        {
            //Unlocked new gun!
            other.gameObject.SetActive(false);
            GM.NewGun(other.gameObject.name);
        }
    } 

    void Fire()
    {
        GM.gunPlaces[GM.HoldingGun].GetComponent<Animator>().SetTrigger("shooting");
        GM.AmmoNum--;
        //Debug.Log("Pew pew");
        AudioClip AD = GM.gunSoundsPlaces[GM.HoldingGun];
        GM.playersource.PlayOneShot(AD);
        //do fancy particles and audio
        RaycastHit hit;
        if(Physics.Raycast(mainCam.position, mainCam.forward, out hit, (GM.gunDamage[GM.HoldingGun] * 6), layerMask))
        {
            //Debug.DrawRay(mainCam.position, mainCam.forward, Color.red, Mathf.Infinity);
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.tag == "Enemy")
                {
                    EnemyMovement em = hit.collider.gameObject.GetComponent<EnemyMovement>();
                    Debug.Log("Hit enemy!");
                    //hit.collider.gameObject.GetComponent<EnemyMovement>().enemyHealth -= 4;
                    em.enemyHealth -= GM.gunDamage[GM.HoldingGun];
                }
                else
                {
                    //Debug.Log("Hit a wall");
                    /*Debug.Log(hit.collider.gameObject.name);
                    Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
                    GameObject bH = Instantiate(bulletHole, hit.point, impactRotation);
                    Destroy(bH, 5f);*/
                }
            }
        }

        isShooting = false;
    }
    IEnumerator RespawnAmmo(GameObject ammoRes)
    {
        yield return new WaitForSeconds(8f);
        ammoRes.SetActive(true);
    }
}
