using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean fireAction;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float bulletSpeed;

    private Interactable interactable;

    private void Start()
    {
        interactable = GetComponent<Interactable>();


    }

    private void Update()
    {
        if(interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;

            if(fireAction[source].stateDown)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        Rigidbody bulletRb = Instantiate(bullet, bulletPosition.position, bulletPosition.rotation).GetComponent<Rigidbody>();
        bulletRb.velocity = bulletPosition.forward * bulletSpeed;
    }
}
