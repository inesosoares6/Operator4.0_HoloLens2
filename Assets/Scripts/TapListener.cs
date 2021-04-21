using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using System;

public class TapListener : MonoBehaviour, IMixedRealityGestureHandler
{
    public GameObject sphere;

    public void OnGestureStarted(InputEventData eventData)
    {
        Debug.Log($"OnGestureStarted [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Hold Action")
        {
            //SetIndicator(HoldIndicator, "Hold: started", HoldMaterial);
        }
        else if (action == "Manipulate Action")
        {
            //SetIndicator(ManipulationIndicator, $"Manipulation: started {Vector3.zero}", ManipulationMaterial, Vector3.zero);
        }
        else if (action == "Navigation Action")
        {
            //SetIndicator(NavigationIndicator, $"Navigation: started {Vector3.zero}", NavigationMaterial, Vector3.zero);
        }
    }

    public void OnGestureUpdated(InputEventData eventData)
    {
        Debug.Log($"OnGestureUpdated [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Hold Action")
        {
            //SetIndicator(HoldIndicator, "Hold: updated", DefaultMaterial);
        }
    }

    public void OnGestureUpdated(InputEventData<Vector3> eventData)
    {
        Debug.Log($"OnGestureUpdated [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Manipulate Action")
        {
            //SetIndicator(ManipulationIndicator, $"Manipulation: updated {eventData.InputData}", ManipulationMaterial, eventData.InputData);
        }
        else if (action == "Navigation Action")
        {
            //SetIndicator(NavigationIndicator, $"Navigation: updated {eventData.InputData}", NavigationMaterial, eventData.InputData);
            //ShowRails(eventData.InputData);
        }
    }

    public void OnGestureCompleted(InputEventData eventData)
    {
        Debug.Log($"OnGestureCompleted [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Hold Action")
        {
            sphere.GetComponent<Renderer>().material.color = Color.green;
            //SetIndicator(HoldIndicator, "Hold: completed", DefaultMaterial);
        }
        else if (action == "Select")
        {
            sphere.GetComponent<Renderer>().material.color = Color.green;
            //SetIndicator(SelectIndicator, "Select: completed", SelectMaterial);
        }
    }

    public void OnGestureCompleted(InputEventData<Vector3> eventData)
    {
        Debug.Log($"OnGestureCompleted [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Manipulate Action")
        {
            sphere.GetComponent<Renderer>().material.color = Color.green;
            //SetIndicator(ManipulationIndicator, $"Manipulation: completed {eventData.InputData}", DefaultMaterial, eventData.InputData);
        }
        else if (action == "Navigation Action")
        {
            sphere.GetComponent<Renderer>().material.color = Color.green;
            //SetIndicator(NavigationIndicator, $"Navigation: completed {eventData.InputData}", DefaultMaterial, eventData.InputData);
            //HideRails();
        }
    }

    public void OnGestureCanceled(InputEventData eventData)
    {
        Debug.Log($"OnGestureCanceled [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Hold Action")
        {
            //SetIndicator(HoldIndicator, "Hold: canceled", DefaultMaterial);
        }
        else if (action == "Manipulate Action")
        {
            //SetIndicator(ManipulationIndicator, "Manipulation: canceled", DefaultMaterial);
        }
        else if (action == "Navigation Action")
        {
            //SetIndicator(NavigationIndicator, "Navigation: canceled", DefaultMaterial);
            //HideRails();
        }
    }
}
