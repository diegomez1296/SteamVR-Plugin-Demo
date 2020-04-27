using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerManager : MonoBehaviour
{
    public bool showControllers = false;
    private bool _isControllersVisible = false;

    [Header("Movement")]
    [SerializeField] private SteamVR_Action_Vector2 input;
    [SerializeField] private CharacterController character;
    [SerializeField] private float speed;


    private void Start()
    {
        _isControllersVisible = showControllers;
        ShowControllers(_isControllersVisible);
    }

    void Update()
    {
        CheckControllersVisible();
        CharacterMovement();
    }

    private void CheckControllersVisible()
    {
        if (showControllers)
        {
            if (_isControllersVisible) return;

            _isControllersVisible = true;
            ShowControllers(_isControllersVisible);
        }
        else
        {
            if (!_isControllersVisible) return;

            _isControllersVisible = false;
            ShowControllers(_isControllersVisible);
        }
    }
    private void ShowControllers(bool value)
    {
        if (value)
        {
            foreach (var hand in Player.instance.hands)
            {
                hand.ShowController();
                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);
            }
        }
        else
        {
            foreach (var hand in Player.instance.hands)
            {
                hand.HideController();
                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithoutController);
            }
        }
    }

    private void CharacterMovement()
    {
        if (input.axis.magnitude <= 0.1f) return;

        Vector3 direction = Player.instance.hmdTransform.TransformDirection(input.axis.x, 0, input.axis.y);
        character.Move(Vector3.ProjectOnPlane(direction, Vector3.up) * speed * Time.deltaTime - new Vector3(0, 9.81f, 0) * Time.deltaTime);

    }
}
