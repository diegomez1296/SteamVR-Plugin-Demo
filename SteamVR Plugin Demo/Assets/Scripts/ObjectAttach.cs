using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ObjectAttach : MonoBehaviour
{
    private Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnHandHoverBegin(Hand hand)
    {
        hand.ShowGrabHint();
    }

    private void OnHandHoverEnd(Hand hand)
    {
        hand.HideGrabHint();
    }

    private void OnHandHoverUpdate(Hand hand)
    {
        GrabTypes grabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

        //Grab
        if(interactable.attachedToHand == null && grabType != GrabTypes.None )
        {
            hand.AttachObject(this.gameObject, grabType);
            hand.HoverLock(interactable);
            hand.HideGrabHint();
        }
        //Release
        else
        {
            hand.DetachObject(this.gameObject);
            hand.HoverUnlock(interactable);
        }
    }
}
