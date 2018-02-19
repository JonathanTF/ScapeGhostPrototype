using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



public class ResistGUI : MonoBehaviour {

    public float bar = 0;
    public Vector2 pos = new Vector2(20, 40);
    public Vector2 size = new Vector2(60, 20);
    public Texture2D progressEmpty;
    public Texture2D progressFull;
    public Slider slider;

    public float value = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = value;
	}

    private void FixedUpdate()
    {
        if(GetComponentInParent<NPCroutine>() != null)
        {
            value = GetComponentInParent<NPCroutine>().resistance;
        }
        else
        {
            value = 0;
        }
    }

     
        
}


