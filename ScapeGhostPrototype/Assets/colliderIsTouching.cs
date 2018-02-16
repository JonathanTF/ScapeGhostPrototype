using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderIsTouching : MonoBehaviour {

    List<GameObject> thingsIHave;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool doYouHave(GameObject obj)
    {
        if (thingsIHave.Contains(obj))
        {
            return true;
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        thingsIHave.Add(collision.gameObject);
        //print("added " + collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        thingsIHave.Remove(collision.gameObject);
    }
}
