using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penTracker : MonoBehaviour {

    List<GameObject> thingsIHave = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool has(GameObject dude)
    {
        if (thingsIHave.Contains(dude))
        {
            return true;
        }
        else{
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        thingsIHave.Add(other.gameObject);
        print("ADDED: " + other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        thingsIHave.Remove(other.gameObject);
    }
}
