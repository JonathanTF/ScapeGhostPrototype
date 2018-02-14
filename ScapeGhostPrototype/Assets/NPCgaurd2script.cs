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
    public GameObject crazy;
    private NPCroutine npc;
    public GameObject mat1;
    public GameObject mat2;
    public GameObject gaurd1;
    public GameObject cellCollider;
    public bool stage1End = false;
    private NavMeshAgent _agent;

    // Use this for initialization
    void Start () {
        npc = GetComponent<NPCroutine>();
        _agent = gameObject.GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void stage1()
    {
        StartCoroutine(getKeyRoutine());
    }

    private bool b = false;
    public void FixedUpdate()
    {

        if(cellCollider.GetComponent<CellScript>().hasCrazy && crazy.GetComponent<crazyScript>().fighting && !b)
        {
            b = true;
            startStage2CrazyRoutine();
        }
    }

    IEnumerator getKeyRoutine()
    {
        NPCroutine myRoutine = gameObject.GetComponent<NPCroutine>();
        _agent.speed = 10;
        yield return StartCoroutine(npc.goToLocator(l2.gameObject, npc));
        yield return StartCoroutine(npc.goToLocator(l3.gameObject, npc));
        npc.matInteract(mat1);
        yield return StartCoroutine(npc.goToLocator(l2.gameObject, npc));
        yield return StartCoroutine(npc.goToLocator(l1.gameObject, npc));
        npc.matInteract(mat2);
        npc.Vspeed = 5;
        while (!stage1End)
        {
            yield return new WaitForSeconds(0.2f);
        }
        npc.matInteract(mat2);
        yield return StartCoroutine(npc.goToLocator(l2.gameObject, npc));
        yield return StartCoroutine(npc.goToLocator(l3.gameObject, npc));
        npc.matInteract(mat1);
        yield return StartCoroutine(npc.goToLocator(l2.gameObject, npc));
        yield return StartCoroutine(npc.goToLocator(l1.gameObject, npc));
        yield return null;
    }

    public void startStage2CrazyRoutine()
    {
        StopAllCoroutines();
        StartCoroutine(stage2CrazyRoutine());
    }

    IEnumerator stage2CrazyRoutine()
    {
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
    }
}
