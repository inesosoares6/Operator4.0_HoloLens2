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
    private Vector3 lookPosition, lookVector;
    private Quaternion lookRotation;
    public TextMeshPro titleText;
    public TextMeshPro descriptionText;
    public TextMeshPro buttonText;
    public GameObject sphere;
    public GameObject dialog;
    public int CurrentCount;
    public ManagerScript managerScript;


    // Update is called once per frame
    void Update()
    {
        switch (CurrentCount)
        {
            // Instructions to origin
            case 1:
                titleText.text = "Define Coordinate System";
                descriptionText.text = "Origin, after you click the button you have 5 seconds to position you finger in the referential origin.";
                buttonText.text = "Set origin";
                break;

            // Define referetial origin
            case 2:
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
            case 3:
                dialog.SetActive(true);
                titleText.text = "Define Coordinate System";
                descriptionText.text = "Second point, after you click the button you have 5 seconds to position you finger in the second point to define the orientation.";
                buttonText.text = "Set point";
                break;

            // Define point in X axis
            case 4:
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
            case 5:
                dialog.SetActive(true);
                titleText.text = "Define Coordinate System";
                descriptionText.text = "Start playing";
                buttonText.text = "Start Playing";

                // define referential origin (coordinations for the cube)
                itemToPosition.transform.position = new Vector3(origin.x, origin.y, origin.z);

                // set referential orientation
                lookVector = secondPoint - itemToPosition.transform.position;
                lookRotation = Quaternion.LookRotation(lookVector, Vector3.up);
                itemToPosition.transform.rotation = lookRotation;

                break;

            case 6:
                dialog.SetActive(false);
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

