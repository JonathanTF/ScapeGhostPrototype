using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crazyScript : MonoBehaviour {
    public GameObject prisoner1;
    public GameObject prisoner2;
    public GameObject prisoner3;
    public GameObject prisoner4;
    public GameObject prisoner5;
    public GameObject prisoner6;
    public GameObject cellLocator;
    public Text crazyText;

    public NPCgaurd1script gaurd1;
    public NPCgaurd2script gaurd2;
    public bool stage1 = true;
    public bool fighting = false;

	// Use this for initialization
	void Start () {
        crazyText.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

	}

    private void FixedUpdate()
    {
        if (GetComponentInChildren<movement>())
        {
            crazyText.enabled = true;
        }
        else
        {
            crazyText.enabled = false;
        }
    }

    public void goCrazy(GameObject otherNPC)
    {
        gameObject.GetComponent<NPCroutine>().fight();
        otherNPC.GetComponent<NPCroutine>().fight();
        StartCoroutine(crazyTime(otherNPC));
    }

    public void spreadFear()
    {
        prisoner1.GetComponent<NPCroutine>().runReturnToStart();
        prisoner2.GetComponent<NPCroutine>().runReturnToStart();
        prisoner3.GetComponent<NPCroutine>().runReturnToStart();
        prisoner4.GetComponent<NPCroutine>().runReturnToStart();
        prisoner5.GetComponent<NPCroutine>().runReturnToStart();
        prisoner6.GetComponent<NPCroutine>().runReturnToStart();
    }

    IEnumerator crazyTime(GameObject otherNPC)
    {
        bool b = false;
        fighting = true;
        //otherNPC.GetComponent<NPCroutine>().StartCoroutine(otherNPC.GetComponent<NPCroutine>().goToLocator(gameObject, otherNPC.GetComponent<NPCroutine>()));
        //yeildgameObject.GetComponent<NPCroutine>()
        //StartCoroutine(otherNPC.GetComponent<NPCroutine>().goToLocator(gameObject, otherNPC.GetComponent<NPCroutine>()));
        yield return StartCoroutine(gameObject.GetComponent<NPCroutine>().goToLocator(otherNPC, gameObject.GetComponent<NPCroutine>()));
        yield return otherNPC.GetComponent<NPCroutine>().StartCoroutine(otherNPC.GetComponent<NPCroutine>().goToLocator(gameObject, otherNPC.GetComponent<NPCroutine>()));

        for (int i = 0; i < 12; i++)
        {
            gameObject.GetComponent<NPCroutine>().target_loc = otherNPC.transform.position;
            yield return StartCoroutine(gameObject.GetComponent<NPCroutine>().goToLocator(otherNPC, gameObject.GetComponent<NPCroutine>()));

            if (gaurd1.getAtL1() && stage1)
            {
                gaurd2.stage1();
                gaurd1.dealWithCrazy();
                //yield return StartCoroutine(gaurd1.getKeyRoutine());

                print("stage 1 started");
                b = true;

            }

            if (gameObject.GetComponent<NPCroutine>().cuffed)
            {
                break;
            }
            yield return new WaitForSeconds(1.0f);
            print(i+"th iteration");
        }
        fighting = false;
        //if(b == false)
        //{
            otherNPC.GetComponent<NPCroutine>().stopFight();
            gameObject.GetComponent<NPCroutine>().stopFight();
            
            print("stopping the fight");
        //}

        yield return new WaitForSeconds(20.0f);
        print("returning from crazyTime");

        
        yield return null;
    }
}
