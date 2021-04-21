using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beginGame : MonoBehaviour
{
    public GameObject sphere;
    public GameObject dialog;

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
}
