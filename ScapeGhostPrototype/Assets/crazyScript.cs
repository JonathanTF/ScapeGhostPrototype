using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crazyScript : MonoBehaviour {
    public GameObject prisoner1;
    public GameObject prisoner2;
    public GameObject prisoner3;
    public GameObject prisoner4;
    public GameObject prisoner5;
    public GameObject prisoner6;

    public NPCgaurd1script gaurd1;
    public NPCgaurd2script gaurd2;
    public bool stage1 = true;
    public bool fighting = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

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
        for (int i = 0; i < 12; i++)
        {
            
            if (gaurd1.getAtL1() && stage1)
            {
                
                gaurd2.stage1();
                gaurd1.dealWithCrazy();
                //yield return StartCoroutine(gaurd1.getKeyRoutine());
                
                print("stage 1 started");
                b = true;
                
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
        fighting = false;
        if(b == false)
        {
            gameObject.GetComponent<NPCroutine>().stopFight();
            otherNPC.GetComponent<NPCroutine>().stopFight();
            print("stopping the fight");
        }

        yield return new WaitForSeconds(20.0f);
        print("returning from crazyTime");

        
        yield return null;
    }
}
