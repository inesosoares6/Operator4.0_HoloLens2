using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Persistence;

public class AnchorScript : MonoBehaviour
{
    public GameObject rootGameObject;
    WorldAnchorStore worldAnchorStore = null;
    bool savedRoot = false;
    string[] ids = null;
    bool storeLoaded = false;
    WorldAnchor anchor;

    // Start is called before the first frame update
    void Start()
    {
        WorldAnchorStore.GetAsync(StoreLoaded);
    }

    // Update is called once per frame
    void Update()
    {
        if (storeLoaded)
        {
            EnumerateIDS();
        }

    }

    void EnumerateIDS()
    {
        Debug.Log("Enumerating IDS");
        storeLoaded = false;
        if (worldAnchorStore != null)
        {
            ids = this.worldAnchorStore.GetAllIds();
            Debug.Log("id length of anchors: " + ids.Length);
            for (int index = 0; index < ids.Length; index++)
            {
                Debug.Log(ids[index]);
            }
            LoadAnchor();
        }
        else
        {
            Debug.Log("Enumerating IDs: worldanchorstore null");
        }
    }

    private void LoadAnchor()
    {
        // Save data about holograms positioned by this world anchor
        // should automatically save a world anchor to this 
        this.savedRoot = this.worldAnchorStore.Load("root", rootGameObject);
        if (!this.savedRoot)
        {
            // We didn't actually have the game root saved! We have to re-place our objects or start over
            Debug.Log("Loadgame: no rootobject anchor saved previously");
        }
    }



    private void StoreLoaded(WorldAnchorStore store)
    {
        this.worldAnchorStore = store;
        Debug.Log("StoreLoaded: World anchor store loaded succesfully");
        storeLoaded = true;
    }

    public void SaveAnchor()
    {
        anchor = rootGameObject.AddComponent<WorldAnchor>();
        // Save data about holograms positioned by this world anchor
        if (!this.savedRoot && anchor != null) // Only Save the root once
        {
            this.savedRoot = this.worldAnchorStore.Save("root", anchor);
            // íf true then was a succesful (re)save
            Debug.Log("Saved anchor: " + this.savedRoot);

        }
        else
        {
            Debug.Log("Root has already been saved!");
        }
    }


    public void DestroyExistingAnchor()
    {
        anchor = rootGameObject.GetComponent<WorldAnchor>();
        if (anchor != null)
        {
            // destroy the anchor else object doesnt move
            Destroy(anchor);
            // delete the anchor ID from store
            worldAnchorStore.Delete("root");
            this.savedRoot = false;
        }
    }

}
