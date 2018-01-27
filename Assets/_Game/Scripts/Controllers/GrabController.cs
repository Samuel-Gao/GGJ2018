﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    public bool isGrab = false;
    public bool isAnyMainFourFingersInHoldPosition;
    public Transform grabPosition;

    public bool isThumbInHoldPosition;
    public bool isIndexInHoldPosition;
    public bool isMiddleInHoldPosition;
    public bool isRingInHoldPosition;
    public bool isPinkyInHoldPosition;

    public bool isHoldingObj = false;
    public GameObject objToHold;

    public float minFingerGrabRotation = 55f;
    public float maxFingerGrabRotation = 200f;
    public float minThumbGrabRotation = 25f;
    public float maxThumbGrabRotation = 200f;

    public VRfreeGlove myVRfreeGloveScript;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check rotations of each finger depanding on which hardware we use (keyboard/controller vs VRfeeeGlove)
        if (MyGameManager._instance.isKeyboardControls) {
            CheckKeyboardGrab();
        }
        else
        {
            CheckVRfreeGloveGrab();
        }

        // Check if any of the main four fingers (excludes thumb) is in holding position
        if (isIndexInHoldPosition || isMiddleInHoldPosition || isRingInHoldPosition || isPinkyInHoldPosition)
        {
            isAnyMainFourFingersInHoldPosition = true;
        }
        else
        {
            isAnyMainFourFingersInHoldPosition = false;
        }

        // Set the isGrab bool
        if (isThumbInHoldPosition && isAnyMainFourFingersInHoldPosition)
        {
            isGrab = true;
        }
        else
        {
            isGrab = false;
        }

        // Grab object
        if (isGrab && objToHold != null)
        {
            isHoldingObj = true;

            // Remove all force from objToHold else we'll have wonky physics
            objToHold.GetComponent<Rigidbody>().velocity = Vector3.zero;
            objToHold.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            objToHold.transform.position = grabPosition.position;
        }
        else
        {
            isHoldingObj = false;
        }
    }

    private void CheckKeyboardGrab()
    {
        // TODO: rotation of Z-finger-segments because the glove doesn't use the r-model rotations
        isThumbInHoldPosition = IsFingerIsInHoldPositionZ(KeyboardGloveController._instance.thumb2Rotation, minThumbGrabRotation, maxThumbGrabRotation);
        isIndexInHoldPosition = IsFingerIsInHoldPositionZ(KeyboardGloveController._instance.index2Rotation, minFingerGrabRotation, maxFingerGrabRotation);
        isMiddleInHoldPosition = IsFingerIsInHoldPositionZ(KeyboardGloveController._instance.middle2Rotation, minFingerGrabRotation, maxFingerGrabRotation);
        isRingInHoldPosition = IsFingerIsInHoldPositionZ(KeyboardGloveController._instance.ring2Rotation, minFingerGrabRotation, maxFingerGrabRotation);
        isPinkyInHoldPosition = IsFingerIsInHoldPositionZ(KeyboardGloveController._instance.little2Rotation, minFingerGrabRotation, maxFingerGrabRotation);
    }
    private void CheckVRfreeGloveGrab()
    {
        // TODO: rotation of Z-finger-segments because the glove doesn't use the r-model rotations
        isThumbInHoldPosition = IsFingerIsInHoldPositionX(myVRfreeGloveScript.thumb2Transform, minThumbGrabRotation, maxThumbGrabRotation);
        isIndexInHoldPosition = IsFingerIsInHoldPositionZ(myVRfreeGloveScript.index2Transform, minFingerGrabRotation, maxFingerGrabRotation);
        isMiddleInHoldPosition = IsFingerIsInHoldPositionZ(myVRfreeGloveScript.middle2Transform, minFingerGrabRotation, maxFingerGrabRotation);
        isRingInHoldPosition = IsFingerIsInHoldPositionZ(myVRfreeGloveScript.ring2Transform, minFingerGrabRotation, maxFingerGrabRotation);
        isPinkyInHoldPosition = IsFingerIsInHoldPositionZ(myVRfreeGloveScript.little2Transform, minFingerGrabRotation, maxFingerGrabRotation);
    }

    private bool IsFingerIsInHoldPositionZ(Transform fingerTransform, float minAngle, float maxAngle)
    {
        if (IsBetween(fingerTransform.localEulerAngles.z, minAngle, maxAngle))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool IsFingerIsInHoldPositionX(Transform fingerTransform, float minAngle, float maxAngle)
    {
        if (IsBetween(fingerTransform.localEulerAngles.x, minAngle, maxAngle))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsBetween(float value, float min, float max)
    {
        return (value >= min && value <= max);
    }

    void OnTriggerEnter(Collider col)
    {
        if (isHoldingObj) { return; }
        Debug.Log("Trigger Enter");
        objToHold = col.gameObject;
    }
    void OnTriggerStay(Collider col)
    {
        if (isHoldingObj) { return; }
        objToHold = col.gameObject;
    }
    void OnTriggerExit(Collider col)
    {
        if (isHoldingObj) { return; }
        Debug.Log("Trigger Exit");
        if (col.gameObject == objToHold) {
            objToHold = null;
        }
    }
    //void OnCollsionEnter(Collision col)
    //{
    //    Debug.Log("Collison Enter");
    //}
}
