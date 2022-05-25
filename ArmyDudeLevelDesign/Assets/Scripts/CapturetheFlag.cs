using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CapturetheFlag : MonoBehaviour
{
    public GameManager GM;
    bool getFlag = false;
    bool getGoal = false;
    bool isShooting;
    public Transform mainCam;
    float fireTime = 0;
    int fireRate = 3;
    int layerMask = 1 << 8;
    bool isCrouching;
    public CharacterController characterController;
    //List<GameObject> ammoList = new List<GameObject>();
    public int health = 25;

    // Start is called before the first frame update
    void Start()
    {
        isCrouching = false;
        layerMask = ~layerMask;
        GM = FindObjectOfType<GameManager>();
        getFlag = false;
        getGoal = false;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Debug.Log("You Died");
            //respawn?
            GM.PlayerDied();
            health = 25;
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
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Flag")
        {
            getFlag = true;
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
        GM.AmmoNum--;
        Debug.Log("Pew pew");
        AudioClip AD = GM.gunSoundsPlaces[GM.HoldingGun];
        GM.playersource.PlayOneShot(AD);
        //do fancy particles and audio
        RaycastHit hit;
        if(Physics.Raycast(mainCam.position, mainCam.forward, out hit, 50, layerMask))
        {
            //Debug.DrawRay(mainCam.position, mainCam.forward, Color.red, Mathf.Infinity);
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.tag == "Enemy")
                {
                    Debug.Log("Hit enemy!");
                    hit.collider.gameObject.GetComponent<EnemyMovement>().enemyHealth -= 4;
                }
                else
                {
                    Debug.Log("Hit a wall");
                    Debug.Log(hit.collider.gameObject.name);
                    //Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
                    //GameObject bH = Instantiate(bulletHole, hit.point, impactRotation);
                    //Destroy(bH, 5f);
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
