using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScriptGaurd3 : MonoBehaviour
{
    public GameObject lockSignal;
    public GameObject holder;
    public GameObject gate;
    private Collider detector;
    public bool locked = true;
    public bool disableInteract = false;
    public Material lockedMat;
    public Material unlockedMat;
    // Use this for initialization
    void Start()
    {
        locked = true;
        detector = GetComponent<BoxCollider>();
        detector.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public bool interact(NPCroutine npc)
    {
        if (disableInteract)
        {
            return false;
        }
        //going to pretend that it's like gaurd 3 has a unique key for this door, but really he's the only one who can unlock it
        if (npc.GetComponent<NPCgaurd3script>() != null)
        {
            if (locked)
            {
                detector.enabled = true;
                locked = false;
                lockSignal.GetComponent<MeshRenderer>().material = unlockedMat;
            }
            else
            {
                gate.transform.position = gameObject.transform.position;
                detector.enabled = false;
                locked = true;
                lockSignal.GetComponent<MeshRenderer>().material = lockedMat;
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
        print("hi");
        if (other.gameObject.tag.Equals("NPC"))
        {
            if (!locked)
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
            if (!locked)
            {
                gate.transform.position = gameObject.transform.position;
            }
            //print("bringing back gate");
        }
    }

}
