using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using TMPro;

public class SetReferentialGestures : MonoBehaviour
{
    public GameObject itemToPosition;
    private Vector3 origin;
    private Vector3 secondPoint;
    private float timer = 0.0f;
    private Vector3 lookVector;
    private Quaternion lookRotation;
    public TextMeshPro descriptionText;
    public TextMeshPro buttonText;
    public GameObject sphere;
    public GameObject dialog;
    private int CurrentCount;
    public ManagerScript managerScript;
    public GameObject dialogIntructions;


    // Update is called once per frame
    void Update()
    {
        switch (CurrentCount)
        {
            // Define referetial origin
            case 1:
                dialog.SetActive(false);
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose1))
                {
                    timer += Time.deltaTime;
                    if (timer > 5.0f)
                    {
                        origin = pose1.Position;
                        Debug.Log("Origin:" + origin + " count:" + CurrentCount);
                        timer = 0.0f;
                        increaseCount();
                    }
                }
                break;

            // Instructions to X
            case 2:
                dialog.SetActive(true);
                descriptionText.text = "Second point, after you click the button you have 5 seconds to position you finger in the second point to define the orientation.";
                buttonText.text = "Set point";
                break;

            // Define point in X axis
            case 3:
                dialog.SetActive(false);
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose2))
                {
                    timer += Time.deltaTime;
                    if (timer > 5.0f)
                    {
                        secondPoint = pose2.Position;
                        Debug.Log("Second Point:" + secondPoint + " count:" + CurrentCount);
                        timer = 0.0f;
                        increaseCount();
                    }
                }
                break;

            // Define orientation and save anchor
            case 4:
                dialogIntructions.SetActive(true);

                // define referential origin (coordinations for the cube)
                itemToPosition.SetActive(true);
                itemToPosition.transform.position = new Vector3(origin.x, origin.y, origin.z);

                // set referential orientation
                lookVector = secondPoint - itemToPosition.transform.position;
                lookRotation = Quaternion.LookRotation(lookVector, Vector3.forward);
                itemToPosition.transform.rotation = lookRotation;

                break;

            case 5:
                dialogIntructions.SetActive(false);
                sphere.SetActive(true);
                managerScript.beginGame();
                increaseCount();
                break;
        }
    }

    public void increaseCount()
    {
        CurrentCount = CurrentCount + 1;
    }

}

