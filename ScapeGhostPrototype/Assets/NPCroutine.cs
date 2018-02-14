using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCroutine : MonoBehaviour {

    public float minResistance = 0;

    public bool ownKey = false;
    public GameObject key = null;
    public NavMeshAgent _agent;
    private CharacterController mover;
    public Vector3 target_loc;
    private bool control;
    public bool allowControl = false;
    public float resistance = 0;
    public float Vspeed = 5;
    private bool waitDelay = false;
    public bool skipUpdate = false;
    public Vector3 start_loc;

    // Use this for initialization
    void Start () {
        mover = gameObject.GetComponent<CharacterController>();
        StartCoroutine(resist());
        start_loc = transform.position;
        target_loc = start_loc;
        resistance = minResistance;
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _agent.angularSpeed = 0;
        _agent.speed = 5;
        _agent.acceleration = 50.0f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector2 getTarget()
    {
        return target_loc;
    }

    public bool getControl()
    {
        return control;
    }

    public void setControl(bool b)
    {
        control = b;
    }

    public bool getAllowControl()
    {
        return allowControl;
    }

    public void setAllowControl(bool b)
    {
        allowControl = b;
    }

    public void matInteract(GameObject mat)
    {
        if (mat.GetComponent<keyMatScript>().holdsKey())
        {
            ownKey = true;
            key = mat.GetComponent<keyMatScript>().takeKey();
        }else if (ownKey)
        {
            ownKey = false;
            mat.GetComponent<keyMatScript>().giveKey(key);
        }
    }

    IEnumerator resist()
    {
        for (; ; )
        {
            if (control)
            {
                resistance += .2f;
            }
            else
            {
                resistance -= .2f;
            }

            if (resistance < minResistance) resistance = minResistance;
            if (resistance > 1)
            {
                resistance = 1;
                StartCoroutine(ReturnToStart());
            }

            //print(resistance);

            yield return new WaitForSeconds(.5f);
        }
    }

    public void runReturnToStart()
    {
        resistance = 0.99f;
        StartCoroutine(ReturnToStart());
    }

    IEnumerator ReturnToStart()
    {
        allowControl = false;
        while (resistance > (minResistance+.1)) { yield return new WaitForSeconds(.5f); };
        if (gameObject.GetComponentInChildren<movement>() != null) { 
            allowControl = true;
        }
        yield return null;
    }


    float curSpeed = 0;
    float curSpeedS = 0;

    public void fight()
    {
        skipUpdate = true;
        allowControl = false;
        StartCoroutine(fightTimer());
    }

    public void skipAllUpdate()
    {
        skipUpdate = true;
    }

    public void stopFight()
    {
       if(gameObject.GetComponent<movement>() != null)
        {
            allowControl = true;
        }
        skipUpdate = false;
    }

    IEnumerator fightTimer()
    {
        yield return new WaitForSeconds(20.0f);
        stopFight();
        yield return null;
        
    }

    private void FixedUpdate()
    {

        if (skipUpdate)
        {
            return;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 sideways = transform.TransformDirection(Vector3.right);
        Vector3 curPos = new Vector2(transform.position.x, transform.position.z);
        Vector3 targetMcurrent = (target_loc - curPos);

        curSpeed = 0;
        curSpeedS = 0;

        if (allowControl)
        {
            curSpeed = Vspeed * Input.GetAxis("Vertical");
            curSpeedS = Vspeed * Input.GetAxis("Horizontal");
        }

        if ((curSpeed == 0) && (curSpeedS == 0))
        {

            if (waitDelay == false)
            {
                waitDelay = true;
                StartCoroutine(controlDelay());
            }
        }
        else
        {
            control = true;
           
        }
        if (!skipUpdate)
        {
            if (control == false)
            {
                _agent.isStopped = false;
                _agent.destination = this.target_loc;
            }
            else if (control == true)
            {
                _agent.isStopped = true;
                mover.SimpleMove(forward * curSpeed);
                mover.SimpleMove(sideways * curSpeedS);
            }
        }

    }

    public void setTargetLoc(Vector3 vec)
    {
        target_loc = vec;
    }

    IEnumerator controlDelay()
    {
        yield return new WaitForSeconds(1.0f);
        if ((curSpeed == 0) && (curSpeedS == 0))
        {
            control = false;
        }
        
        waitDelay = false;
        yield return null;
    }

    public IEnumerator goToLocator(GameObject l, NPCroutine npc)
    {
       if (!skipUpdate)
       {
            _agent.destination = l.transform.position;
       }

        setTargetLoc(l.transform.position);

        Vector2 curr = new Vector2(transform.position.x, transform.position.z);
        Vector2 tar = new Vector2(l.transform.position.x, l.transform.position.z);

        while (Vector2.SqrMagnitude(tar-curr) > 2)
        {
            yield return new WaitForSeconds(0.1f);
            curr = new Vector2(transform.position.x, transform.position.z);
        }

    }

    public bool cuffed = false;
    public IEnumerator handcuffTo(GameObject npc)
    {
        print("cuffing " + gameObject + " to " + npc.gameObject);
        cuffed = true;
        stopFight();
        float old_s = _agent.speed;
        _agent.speed = npc.GetComponent<NavMeshAgent>().speed + 2;
        while (cuffed)
        {
            allowControl = false;
            setTargetLoc(npc.transform.position);
            //skipUpdate = true;
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(2.0f);
        //skipUpdate = false;
        if (gameObject.GetComponentInChildren<movement>() != null)
        {
            allowControl = true;
        }
        Vspeed = old_s;
        yield return null;
    }

    public void uncuff()
    {
        cuffed = false;
    }
}
