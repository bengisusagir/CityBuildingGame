using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    private bool currentlyPlacing;
    private bool currentlyBulldozering;

    private BuildingPresets curBuildingPreset;

    private float indicatorUpdateTime = 0.05f;
    private float lastUpdateTime;
    private Vector3 curIndicatorPos;

    public GameObject farmInd;
    public GameObject factorInd;
    public GameObject houseInd;
    public GameObject roadInd;
    public GameObject selectedInd;
    public GameObject bulldozerIndicator;
    

    public void BeginNewBuildingPlacement(BuildingPresets preset)
    {   
        currentlyPlacing = true;
        curBuildingPreset = preset;
        if(preset.name == "Farm")
        {
            farmInd.SetActive(true);
            selectedInd = farmInd;
        }
        else if(preset.name == "House")
        {
            houseInd.SetActive(true);
            selectedInd = houseInd;
        }
        else if (preset.name == "Factory")
        {
            factorInd.SetActive(true);
            selectedInd = factorInd;
        }
        else if (preset.name == "Road")
        {
            roadInd.SetActive(true);
            selectedInd = roadInd;
        }



    }

    void CancelBuildingPlacement()
    {
        currentlyPlacing = false;
        factorInd.SetActive(false);
        farmInd.SetActive(false);
        roadInd.SetActive(false);
        houseInd.SetActive(false);
    }
    public void ToogleBulldoze()
    {
        currentlyBulldozering = !currentlyBulldozering;
        bulldozerIndicator.SetActive(currentlyBulldozering);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelBuildingPlacement();
        if (Time.time - lastUpdateTime > indicatorUpdateTime)
        {
            lastUpdateTime = Time.time;

            curIndicatorPos = Selector.instance.GetCurTilePosition();

            if (currentlyPlacing)
            {

                if (curBuildingPreset.name == "Road")
                 {
                     roadInd.transform.position = curIndicatorPos;
                     if (Input.GetKey(KeyCode.R))
                     {
                         roadInd.transform.Rotate(Vector3.up, 90f);
                     }
                 }

                 else if (curBuildingPreset.name == "House")
                 {
                     houseInd.transform.position = curIndicatorPos;
                     if (Input.GetKey(KeyCode.R))
                     {
                         houseInd.transform.Rotate(Vector3.up, 90f );
                     }
                 }

                 else if(curBuildingPreset.name == "Farm")
                {
                    farmInd.transform.position = curIndicatorPos;
                    if (Input.GetKey(KeyCode.R))
                    {
                        farmInd.transform.Rotate(Vector3.up, 90f );
                    }
                }
                    
                 else if(curBuildingPreset.name == "Factory")
                {
                    factorInd.transform.position = curIndicatorPos;
                    if (Input.GetKey(KeyCode.R))
                    {
                        factorInd.transform.Rotate(Vector3.up, 90f);
                    }
                }

            }

            else if (currentlyBulldozering)
                bulldozerIndicator.transform.position = curIndicatorPos;

        }
        if(Input.GetMouseButtonDown(0) && currentlyPlacing)
        {
            PlaceBuilding();
        }
        else if (Input.GetMouseButtonDown(0) && currentlyBulldozering)
        {
            BullDoze();
        }
    }

    void PlaceBuilding()
    {
        GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curIndicatorPos, selectedInd.transform.rotation);
        City.instance.OnPlaceBuilding(buildingObj.GetComponent<Building>());
        CancelBuildingPlacement();
    }

    void BullDoze()
    {
        Building buildingToDestroy = City.instance.buildings.Find(x => x.transform.position == curIndicatorPos);
        if (buildingToDestroy != null)
        {
            City.instance.OnRemoveBuilding(buildingToDestroy);
        }
    }
}
