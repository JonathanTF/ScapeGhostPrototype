using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour {

    public GameObject holder;
    public GameObject gate;
    private Collider detector;
    public bool locked = true;
    public bool disableInteract = false;

	// Use this for initialization
	void Start () {
        locked = true;
        detector = GetComponent<BoxCollider>();
        detector.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {


    }

    public bool interact(NPCroutine npc)
    {
        if (disableInteract)
        {
            return false;
        }

        if (npc.ownKey)
        {
            if (locked)
            {
                detector.enabled = true;
                locked = false;
            }
            else
            {
                gate.transform.position = gameObject.transform.position;
                detector.enabled = false;
                locked = true;
            }
            return true;
        }
        else
        {
            return false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("NPC"))
        {
            if (!locked)
            {
                gate.transform.position = holder.transform.position;
            }
            //print("removing gate");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("NPC"))
        {
            if (!locked)
            {
                gate.transform.position = gameObject.transform.position;
            }
            //print("bringing back gate");
        }  
    }

}
