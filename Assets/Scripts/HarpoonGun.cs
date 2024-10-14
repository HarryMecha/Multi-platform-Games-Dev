using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonGun : MonoBehaviour
{
    public Camera PlayerCamera;
    public float maxDistance = 100;
    public LineRenderer lineRenderer;
    private Vector3 harpoonEnd;
    public Transform harpoonStart;
    private SpringJoint joint;
    public Transform player;

    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }

    private void LateUpdate()
    {
        DrawRope();
    }


    public void shootRope()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.Log("This");
            if (hit.transform.tag == "Swingable")
            {
                harpoonEnd = hit.point;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = harpoonEnd;

                float distance = Vector3.Distance(player.transform.position, harpoonEnd);

                joint.maxDistance = distance * 0.8f;
                joint.minDistance = distance * 0.25f;

                joint.spring = 4.5f;
                joint.damper = 7.0f;
                joint.massScale = 4.5f;
            }

        }
    }

    void DrawRope()
    {
        lineRenderer.SetPosition(0, harpoonStart.position);
        lineRenderer.SetPosition(1, harpoonEnd);
    }
}
