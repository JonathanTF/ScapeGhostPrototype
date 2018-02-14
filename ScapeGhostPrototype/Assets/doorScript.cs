using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour {

    public GameObject holder;
    public GameObject gate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("NPC"))
        {
            if (other.gameObject.GetComponent<NPCroutine>().ownKey)
            {
                gate.transform.position = holder.transform.position;
            }
            print("removing gate");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("NPC"))
        {
            if (other.gameObject.GetComponent<NPCroutine>().ownKey)
            {
                gate.transform.position = gameObject.transform.position;
            }
            print("bringing back gate");
        }  
    }

}
