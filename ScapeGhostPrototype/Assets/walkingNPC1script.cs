using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingNPC1script : MonoBehaviour {

    public GameObject l1;
    public GameObject l2;
    public GameObject l3;
    public GameObject l4;
    private NPCroutine npc;

    public bool stopped = false;

    // Use this for initialization
    void Start() {
        npc = GetComponent<NPCroutine>();
        StartCoroutine(fixedRoutine());
    }

    // Update is called once per frame
    void Update() {

    }

    public void setStopped(bool b)
    {
        stopped = b;
    }

    IEnumerator fixedRoutine()
        {
        for (; ; )
        {
            
            yield return StartCoroutine(npc.goToLocator(l1, npc));
            //yield return new WaitForSeconds(3.0f);
            yield return StartCoroutine(npc.goToLocator(l2, npc));
            yield return new WaitForSeconds(4.0f);
            yield return StartCoroutine(npc.goToLocator(l3, npc));
            //yield return new WaitForSeconds(4.0f);
            yield return StartCoroutine(npc.goToLocator(l4, npc));
            yield return new WaitForSeconds(6.0f);
            //if (!stopped)
            //{
            //    npc.setTargetLoc(new Vector2(l4.gameObject.transform.position.x, l4.gameObject.transform.position.z));
            //    yield return new WaitForSeconds(9.0f);
            //}
            //if (!stopped)
            //{
            //    npc.setTargetLoc(new Vector2(l1.gameObject.transform.position.x, l1.gameObject.transform.position.z));
            //    yield return new WaitForSeconds(3.0f);
            //}
            //if (!stopped)
            //{
            //    npc.setTargetLoc(new Vector2(l2.gameObject.transform.position.x, l2.gameObject.transform.position.z));
            //    yield return new WaitForSeconds(8.0f);
            //}
            //if (!stopped)
            //{
            //    npc.setTargetLoc(new Vector2(l3.gameObject.transform.position.x, l3.gameObject.transform.position.z));
            //    yield return new WaitForSeconds(3.5f);
            //}
        }
    }

}
