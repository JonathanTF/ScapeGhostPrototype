using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalRunnerScript : MonoBehaviour {

    public doorScript gate;
    public bool hold = true;
    public GameObject runToLocator;
    public GameObject secondaryLocator;
    public GameObject assignedGaurd;
    private NPCroutine npc;
    public colliderIsTouching b1;
    public colliderIsTouching b2;
    public GameObject starter;
    public doorScript d1;

	// Use this for initialization
	void Start () {
        npc = gameObject.GetComponent<NPCroutine>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (hold)
        {
            if (gate.locked == false)
            {
                //initiate the attempted breakout
                hold = false;
                StartCoroutine(breakout());
            }
        }
    }

    public IEnumerator breakout()
    {
        gate.disableInteract = true;

        float old_s = npc._agent.speed;

        yield return StartCoroutine(npc.goToLocator(runToLocator, npc));

        if(assignedGaurd.GetComponent<NPCgaurd1script>() != null)
        {
            StartCoroutine(assignedGaurd.GetComponent<NPCgaurd1script>().breakout());
        }
        else if(assignedGaurd.GetComponent<NPCgaurd2script>() != null) {
            StartCoroutine(assignedGaurd.GetComponent<NPCgaurd2script>().breakout());
        }
        else
        {
            StartCoroutine(assignedGaurd.GetComponent<NPCgaurd3script>().breakout());
        }

        //yield return StartCoroutine(npc.goToLocator(secondaryLocator, npc));
        npc._agent.isStopped = true;
        //yield return new WaitForSeconds(20.0f);
        yield return StartCoroutine(waitForGaurd());
        npc._agent.speed = old_s;

        yield return StartCoroutine(npc.goToLocator(starter, npc));

        hold = true;

    }

    public IEnumerator waitForGaurd()
    {
        while (!d1.locked)
        {
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }

        public IEnumerator breakoutCheckTimer()
    {
        yield return new WaitForSeconds(2.0f);

        if(gate.locked && !b1.doYouHave(gameObject) && !b2.doYouHave(gameObject)){
            hold = true;
            StopAllCoroutines();
        }

        yield return null;
    }
}
