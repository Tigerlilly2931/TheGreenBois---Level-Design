using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagIconLookatPlayer : MonoBehaviour
{
    public CapturetheFlag ctf;
    // Start is called before the first frame update
    void Start()
    {
        ctf = FindObjectOfType<CapturetheFlag>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(ctf.gameObject.transform.position);
    }
}
