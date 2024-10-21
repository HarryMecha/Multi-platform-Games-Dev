/* ACKNOWLEDGMENTS
 * Code has been modified from Grapple Hook script found at: https://youtu.be/Xgh4v1w5DxU?si=d17aevaVWMJ_xfsd 
 */


using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class HarpoonGun : MonoBehaviour
{
    public Camera PlayerCamera;
    public float maxDistance = 100;
    public LineRenderer lineRenderer;
    private Vector3 harpoonEnd;
    public Transform harpoonStart;
    private SpringJoint swingJoint;
    private ConfigurableJoint joint;
    public Transform player;
    private float lerpSpeed;
    private GameObject hookedObject;
    private Vector3 hookedObjectStartPos;
    private Vector3 hookedObjectEndPos;
    private float startTime;
    private float journeyLength;

    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        hookedObjectStartPos = Vector3.zero;
        hookedObjectEndPos = Vector3.zero;
        lerpSpeed = 10f;
}


    private void LateUpdate()
    { 
        DrawRope();
        if (hookedObject)
        {
            float distCovered = (Time.time - startTime) * lerpSpeed;

            float fractionOfJourney = distCovered / journeyLength;
           fractionOfJourney = Mathf.Clamp01(fractionOfJourney);
            hookedObjectEndPos = harpoonStart.position;
            hookedObject.transform.position = Vector3.Lerp(hookedObjectStartPos, hookedObjectEndPos, fractionOfJourney);


            if (hookedObject.transform.position == hookedObjectEndPos)
            {
                lineRenderer.positionCount = 0;
                Destroy(joint);
                hookedObject = null;
            }

        }
    }

    public void shootRope()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            switch (hit.transform.tag) {
                case ("Swingable"):

                    player.gameObject.GetComponent<PlayerController>().setGroundType(PlayerController.groundType.Swinging);
                    harpoonEnd = hit.point;
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

                break;

                case ("Enemy"):
                    startTime = Time.time;
                    harpoonEnd = hit.point;
                    hookedObject = hit.collider.gameObject;
                    hookedObjectStartPos = hookedObject.transform.position;
                    hookedObjectEndPos = transform.position;
                    journeyLength = Vector3.Distance(hookedObjectStartPos, hookedObjectEndPos);
                    joint = player.gameObject.AddComponent<ConfigurableJoint>();
                    joint.connectedAnchor = harpoonEnd;
                    lineRenderer.positionCount = 2;
                    break;
            }


        }
    }

    public void stopRope()
    {
        lineRenderer.positionCount = 0;
        Destroy(swingJoint);
        hookedObject = null;

    }

    void DrawRope()
    {
        if (swingJoint)
        {
            lineRenderer.SetPosition(0, harpoonStart.position);
            lineRenderer.SetPosition(1, harpoonEnd);
        }

        if (joint)
        {
            lineRenderer.SetPosition(0, harpoonStart.position);
            lineRenderer.SetPosition(1, hookedObject.transform.position);
        }
    }

    public bool isHooked()
    {
        if (hookedObject) return true;

        return false;
    }
}
