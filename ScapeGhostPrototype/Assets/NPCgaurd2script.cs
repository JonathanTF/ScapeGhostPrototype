using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class NPCgaurd2script : MonoBehaviour {

    public GameObject l1;
    public GameObject l2;
    public GameObject l3;
    public GameObject l4;
    public GameObject l5;
    public GameObject l6;
    public GameObject l7;
    public GameObject crazy;
    private NPCroutine npc;
    public GameObject mat1;
    public GameObject mat2;
    public GameObject gaurd1;
    public GameObject cellCollider;
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
    public penTracker pt;
    public GameObject ghostie;

    // Use this for initialization
    void Start () {
        npc = GetComponent<NPCroutine>();
        _agent = gameObject.GetComponent<NavMeshAgent>();
        StartCoroutine(standardRoutine());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void stage1()
    {
        StartCoroutine(getKeyRoutine());
    }

    private bool b = false;
    private bool c = false;
    public void FixedUpdate()
    {

        if(cellCollider.GetComponent<CellScript>().hasCrazy && crazy.GetComponent<crazyScript>().fighting && !b)
        {
            b = true;
            startStage2CrazyRoutine();
        }



        
    }

    public bool haveTarget = false;
    public bool readyRelease = false;

    public IEnumerator breakout()
    {
        haveTarget = false;
        stdWalk = false;
        npc._agent.speed = 10;
        NPCroutine myRoutine = gameObject.GetComponent<NPCroutine>();
        yield return StartCoroutine(myRoutine.goToLocator(breakoutTarget, npc));
        StartCoroutine(breakoutTarget.GetComponent<NPCroutine>().handcuffTo(npc.gameObject));
        haveTarget = true;
        while (!gaurd1.GetComponent<NPCgaurd1script>().haveTarget)
        {
            yield return new WaitForSeconds(0.2f);
        }
        while (!gaurd3.GetComponent<NPCgaurd3script>().haveTarget)
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
        while (!gaurd3.GetComponent<NPCgaurd3script>().readyRelease)
        {
            yield return new WaitForSeconds(0.2f);
        }
        breakoutTarget.GetComponent<NPCroutine>().uncuff();

        npc._agent.speed = 5;

        yield return StartCoroutine(myRoutine.goToLocator(l1, npc));
        haveTarget = false;
        readyRelease = false;
        stdWalk = true;
    }

    

    IEnumerator getKeyRoutine()
    {
        stdWalk = false;
        NPCroutine myRoutine = gameObject.GetComponent<NPCroutine>();
        _agent.speed = 10;
        yield return StartCoroutine(npc.goToLocator(l2.gameObject, npc));
        yield return StartCoroutine(npc.goToLocator(l3.gameObject, npc));

        while (!npc.ownKey)
        {
            npc.matInteract(mat1);
            yield return new WaitForSeconds(0.2f);
        }
        
        yield return StartCoroutine(npc.goToLocator(l2.gameObject, npc));
        yield return StartCoroutine(npc.goToLocator(l1.gameObject, npc));
        npc.matInteract(mat2);
        npc.Vspeed = 5;
        stdWalk = true;
        yield return null;
    }

    public void startStage2CrazyRoutine()
    {
        //StopAllCoroutines();
        StartCoroutine(stage2CrazyRoutine());
    }

    public bool stdWalk = true;

    IEnumerator standardRoutine()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(1.0f);

            if (!stdWalk)
            {
                continue;
            }
            //yield return StartCoroutine(npc.goToLocator(l1.gameObject, npc));
            Vector2 curr = new Vector2(transform.position.x, transform.position.z);
            Vector2 tar = new Vector2(l1.transform.position.x, l1.transform.position.z);
            if (mat2.GetComponent<keyMatScript>().holdsKey() && (Vector2.SqrMagnitude(tar - curr) < 3))
            {
                yield return new WaitForSeconds(3.0f); // TODO: building reaction
                curr = new Vector2(transform.position.x, transform.position.z);
                if (mat2.GetComponent<keyMatScript>().holdsKey() && stdWalk && (Vector2.SqrMagnitude(tar - curr) < 3))
                {
                    npc.matInteract(mat2);
                    yield return StartCoroutine(npc.goToLocator(l2.gameObject, npc));
                    yield return StartCoroutine(npc.goToLocator(l3.gameObject, npc));
                    npc.matInteract(mat1);
                    yield return StartCoroutine(npc.goToLocator(l2.gameObject, npc));
                    yield return StartCoroutine(npc.goToLocator(l1.gameObject, npc));
                }
            }

            
        }
    }


    IEnumerator stage2CrazyRoutine()
    {
        stdWalk = false;
        yield return StartCoroutine(npc.goToLocator(l4.gameObject, npc));
        yield return StartCoroutine(npc.goToLocator(l5.gameObject, npc));
        while (crazy.GetComponent<crazyScript>().fighting)
        {
            yield return new WaitForSeconds(1.0f);
        }
        yield return StartCoroutine(npc.goToLocator(l5.gameObject, npc));
        yield return StartCoroutine(npc.goToLocator(l4.gameObject, npc));
        yield return StartCoroutine(npc.goToLocator(l1.gameObject, npc));
        b = false;
        stdWalk = true;
        yield return null;
    }

    IEnumerator releaseCrazy()
    {
        stdWalk = false;
        yield return StartCoroutine(npc.goToLocator(l3.gameObject, npc));
        npc.matInteract(mat1);
        yield return StartCoroutine(npc.goToLocator(crazy, npc));
        yield return StartCoroutine(crazy.GetComponent<NPCroutine>().handcuffTo(gameObject));
        cellDoor.disableInteract = false;
        cellDoor.interact(npc);
        yield return StartCoroutine(npc.goToLocator(l7.gameObject, npc));
        yield return StartCoroutine(npc.goToLocator(l6.gameObject, npc));
        crazy.GetComponent<NPCroutine>().uncuff();
        crazy.GetComponent<crazyScript>().stage1 = true;
        yield return StartCoroutine(npc.goToLocator(l3.gameObject, npc));
        npc.matInteract(mat1);
        stdWalk = true;
        yield return null;
        c = false;
    }
}
