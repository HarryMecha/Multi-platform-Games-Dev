/* ACKNOWLEDGMENTS
 * Code has been modified from Grapple Hook script found at: https://youtu.be/Xgh4v1w5DxU?si=d17aevaVWMJ_xfsd 
 */


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class HarpoonGun : MonoBehaviour
{
    #region Fields
    public Camera PlayerCamera;
    public float maxDistance;
    public LineRenderer lineRenderer;
    private Vector3 harpoonEnd;
    public Transform harpoonStart;
    private SpringJoint swingJoint;
    private ConfigurableJoint joint;
    private FixedJoint FixedJoint;
    public Transform player;
    private float lerpSpeed;
    private GameObject hookedObject;
    private Vector3 hookedObjectStartPos;
    private Vector3 hookedObjectEndPos;
    private Vector3 harpoonEndStartPos;
    private float startTime;
    private float journeyLength;
    [SerializeField] private List<GameObject> harpoonEndPFList;
    [SerializeField] private GameObject harpoonEndPF;
    private GameObject harpoonEndObject;
    private bool harpoonEndInst;
    private RaycastHit hit;
    private List<GameObject> activeHarpoons = new List<GameObject>();
    private GameObject EnviromentManager;
    private string currentHarpoonType;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        EnviromentManager = GameObject.Find("EnviromentManager");
        currentHarpoonType = EnviromentManager.GetComponent<PlayerManager>().currentHarpoonEquipped;
        harpoonEndInst = false;
        //harpoonEndPF = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/HarpoonEnd.prefab");
        maxDistance = 15f;
        lineRenderer = GetComponent<LineRenderer>();
        hookedObjectStartPos = Vector3.zero;
        hookedObjectEndPos = Vector3.zero;
        lerpSpeed = 20f;
    }


    private void LateUpdate()
    {
        DrawRope();
        /* A check is performed to see if an object is attached to the player object via a Joint, if so it will, move that object towards the player using linear interpolation, until
         * it reaches the player where the joint will be destroyed.*/
        

        
        if (harpoonEndInst)
        {
            float distCovered = (Time.time - startTime) * lerpSpeed;

            float fractionOfJourney = distCovered / journeyLength;
            fractionOfJourney = Mathf.Clamp01(fractionOfJourney);
            hookedObjectEndPos = harpoonEnd;
            harpoonEndObject.transform.position = Vector3.Lerp(harpoonStart.position, hookedObjectEndPos, fractionOfJourney);

            if (harpoonEndObject.transform.position == hookedObjectEndPos)
            {
                lineRenderer.positionCount = 0;
                Destroy(joint);
                hookedObject = null;
                if (hit.transform != false)
                {
                    switch (hit.transform.tag)
                    {
                        case ("Swingable"):
                            swingJointSetup();
                            break;

                        case ("Enemy"):
                            switch (currentHarpoonType)
                            {
                                case ("none"):
                                    jointSetup();
                                    break;
                                case ("Barbed Harpoon"):
                                    hit.transform.GetComponent<EnemyHealth>().TakeDamage(10);
                                    lineRenderer.positionCount = 0;
                                    Destroy(joint);

                                    harpoonEndInst = false;
                                    break;
                            }
                            break;
                        case ("Collectible"):
                            jointSetup();
                            break;
                        case ("Interactable"):
                            interactableJointSetup(hit.transform.gameObject);
                            break;

                        default:
                            if(activeHarpoons.Count >= 5)
                            {
                               GameObject harpoonToDestroy = activeHarpoons[0];
                                activeHarpoons.RemoveAt(0);
                                Destroy(harpoonToDestroy);
                            }
                            activeHarpoons.Add(harpoonEndObject);
                            harpoonEndObject.transform.parent = hit.transform;
                            break;
                    }
                    harpoonEndInst = false;
                }
                else
                {
                    lineRenderer.positionCount = 0;
                    Destroy(joint);
                    Destroy(harpoonEndObject);
                    harpoonEndInst = false;
                }
            }

        }
        
        if (hookedObject)
        {
            float distCovered = (Time.time - startTime) * lerpSpeed;

            float fractionOfJourney = distCovered / journeyLength;
            fractionOfJourney = Mathf.Clamp01(fractionOfJourney);
            hookedObjectEndPos = harpoonStart.position;
            hookedObject.transform.position = Vector3.Lerp(hookedObjectStartPos, hookedObjectEndPos, fractionOfJourney);
            harpoonEndObject.transform.position = Vector3.Lerp(harpoonEndStartPos, hookedObjectEndPos, fractionOfJourney);

            if (hookedObject.transform.position == hookedObjectEndPos)
            {
                lineRenderer.positionCount = 0;
                Destroy(joint);
                Destroy(harpoonEndObject);
                hookedObject = null;
            }

        }
        
    }

    /* shootRope is used to create a rope when the an eligble object is lined up with the crosshair, a ray is cast from the player object until it hits
     * an object if that object is swingable, a SpringJoint is attached to the player and the object, tethering them together, in contrast when an object 
     * with tag 'Enemy' is hit a ConfigurableJoint will be attached to the player.*/
    public void shootRope()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (!harpoonEndInst)
        {
            startTime = Time.time;
            currentHarpoonType = EnviromentManager.GetComponent<PlayerManager>().currentHarpoonEquipped;
            //this switch is used to determine the effect the currently equipped Harpoon will have.
            switch (currentHarpoonType)
            {
                case ("none"):
                    harpoonEndPF = harpoonEndPFList[0];
                    break;
                case ("Barbed Harpoon"):
                    harpoonEndPF = harpoonEndPFList[1];
                    break;
            }
            harpoonEndObject = Instantiate(harpoonEndPF, harpoonStart.position, PlayerCamera.transform.rotation);
            harpoonEndInst = true;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                harpoonEnd = hit.point;
            }
            else
            {
                harpoonEnd = harpoonStart.position + (ray.direction * maxDistance);
            }

            journeyLength = Vector3.Distance(harpoonStart.position, harpoonEnd);
            lineRenderer.positionCount = 2; // Ensure we can set both positions
            lineRenderer.SetPosition(0, harpoonStart.position);
            lineRenderer.SetPosition(1, harpoonEnd);
        }
    }

    /* stopRope is that condition is called when the SpringJoint needs to be destroyed*/
    public void stopRope()
    {
        lineRenderer.positionCount = 0;
        Destroy(swingJoint);
        Destroy(harpoonEndObject);
        hookedObject = null;
        harpoonEndInst = false;

    }

    /* DrawRope just renders the line where the joint is attached at each end.*/
    void DrawRope()
    {
{
    if (lineRenderer.positionCount < 2) return; // Prevent out-of-bounds errors

    if (harpoonEndInst)
    {
        lineRenderer.SetPosition(0, harpoonStart.position);
        lineRenderer.SetPosition(1, harpoonEndObject.transform.position);
    }
    else if (swingJoint)
    {
        lineRenderer.SetPosition(0, harpoonStart.position);
        lineRenderer.SetPosition(1, harpoonEnd);
    }
    else if (joint)
    {
        lineRenderer.SetPosition(0, harpoonStart.position);
        lineRenderer.SetPosition(1, harpoonEndObject.transform.position);
    }
}
    }

    /* isHooked is a getter method that returns whether there is an object assigned to the hookedObject field or not.*/
    public bool isHooked()
    {
        if (hookedObject) return true;

        return false;
    }

    public void swingJointSetup()
    {
        player.gameObject.GetComponent<PlayerController>().setGroundType(PlayerController.groundType.Swinging);
        swingJoint = player.gameObject.AddComponent<SpringJoint>();
        swingJoint.autoConfigureConnectedAnchor = false;
        swingJoint.connectedAnchor = harpoonEnd;

        float distance = Vector3.Distance(player.transform.position, harpoonEnd);

        swingJoint.maxDistance = distance * 0.8f;
        swingJoint.minDistance = distance * 0.25f;

        swingJoint.spring = 7;
        swingJoint.damper = 7.0f;
        swingJoint.massScale = 4.5f;

        lineRenderer.positionCount = 2;
    }

    public void jointSetup()
    {
        startTime = Time.time;
        harpoonEnd = hit.point;
        hookedObject = hit.collider.gameObject;
        hookedObjectStartPos = hookedObject.transform.position;
        harpoonEndStartPos = harpoonEndObject.transform.position;
        hookedObjectEndPos = transform.position;
        journeyLength = Vector3.Distance(hookedObjectStartPos, hookedObjectEndPos);
        joint = player.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedAnchor = harpoonEnd;
        lineRenderer.positionCount = 2;
    }

    public void interactableJointSetup(GameObject interactable)
    {
        FixedJoint = player.gameObject.AddComponent<FixedJoint>();
        FixedJoint.connectedBody = interactable.GetComponent<Rigidbody>();
        lineRenderer.positionCount = 2;
    }
}
