using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyMatScript : MonoBehaviour {

    public GameObject key;
    public GameObject keyHolder;
    public bool hasKey= false;

	// Use this for initialization
	void Start () {
		if(key != null)
        {
            hasKey = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        
    }

    public bool giveKey(GameObject givenKey)
    {
        if(givenKey == null)
        {
            return false;
        }

        hasKey = true;
        key = givenKey;
        key.transform.parent = gameObject.transform;
        key.transform.position = gameObject.transform.position + new Vector3(0, 10, 0);

        return true;


    }

    public GameObject takeKey()
    {
        key.transform.parent = null;
        key.transform.position = keyHolder.transform.position;
        hasKey = false;
        return key;

    }

    public bool holdsKey()
    {
        return hasKey;
    }


}
