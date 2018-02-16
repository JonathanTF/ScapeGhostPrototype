using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour {

    public GameObject crazy;
    public bool hasCrazy;
    public GameObject cellPrisoner;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(crazy))
        {
            hasCrazy = true;
            print("CRAZY ACQUIRED");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(crazy))
        {
            hasCrazy = false;
        }else if (other.gameObject.Equals(cellPrisoner))
        {
            //alert gaurds
        }
    }
}
