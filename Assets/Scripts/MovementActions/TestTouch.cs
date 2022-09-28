using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using PhysicsBasedCharacterController;
using UnityEngine;

public class TestTouch : MonoBehaviour
{
    private InputReader input;
    private MovementActions movementActions;

    private Camera cameraMain;

    private void Awake()
    {
        input = new InputReader();
        movementActions = new MovementActions();
        cameraMain = Camera.main;
    }

    private void OnEnable()
    {
        input.OnStartTouch += Move;
    }

    private void OnDisable()
    {
        input.OnEndTouch -= Move;
    }

    public void Move(Vector2 screenPosition, float time)
    {
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, cameraMain.nearClipPlane);
        Vector3 worldCoordinates = cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;
        transform.position = worldCoordinates;
    }
}
