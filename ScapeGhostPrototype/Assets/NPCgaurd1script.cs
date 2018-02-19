using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCgaurd1script : MonoBehaviour {

    public GameObject l1;
    public GameObject l2;
    public GameObject l3;
    public GameObject l4;
    public GameObject l5;
    public GameObject l6;
    public GameObject l7;
    public GameObject l8;
    public GameObject breakoutTarget;
    public GameObject mat1;
    private NPCroutine npc;
    public GameObject crazyPrisoner;
    public GameObject gaurd2;
    public GameObject gaurd3;
    public doorScript bottomDoor;
    public doorScript topDoor;
    public doorScript cellDoor;
    public GameObject innerCellLocator1;
    public GameObject innerCellLocator2;


    bool stdWalk = true;
    private bool atL1 = false;
    bool hitCrazy = false;

    // Use this for initialization
    void Start () {
        npc = GetComponent<NPCroutine>();
        //StartCoroutine(fixedRoutine());
        cellDoor.disableInteract = true;
        StartCoroutine(starterRoutine());
    }



    // Update is called once per frame
    void Update()
    {
        if (Vector3.SqrMagnitude(npc.transform.position - l8.transform.position) < 0.5f)
        {
            atL1 = true;
        }
        else
        {
            atL1 = false;
        }
    }

    public bool getAtL1()
    {
        return atL1;
    }

    public void dealWithCrazy()
    {
        StopAllCoroutines();
        StartCoroutine(getKeyRoutine());
        print("dealwithcrazy called");
    }

    public IEnumerator getKeyRoutine()
    {
       

        stdWalk = false;
        npc.Vspeed = 5;
        npc._agent.speed = 10;
        NPCroutine myRoutine = gameObject.GetComponent<NPCroutine>();

        bottomDoor.disableInteract = true;

        yield return StartCoroutine(myRoutine.goToLocator(l2, npc));
        while (!mat1.GetComponent<keyMatScript>().holdsKey())
        {
            yield return new WaitForSeconds(0.2f);
        }
        

        npc.matInteract(mat1);

        yield return StartCoroutine(myRoutine.goToLocator(l6, npc));
        bottomDoor.disableInteract = false;
        bottomDoor.interact(npc);
        bottomDoor.disableInteract = true;
        yield return StartCoroutine(myRoutine.goToLocator(l4, npc));
        bottomDoor.disableInteract = false;
        bottomDoor.interact(npc);
        bottomDoor.disableInteract = true;
        yield return StartCoroutine(myRoutine.goToLocator(crazyPrisoner, npc));

        hitCrazy = false;


        StartCoroutine(crazyPrisoner.GetComponent<NPCroutine>().handcuffTo(npc.gameObject));
        print("PARENTED!");
        crazyPrisoner.GetComponent<crazyScript>().spreadFear();
        npc._agent.speed = 10;
        yield return new WaitForSeconds(1.0f);

        print("RETURNING...");
        yield return StartCoroutine(myRoutine.goToLocator(l4, npc));
        bottomDoor.disableInteract = false;
        bottomDoor.interact(npc);
        bottomDoor.disableInteract = true;
        yield return StartCoroutine(myRoutine.goToLocator(l6, npc));
        bottomDoor.disableInteract = false;
        bottomDoor.interact(npc);
        bottomDoor.disableInteract = true;

        cellDoor.disableInteract = true;
        yield return StartCoroutine(myRoutine.goToLocator(l7, npc));
        cellDoor.disableInteract = false;
        cellDoor.interact(npc);
        cellDoor.disableInteract =  true;

        yield return StartCoroutine(myRoutine.goToLocator(l3, npc));

        crazyPrisoner.GetComponent<NPCroutine>().start_loc = (crazyPrisoner.transform.position);
        crazyPrisoner.GetComponent<NPCroutine>().setTargetLoc(crazyPrisoner.transform.position);

        crazyPrisoner.GetComponent<NPCroutine>().uncuff();

        yield return StartCoroutine(myRoutine.goToLocator(l7, npc));
        cellDoor.disableInteract = false;
        cellDoor.interact(npc);
        cellDoor.disableInteract = true;

        bottomDoor.disableInteract = false;

        yield return StartCoroutine(myRoutine.goToLocator(l2, npc));

        yield return new WaitForSeconds(1.0f);
        npc.matInteract(mat1);
        gaurd2.GetComponent<NPCgaurd2script>().stage1End = true;
        crazyPrisoner.GetComponent<crazyScript>().stage1 = false;
        npc.Vspeed = 5;
        npc._agent.speed = 5;
        stdWalk = true;


        StartCoroutine(fixedRoutine());
        yield return null;
    }

    public bool haveTarget = false;
    public bool readyRelease = false;

    public IEnumerator breakout()
    {
        print("starting breakout!");
        while(crazyPrisoner.GetComponent<crazyScript>().stage1 == true)
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (!npc.ownKey)
        {
            yield return StartCoroutine(npc.goToLocator(l2.gameObject, npc));
            npc.matInteract(mat1);
        }

        haveTarget = false;
        stdWalk = false;
        npc._agent.speed = 10;
        NPCroutine myRoutine = gameObject.GetComponent<NPCroutine>();
        yield return StartCoroutine(myRoutine.goToLocator(breakoutTarget, npc));
        StartCoroutine(breakoutTarget.GetComponent<NPCroutine>().handcuffTo(npc.gameObject));
        yield return StartCoroutine(myRoutine.goToLocator(breakoutTarget, npc));
        haveTarget = true;
        while (!gaurd2.GetComponent<NPCgaurd2script>().haveTarget)
        {
            yield return new WaitForSeconds(0.2f);
        }
        while (!gaurd3.GetComponent<NPCgaurd3script>().haveTarget)
        {
            yield return new WaitForSeconds(0.2f);
        }
        print("we got em!");

        //if (myRoutine.ownKey)
        //{
        //    //i have the key
        //    yield return StartCoroutine(myRoutine.goToLocator(innerCellLocator1, npc));
        //    if (topDoor.locked)
        //    {
        //        topDoor.interact(npc);
        //    }
        //    yield return StartCoroutine(myRoutine.goToLocator(innerCellLocator2, npc));
        //    readyRelease = true;
        //}
        //else
        //{
            yield return new WaitForSeconds(1.0f);//wait for another gaurd to deal with key
            yield return StartCoroutine(myRoutine.goToLocator(innerCellLocator2, npc));
            readyRelease = true;
        //}
        while (!gaurd2.GetComponent<NPCgaurd2script>().readyRelease)
        {
            yield return new WaitForSeconds(0.2f);
        }
        while (!gaurd3.GetComponent<NPCgaurd3script>().readyRelease)
        {
            yield return new WaitForSeconds(0.2f);
        }

        print("we placed em!");

        breakoutTarget.GetComponent<NPCroutine>().uncuff();

        npc._agent.speed = 5;
        stdWalk = true;
        yield return StartCoroutine(gameObject.GetComponent<NPCroutine>().goToLocator(l1, npc));
        topDoor.disableInteract = false;
        topDoor.interact(npc);
        haveTarget = false;
        readyRelease = false;
        StartCoroutine(fixedRoutine());
        
    }

    IEnumerator starterRoutine()
    {
        yield return npc.goToLocator(l8.gameObject, npc);
        yield return null;
    }

    IEnumerator fixedRoutine()
    {
        for (; ; )
        {
            if (stdWalk)
            {
                yield return StartCoroutine(gameObject.GetComponent<NPCroutine>().goToLocator(l1, npc));
                if(!bottomDoor.locked && npc.ownKey)
                {
                    yield return StartCoroutine(gameObject.GetComponent<NPCroutine>().goToLocator(l6, npc));
                    bottomDoor.interact(npc);
                    yield return StartCoroutine(gameObject.GetComponent<NPCroutine>().goToLocator(l1, npc));
                }
                yield return new WaitForSeconds(4.0f);
                yield return StartCoroutine(gameObject.GetComponent<NPCroutine>().goToLocator(l2, npc));
                yield return new WaitForSeconds(4.0f);
            }
            else
            {
               yield return new WaitForSeconds(0.5f); 
            }
            

        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //print("hit something");
        if(hit.collider.gameObject.GetComponent<crazyScript>() != null)
        {
            hitCrazy = true;
           // print("hit crazy");
        }
    }

}
