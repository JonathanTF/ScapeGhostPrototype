using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingNPC1script : MonoBehaviour {

    public GameObject l1;
    public GameObject l2;
    public GameObject l3;
    public GameObject l4;
    private NPCroutine npc;
    public penTracker pt;
    public GameObject crazyMans;

    public bool stopped = false;

    // Use this for initialization
    void Start() {
        npc = GetComponent<NPCroutine>();
        StartCoroutine(fixedRoutine());
    }

    // Update is called once per frame
    void Update() {

    }

    bool move = true;

    public void FixedUpdate()
    {
        if (pt.has(crazyMans))
        {
            move = true;
        }
        else
        {
            move = false;
        }

        if (npc.fighting && !stopped)
        {
            print("STOPPING ALL COROUTINES");
            StopAllCoroutines();
            //npc.StopCoroutine("goToLocator");
            StopCoroutine(npc.goToLocator(crazyMans, npc));
            stopped = true;
        }else if(stopped && !npc.fighting)
        {
            stopped = false;
            StartCoroutine(fixedRoutine());
        }
    }

    public void setStopped(bool b)
    {
        stopped = b;
    }

    public void _stop()
    {
        StopAllCoroutines();
    }

    public void _continue()
    {
        StartCoroutine(fixedRoutine());
    }


    IEnumerator fixedRoutine()
        {
        yield return StartCoroutine(npc.goToLocator(l4, npc));
        for (; ; )
        {
            if (move)
            {
                yield return StartCoroutine(npc.goToLocator(l1, npc));
                yield return StartCoroutine(npc.goToLocator(l2, npc));
                yield return new WaitForSeconds(4.0f);
                yield return StartCoroutine(npc.goToLocator(l3, npc));
                yield return StartCoroutine(npc.goToLocator(l4, npc));
                yield return new WaitForSeconds(6.0f);
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

}
