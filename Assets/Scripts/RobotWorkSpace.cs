using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotWorkSpace : MonoBehaviour
{
    private GameObject recommendedWorkspace;
    private GameObject cilinder;
    public GameObject gizmo;
    public Material materialRecommendedWorkspace;

    public void drawRobotWorkspace()
    {
        recommendedWorkspace = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        recommendedWorkspace.transform.position = gizmo.transform.position;
        recommendedWorkspace.transform.localScale = new Vector3(1.700f, 1.700f, 1.700f); //UR5
        recommendedWorkspace.GetComponent<Renderer>().material = materialRecommendedWorkspace;
        
        cilinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cilinder.transform.position = gizmo.transform.position;
        cilinder.transform.localScale = new Vector3(0.151f, 0.8105f, 0.151f); //UR5
        cilinder.GetComponent<Renderer>().material = materialRecommendedWorkspace;
    }

}
