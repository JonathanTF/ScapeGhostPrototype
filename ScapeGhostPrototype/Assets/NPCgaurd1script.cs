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
    public GameObject mat1;
    private NPCroutine npc;
    public GameObject crazyPrisoner;
    public GameObject gaurd2;
    bool stdWalk = true;
    private bool atL1 = false;
    bool hitCrazy = false;

    // Use this for initialization
    void Start () {
        npc = GetComponent<NPCroutine>();
        StartCoroutine(fixedRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.SqrMagnitude(npc.transform.position - l1.transform.position) < 0.5f)
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
        npc.Vspeed = 10;
        npc._agent.speed = 10;
        NPCroutine myRoutine = gameObject.GetComponent<NPCroutine>();

        yield return StartCoroutine(myRoutine.goToLocator(l2, npc));
        yield return new WaitForSeconds(3.0f);

        npc.matInteract(mat1);

        //yield return StartCoroutine(myRoutine.goToLocator(l4, npc));

        //npc.setTargetLoc(new Vector2(crazyPrisoner.gameObject.transform.position.x, crazyPrisoner.gameObject.transform.position.z));
        //while (!hitCrazy)
        //{

        //    yield return new WaitForSeconds(0.1f);
        //    npc.setTargetLoc(new Vector2(crazyPrisoner.gameObject.transform.position.x, crazyPrisoner.gameObject.transform.position.z));
        //}
        yield return StartCoroutine(myRoutine.goToLocator(crazyPrisoner, npc));

        hitCrazy = false;
        //crazyPrisoner.transform.parent = gameObject.transform;
        StartCoroutine(crazyPrisoner.GetComponent<NPCroutine>().handcuffTo(npc.gameObject));
        print("PARENTED!");
        crazyPrisoner.GetComponent<crazyScript>().spreadFear();
        yield return new WaitForSeconds(1.0f);

        print("RETURNING...");

       // yield return StartCoroutine(myRoutine.goToLocator(l4, npc));

       // yield return StartCoroutine(myRoutine.goToLocator(l6, npc));

       // yield return StartCoroutine(myRoutine.goToLocator(l6, npc));

       // yield return StartCoroutine(myRoutine.goToLocator(l2, npc));

        yield return StartCoroutine(myRoutine.goToLocator(l3, npc));

        crazyPrisoner.GetComponent<NPCroutine>().setTargetLoc(new Vector2(crazyPrisoner.transform.position.x, crazyPrisoner.transform.position.z));
        crazyPrisoner.GetComponent<NPCroutine>().uncuff();
        //yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(myRoutine.goToLocator(l2, npc));

        yield return new WaitForSeconds(3.0f);
        npc.matInteract(mat1);
        gaurd2.GetComponent<NPCgaurd2script>().stage1End = true;
        crazyPrisoner.GetComponent<crazyScript>().stage1 = false;
        npc.Vspeed = 5;
        npc._agent.speed = 5;
        stdWalk = true;


        StartCoroutine(fixedRoutine());
        yield return null;
    }

    IEnumerator fixedRoutine()
    {
        for (; ; )
        {
            if (stdWalk)
            {
                yield return StartCoroutine(gameObject.GetComponent<NPCroutine>().goToLocator(l1, npc));
                yield return new WaitForSeconds(16.0f);
                yield return StartCoroutine(gameObject.GetComponent<NPCroutine>().goToLocator(l2, npc));
                yield return new WaitForSeconds(3.0f);
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
