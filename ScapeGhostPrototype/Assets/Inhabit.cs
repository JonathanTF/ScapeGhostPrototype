using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inhabit : MonoBehaviour {

    SphereCollider detectRadius;
    List<GameObject> NPCs = new List<GameObject>();
    public float radius;
    

	// Use this for initialization
	void Start () {
        detectRadius = GetComponent<SphereCollider>();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")){
            Collider[] hitObjects = Physics.OverlapSphere(gameObject.transform.position, radius);
            print("Touching objects: ");
            foreach (Collider c in hitObjects)
            {
                print(c.gameObject);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        //NPCs.
        //NPCs.add(other.gameObject);
    }


}
