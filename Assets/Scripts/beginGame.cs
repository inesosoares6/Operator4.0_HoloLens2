using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class beginGame : MonoBehaviour
{
    public GameObject sphere;
    public GameObject dialog;
    public TextMeshPro descriptionText;
    public TextMeshPro buttonText;
    public TextMeshPro titleText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void beginGameFunction()
    {
        dialog.SetActive(false);
        sphere.SetActive(true);
    }

    //public void firstCoordinate()
    //{
    //    titleText.text = "Define Coordinate System";
    //    descriptionText.text = "Origin, after you click the button you have 5 seconds to position you finger in the referential origin.";
    //    buttonText.text = "Set origin";
    //}
}
