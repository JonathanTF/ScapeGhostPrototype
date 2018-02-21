using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movement : MonoBehaviour {

    public float ghost_radius = 4;
    public float build_up_resistance = 0.2f;
    public float speed = 5;
    public GameObject firstGuy;
    private CharacterController move;
    private NPCroutine npc;
    public NPCroutine lastNpc;
    private Vector2 start_loc;
    public float Vspeed = 1;
    public float Rspeed = 1;
    private bool ejected = true;
    //public float radius;
    public Text theBOX;
    public Image goldKey;
    public GameObject theGreenSphere;
    

    // Use this for initialization
    void Start() {
        move = gameObject.GetComponent<CharacterController>();
        start_loc = new Vector2(transform.position.x, transform.position.z);
        gameObject.transform.position = firstGuy.transform.position;
        gameObject.transform.parent = firstGuy.transform;
        move = GetComponentInParent<CharacterController>();
        npc = GetComponentInParent<NPCroutine>();
        npc.writeBox();
        npc.setAllowControl(true);
        npc.changeToGhostMat();
        npc.resistance_build_up = build_up_resistance;
        npc.Vspeed = speed;
        ejected = false;
        gameObject.GetComponent<SphereCollider>().radius = ghost_radius;
        theGreenSphere.transform.localScale = new Vector3(ghost_radius * 2, ghost_radius * 2, ghost_radius * 2);
    }

    // Update is called once per frame
    bool spaceBreak = false;
    bool spaceUp = true;
    bool nDown = false;

    void Update() {

        if (!ejected)
        {
            if (GetComponentInParent<NPCroutine>().ownKey)
            {
                goldKey.enabled = true;
            }
            else
            {
                goldKey.enabled = false;
            }
        }

        if (ejected)
        {
            theBOX.text = " ";
        }

        if (Input.GetKeyUp("space"))
        {
            spaceUp = true;
            
        }
        if (Input.GetKeyDown("space"))
        {

            spaceUp = false;
            if (!spaceBreak)
            {


                spaceBreak = true;
                
                Collider[] hitObjects = Physics.OverlapSphere(gameObject.transform.position, ghost_radius);
                // print("Touching objects: ");

                GameObject me = null;

                List<GameObject> closeNPCs = new List<GameObject>();
                foreach (Collider c in hitObjects)
                {
                    //print(c.gameObject);

                    if (c.gameObject.tag.Equals("NPC"))
                    {
                        if (!ejected)
                        {
                            if (c.gameObject.Equals(gameObject.transform.parent.gameObject))
                            {
                                me = c.gameObject;
                                continue;
                            }
                        }
                        closeNPCs.Add(c.gameObject);
                    }
                }
                if (me != null)
                {
                    closeNPCs.Add(me);
                }

                

                GameObject[] NPCarr = closeNPCs.ToArray();

                if (NPCarr.Length != 0)
                {
                    StartCoroutine(pickInhabit(NPCarr));
                }
                else
                {
                    StartCoroutine(noOneAround());
                }

                /*
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
                */

            }
        }


        //Disperse
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (!ejected)
            {
                //not already ejected
                npc.setAllowControl(false);
                lastNpc = npc;
                //npc.setControl(false);
                gameObject.transform.parent = null;
                ejected = true;

            }
            else
            {
                //already ejected
                gameObject.transform.position = lastNpc.transform.position;
                gameObject.transform.parent = lastNpc.transform;
                move = GetComponentInParent<CharacterController>();
                npc = GetComponentInParent<NPCroutine>();
                npc.setAllowControl(true);
                npc.writeBox();
                ejected = false;
            }

            
        }

        if (Input.GetKeyDown(KeyCode.K) && npc!=null)
        {
            crazyScript crazy = npc.GetComponent<crazyScript>();
            if(crazy != null)
            {
                Collider[] hitObjects = Physics.OverlapSphere(gameObject.transform.position, ghost_radius);
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

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (ejected)
            {
                return;
            }
            if (GetComponentInParent<NPCroutine>().cuffed)
            {
                return;
            }
            Collider[] hitObjects = Physics.OverlapSphere(gameObject.transform.position, ghost_radius);
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
                }else if (c.gameObject.tag.Equals("gate"))
                {
                    if(c.gameObject.GetComponentInParent<doorScriptGaurd3>() != null)
                    {
                        c.gameObject.GetComponentInParent<doorScriptGaurd3>().interact(GetComponentInParent<NPCroutine>());
                    }
                    else
                    {
                        c.gameObject.GetComponentInParent<doorScript>().interact(GetComponentInParent<NPCroutine>());
                    }
                   
                    //print("gate interacted");
                }
            }

            if(min_mat == null)
            {
               // print("no mats close enough");
            }
            else
            {
                npc.GetComponent<NPCroutine>().matInteract(min_mat);
            }


        }

    }

    public IEnumerator pickInhabit(GameObject[] NPClist)
    {
        int length = NPClist.Length;
        int prev = 0;
        int current = 0;
        GameObject selectedNPC = NPClist[0];
        Time.timeScale = 0.075f;

        if (length != 0)
        {
            if (!ejected) {
                npc.changeTostandardMat();
            }

            NPClist[current].GetComponent<NPCroutine>().changeToGhostMat();

        }
        float timer = Time.time;
        while (!spaceUp && ((Time.time - timer) < 0.3f))
        {

            yield return new WaitForSeconds(0.0002f);
           
            if(length <= 1)
            {
                continue;
            }
            if (Input.GetKeyDown(KeyCode.N)) { 
                current++;
                current = current % length;
            
                NPClist[current].GetComponent<NPCroutine>().changeToGhostMat();
                NPClist[prev].GetComponent<NPCroutine>().changeTostandardMat();
            }
            prev = current;
        }

        print("exiting space break");

        if (!ejected)
        {
            npc.setAllowControl(false);
        }
        gameObject.transform.position = NPClist[current].transform.position;
        gameObject.transform.parent = NPClist[current].transform;
        move = GetComponentInParent<CharacterController>();
        npc = GetComponentInParent<NPCroutine>();
        npc.writeBox();
        npc.resistance_build_up = build_up_resistance;
        npc.Vspeed = speed;
        if (!npc.fighting && !npc.cuffed)
        {
            npc.setAllowControl(true);
        }

        if (ejected && (lastNpc != null) && (lastNpc != npc))
        {
            lastNpc.changeTostandardMat();
            lastNpc = null;
        }

        ejected = false;

        Time.timeScale = 1.0f;
        spaceBreak = false;
        yield return null;
    }

    public IEnumerator noOneAround()
    {
        Time.timeScale = 0.075f;
        float timer = Time.time;
        while (!spaceUp && ((Time.time - timer) < 0.3f))
        {
            yield return new WaitForSeconds(0.0002f);
        }
        Time.timeScale = 1.0f;
        ejected = true;
        spaceBreak = false;
        yield return null;
    }


}
