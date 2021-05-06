using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotWorkSpace : MonoBehaviour
{
    public GameObject sphere;
    public GameObject cylinder;
    public GameObject gizmo;

    public void drawRobotWorkspace()
    {
        sphere.SetActive(true);
        sphere.transform.position = gizmo.transform.position;

        cylinder.SetActive(true);
        cylinder.transform.position = gizmo.transform.position;
    }

}
