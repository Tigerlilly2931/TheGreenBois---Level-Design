using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int AmmoNum = 30;
    [SerializeField] string[] CurrentGunType = new string[4];
    [SerializeField] Text ammoTxt;
    [SerializeField] Text currentGunTxt;
    [SerializeField] Text playerHealthTxt;
    public GameObject player, canvastxt, specCam, raspawn;
    [SerializeField] GameObject Pistol, Sniper, SMG, Shotgun;
    Dictionary<int, GameObject> gunPlaces = new Dictionary<int, GameObject>();
    public Dictionary<int, AudioClip> gunSoundsPlaces = new Dictionary<int, AudioClip>();
    public int HoldingGun = 0;
    public int totalGuns = 0;
    public bool PickedUpGun2;
    public bool PickedUpGun3;
    float timetoRespawn = 5f;
    bool PlayerDeaed;

    [SerializeField] AudioClip pistolPew, sniperPow, SMGRattatat, shotgunBOOM;
    public AudioSource playersource;
    // Start is called before the first frame update
    void Start()
    {
        PlayerDeaed = false;
        player.SetActive(true);
        canvastxt.SetActive(false);
        specCam.SetActive(false);
        gunPlaces.Add(0, Pistol);
        gunSoundsPlaces.Add(0, pistolPew);
        AmmoNum = 30;
        totalGuns = 0;
        PickedUpGun2 = false;
        PickedUpGun3 = false;
        HoldingGun = 0;
        CurrentGunType[0] = "Default Pistol";
    }

    // Update is called once per frame
    void Update()
    {
        ammoTxt.text = "Ammo: " + AmmoNum;
        currentGunTxt.text = "Gun: " + CurrentGunType[HoldingGun];
        playerHealthTxt.text = "Health: " + (int)((float)player.GetComponent<CapturetheFlag>().health / 25f * 100f) + "%";
        if (PlayerDeaed)
        {
            timetoRespawn -= Time.deltaTime;
            canvastxt.GetComponent<Text>().text = "Respawn in: \n" + (int)timetoRespawn;
            if(timetoRespawn < 0)
            {
                PlayerDeaed = false;
                timetoRespawn = 5f;
                PlayerDeaed = false;
                canvastxt.SetActive(false);
                specCam.SetActive(false);
                player.SetActive(true);
                //canvas.SetActive(true);
            }
        }
        
    }

    private void LateUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            Debug.Log("Scrolling Forwards");
            /*if(totalGuns == 1)
            {
                //no other guns, no rotate
                return;
            }*/
            gunPlaces[HoldingGun].gameObject.SetActive(false);
            HoldingGun++;
            if(HoldingGun > totalGuns)
            {
                HoldingGun = 0;
            }
            gunPlaces[HoldingGun].gameObject.SetActive(true);
            /*switch (HoldingGun)
            {
                case 0:
                    {
                        Pistol.gameObject.SetActive(true);
                        Sniper.gameObject.SetActive(false);
                        SMG.gameObject.SetActive(false);
                        Shotgun.gameObject.SetActive(false);
                        break;
                    }
                case 1:
                    {
                        Pistol.gameObject.SetActive(false);
                        Sniper.gameObject.SetActive(true);
                        SMG.gameObject.SetActive(false);
                        Shotgun.gameObject.SetActive(false);
                        break;
                    }
                case 3:
                    {
                        Pistol.gameObject.SetActive(false);
                        Sniper.gameObject.SetActive(false);
                        SMG.gameObject.SetActive(true);
                        Shotgun.gameObject.SetActive(false);
                        break;
                    }
                case 4:
                    {
                        Pistol.gameObject.SetActive(false);
                        Sniper.gameObject.SetActive(false);
                        SMG.gameObject.SetActive(false);
                        Shotgun.gameObject.SetActive(true);
                        break;
                    }
            }*/
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            Debug.Log("Scrolling Backwards");
            /*if (totalGuns == 1)
            {
                //no other guns, no rotate
                return;
            }*/
            gunPlaces[HoldingGun].gameObject.SetActive(false);
            HoldingGun--;
            if(HoldingGun < 0)
            {
                HoldingGun = totalGuns;
            }
            gunPlaces[HoldingGun].gameObject.SetActive(true);
        }
    }
    public void NewGun(string GunName)
    {
        totalGuns++;
        CurrentGunType[totalGuns] = GunName;
        switch (GunName)
        {
            case "SMG":
                {
                    Debug.Log("Picked up the SMG");
                    gunPlaces.Add(totalGuns, SMG);
                    gunSoundsPlaces.Add(totalGuns, SMGRattatat);
                    break;
                }
            case "Shotgun":
                {
                    Debug.Log("Picked up the Shotgun");
                    gunPlaces.Add(totalGuns, Shotgun);
                    gunSoundsPlaces.Add(totalGuns, shotgunBOOM);
                    break;
                }
            case "Sniper":
                {
                    Debug.Log("Picked up the Sniper");
                    gunPlaces.Add(totalGuns, Sniper);
                    gunSoundsPlaces.Add(totalGuns, sniperPow);
                    break;
                }
            default:
                {
                    Debug.Log("You're not have gun. No");
                    //totalGuns--;
                    break;
                }
        }
        
        //gunPlaces.Add(totalGuns, GameObject.Find(GunName));
    }

    public void PlayerDied()
    {
        player.SetActive(false);
        canvastxt.SetActive(true);
        specCam.SetActive(true);
        PlayerDeaed = true;
        player.transform.position = raspawn.transform.position;
        player.transform.rotation = raspawn.transform.rotation;
    }

}
