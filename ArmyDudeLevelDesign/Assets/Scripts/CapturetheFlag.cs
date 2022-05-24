using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CapturetheFlag : MonoBehaviour
{
    public GameManager GM;
    bool getFlag = false;
    bool getGoal = false;
    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        getFlag = false;
        getGoal = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
            other.gameObject.SetActive(false);
        }
        if(other.tag == "Gun")
        {
            //Unlocked new gun!
        }
    } 

}
