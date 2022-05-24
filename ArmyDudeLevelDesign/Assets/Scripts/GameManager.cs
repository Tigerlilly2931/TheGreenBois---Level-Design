using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int AmmoNum = 30;
    [SerializeField] string[] CurrentGunType = new string[3];
    [SerializeField] Text ammoTxt;
    [SerializeField] Text currentGunTxt;
    int HoldingGun = 0;
    public bool PickedUpGun2;
    public bool PickedUpGun3;
    // Start is called before the first frame update
    void Start()
    {
        AmmoNum = 30;
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
    }

    private void LateUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if(!PickedUpGun2 && !PickedUpGun3)
            {
                //no other guns, no rotate
                return;
            }
            HoldingGun++;
            if(HoldingGun > 2)
            {
                HoldingGun = 0;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (!PickedUpGun2 && !PickedUpGun3)
            {
                //no other guns, no rotate
                return;
            }
            HoldingGun--;
            if(HoldingGun < 0)
            {
                HoldingGun = 2;
            }
        }
    }

}
