using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCgaurd3script : MonoBehaviour {

    private NPCroutine npc;
    public GameObject mat1;
    public GameObject mat2;
    public GameObject gaurd1;
    //public GameObject cellCollider;
    public bool stage1End = false;
    private NavMeshAgent _agent;
    public GameObject gaurd2;
    public GameObject gaurd3;
    public doorScript bottomDoor;
    public doorScript topDoor;
    public doorScript cellDoor;
    public GameObject innerCellLocator1;
    public GameObject innerCellLocator2;
    public GameObject breakoutTarget;
    public Vector3 start_loc;
    public GameObject starter;
    public doorScriptGaurd3 myDoor;

    // Use this for initialization
    void Start () {
        npc = GetComponent<NPCroutine>();
        _agent = gameObject.GetComponent<NavMeshAgent>();
        start_loc = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool haveTarget = false;
    public bool readyRelease = false;

    public IEnumerator breakout()
    {
        haveTarget = false;
        myDoor.interact(npc);
        myDoor.disableInteract = true;
        //stdWalk = false;
        npc._agent.speed = 10;
        NPCroutine myRoutine = gameObject.GetComponent<NPCroutine>();
        yield return StartCoroutine(myRoutine.goToLocator(breakoutTarget, npc));
        StartCoroutine(breakoutTarget.GetComponent<NPCroutine>().handcuffTo(npc.gameObject));
        haveTarget = true;
        while (!gaurd1.GetComponent<NPCgaurd1script>().haveTarget)
        {
            yield return new WaitForSeconds(0.2f);
        }
        while (!gaurd2.GetComponent<NPCgaurd2script>().haveTarget)
        {
            yield return new WaitForSeconds(0.2f);
        }
        if (myRoutine.ownKey)
        {
            //i have the key
            yield return StartCoroutine(myRoutine.goToLocator(innerCellLocator1, npc));
            if (topDoor.locked)
            {
                topDoor.interact(npc);
            }
            yield return StartCoroutine(myRoutine.goToLocator(innerCellLocator2, npc));
            readyRelease = true;
        }
        else
        {
            yield return new WaitForSeconds(1.0f);//wait for another gaurd to deal with key
            yield return StartCoroutine(myRoutine.goToLocator(innerCellLocator2, npc));
            readyRelease = true;
        }
        while (!gaurd1.GetComponent<NPCgaurd1script>().readyRelease)
        {
            yield return new WaitForSeconds(0.2f);
        }
        while (!gaurd2.GetComponent<NPCgaurd2script>().readyRelease)
        {
            yield return new WaitForSeconds(0.2f);
        }
        breakoutTarget.GetComponent<NPCroutine>().uncuff();
        

        npc._agent.speed = 5;
        yield return StartCoroutine(myRoutine.goToLocator(starter, npc));
        
        myDoor.disableInteract = false;
        myDoor.interact(npc);
        haveTarget = false;
        readyRelease = false;
        //stdWalk = true;
    }

}
