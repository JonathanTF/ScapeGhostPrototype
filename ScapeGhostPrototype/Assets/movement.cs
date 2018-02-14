using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

    private CharacterController move;
    private NPCroutine npc;
    private Vector2 start_loc;
    public float Vspeed = 1;
    public float Rspeed = 1;
    private bool ejected = true;
    public float radius;

    // Use this for initialization
    void Start() {
        move = gameObject.GetComponent<CharacterController>();


        start_loc = new Vector2(transform.position.x, transform.position.z);

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("space"))
        {
            Collider[] hitObjects = Physics.OverlapSphere(gameObject.transform.position, radius);
           // print("Touching objects: ");
            float min_dist = float.MaxValue;
            GameObject min_NPC = null;

            foreach (Collider c in hitObjects)
            {
                //print(c.gameObject);
                if (c.gameObject.tag.Equals("NPC"))
                {
                    if (!ejected)
                    {
                        if (c.gameObject.Equals(gameObject.transform.parent.gameObject))
                        {
                            continue;
                        }
                    }

                    float t = Vector3.Magnitude(c.gameObject.transform.position - gameObject.transform.position);
                    if (t < min_dist)
                    {
                        min_dist = t;
                        min_NPC = c.gameObject;
                    }
                }
            }

            if (min_NPC != null){
                if (!ejected)
                {
                    npc.setAllowControl(false);
                    npc.setControl(false);
                }

                gameObject.transform.position = min_NPC.transform.position;
                gameObject.transform.parent = min_NPC.transform;
                move = GetComponentInParent<CharacterController>();
                npc = GetComponentInParent<NPCroutine>();
                npc.setAllowControl(true);

                ejected = false;
            }
            else
            {
                print("nothin to control");
            }

            

        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (npc != null)
            {
                npc.setAllowControl(false);
                //npc.setControl(false);
            }

            gameObject.transform.parent = null;

            ejected = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && npc!=null)
        {
            crazyScript crazy = npc.GetComponent<crazyScript>();
            if(crazy != null)
            {
                Collider[] hitObjects = Physics.OverlapSphere(gameObject.transform.position, radius);
                // print("Touching objects: ");
                float min_dist = float.MaxValue;
                GameObject min_NPC = null;

                foreach (Collider c in hitObjects)
                {
                    //print(c.gameObject);
                    if (c.gameObject.tag.Equals("NPC"))
                    {
                        if (!ejected)
                        {
                            if (c.gameObject.Equals(gameObject.transform.parent.gameObject))
                            {
                                continue;
                            }
                        }

                        float t = Vector3.Magnitude(c.gameObject.transform.position - gameObject.transform.position);
                        if (t < min_dist)
                        {
                            min_dist = t;
                            min_NPC = c.gameObject;
                        }
                    }
                }
                if(min_dist > 5)
                {
                    print("nothin to fight");
                    return;
                }
                print("fighting: " + min_NPC);
                crazy.goCrazy(min_NPC);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (ejected)
            {
                return;
            }
            Collider[] hitObjects = Physics.OverlapSphere(gameObject.transform.position, radius);
            // print("Touching objects: ");
            float min_dist = float.MaxValue;
            GameObject min_mat = null;

            foreach (Collider c in hitObjects)
            {
                //print(c.gameObject);
                if (c.gameObject.tag.Equals("mat"))
                {
                    if (c.gameObject.Equals(gameObject.transform.parent.gameObject))
                    {
                        continue;
                    }

                    float t = Vector3.Magnitude(c.gameObject.transform.position - gameObject.transform.position);
                    if (t < min_dist)
                    {
                        min_dist = t;
                        min_mat = c.gameObject;
                    }
                }
            }

            if(min_mat == null)
            {
                print("no mats close enough");
            }
            else
            {
                npc.GetComponent<NPCroutine>().matInteract(min_mat);
            }


        }

    }





}
