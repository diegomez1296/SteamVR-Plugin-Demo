using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class RemoteController : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean powerAction;
    private Interactable interactable;

    private TVController connectedTV;
    private BoxCollider remoteBody;

    #region SphereCast Options

    [Header("SphereCast Options")]

    [SerializeField] private bool debugSphereCast = false;
    [SerializeField] private GameObject hitObject;

    [SerializeField] private float sphereRadius = 0.5f;
    [SerializeField] private float maxDistance = 10;
    [SerializeField] private LayerMask layerMask;

    private Vector3 origin;
    private Vector3 direction;
    private float currentHitDistance;
    #endregion

    private void Start()
    {
        remoteBody = GetComponentInChildren<BoxCollider>();
        interactable = GetComponent<Interactable>();
    }

    void Update()
    {

        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;

            if (powerAction[source].stateDown)
            {
                connectedTV = FindTV();

                if (connectedTV == null) return;

                connectedTV.Switch();
            }
        }

        //FindTV();
    }

    private TVController FindTV()
    {
        origin = remoteBody.transform.position;
        direction = remoteBody.transform.right * -1;
        RaycastHit hit;

        if(Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            hitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;

            if(hitObject.GetComponentInParent<TVController>())
            {
                Debug.Log("Find: " + hitObject.GetComponentInParent<TVController>().gameObject.name);
                return hitObject.GetComponentInParent<TVController>();
            }
            else
            {
                return null;
            }
        }
        else
        {
            currentHitDistance = maxDistance;
            hitObject = null;
            return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!debugSphereCast) return;

        Gizmos.color = Color.blue;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
}
