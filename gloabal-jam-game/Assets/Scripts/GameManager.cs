using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public List<GridComponent> gridPrefabs;
    public static Dictionary<TileType, GridComponent> gridPrefabsDict = new Dictionary<TileType, GridComponent>();

    private float[] undoInterval = {0.25f, 0.1f, 0.05f};
    public int intervalIndex = 0;
    public int undoCounts = 0;
    public float undoTimer = 0;
    

    public Dictionary<string, GameObject> levelPrefabs = new Dictionary<string, GameObject>();
    public List<GameObject> levels;

    public GameObject presentPlayer;
    public GameObject pastPlayer;
    public GameObject postProcessing;

    public CinemachineTargetGroup cameraLevelTarget;

    public LevelBehavior currentLevel;
    public bool isPresent = true;

    public GameObject graveYard;

    

    private void Awake()
    {
        
        foreach(GameObject level in levels)
        {
            levelPrefabs.Add(level.name, level);
        }
        foreach(GridComponent component in gridPrefabs)
        {
            gridPrefabsDict.Add(component.tileType, component);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLevel.ToggleVisibleStates();
            TogglePlayer();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        //allow user to hold undo button and undo speeds up based on time held down
        if (Input.GetKey(KeyCode.U))
        {
            if(undoTimer <= 0)
            {
                undoTimer = undoInterval[intervalIndex];
                IntervalChange();

                InteractionLog.Undo();
            }
            else
            {
                undoTimer -= Time.deltaTime;
            }
        }
        else
        {
            intervalIndex = 0;
            undoTimer = 0;
            undoCounts = 0;
        }

        
    }

    public void TogglePlayer()
    {
        if (isPresent)//switch to past
        {
            isPresent = false;
            presentPlayer.SetActive(false);
            pastPlayer.SetActive(true);
            postProcessing.SetActive(true);

        }
        else//switch to present
        {

            isPresent = true;
            presentPlayer.SetActive(true);
            pastPlayer.SetActive(false);
            postProcessing.SetActive(false);

            GameObject[] list = new GameObject[1];

        }
    }

    //Yes dog poop code i know
    public void RestartLevel()
    {
        GameObject levelPrefab = levelPrefabs[currentLevel.name];

        GameObject createdPrefab = levelPrefab.GetComponent<LevelBehavior>().createdParent;
        GameObject clonedPrefab = levelPrefab.GetComponent<LevelBehavior>().clonedParent;
        GameObject dynamicPastPrefab = levelPrefab.GetComponent<LevelBehavior>().dynamicPast;
        GameObject dynamicPresentPrefab = levelPrefab.GetComponent<LevelBehavior>().dynamicPresent;

        currentLevel.EraseChildrenFromGrid();

        Destroy(currentLevel.createdParent);
        Destroy(currentLevel.clonedParent);
        Destroy(currentLevel.dynamicPast);
        Destroy(currentLevel.dynamicPresent);

        GameObject createdClone = Instantiate(createdPrefab, currentLevel.pastObject.transform);
        GameObject clonedClone = Instantiate(clonedPrefab, currentLevel.presentObject.transform);
        GameObject dynamicPastClone = Instantiate(dynamicPastPrefab, currentLevel.pastObject.transform);
        GameObject dynamicPresentClone = Instantiate(dynamicPresentPrefab, currentLevel.presentObject.transform);

        currentLevel.createdParent = createdClone;
        currentLevel.clonedParent = clonedClone;
        currentLevel.dynamicPast = dynamicPastClone;
        currentLevel.dynamicPresent = dynamicPresentClone;
        //may need separate spawnpoint for past player
        currentLevel.spawnPoint = currentLevel.presentObject.transform.Find("StaticObjects").Find("SpawnPoint").GetComponent<GridComponent>();

        ResetPlayerPosition(presentPlayer.GetComponentInChildren<GridComponent>(true), currentLevel.spawnPoint);
        ResetPlayerPosition(pastPlayer.GetComponentInChildren<GridComponent>(true), currentLevel.spawnPoint);

    }
    public void ResetPlayerPosition(GridComponent player, GridComponent spawnPoint)
    {
        player.RemoveFromGrid();
        player.transform.position = spawnPoint.transform.position;
        player.gridPosition = spawnPoint.gridPosition;
        GridManager.InsertSelf(player);
    }

    public void ChangeLevel(LevelBehavior level)
    {

        cameraLevelTarget.AddMember(level.gameObject.transform, 1, level.cameraRadius);
        cameraLevelTarget.RemoveMember(currentLevel.transform);

        currentLevel = level;

    }

    public void IntervalChange()
    {
        undoCounts++;
        if(undoCounts > 30)
        {
            intervalIndex = 2;
        } else if(undoCounts > 5)
        {
            intervalIndex = 1;
        }
        else
        {
            intervalIndex = 0;
        }
    }


    public void SendToGraveyard(GameObject item)
    {
        item.transform.parent = graveYard.transform;
    }



    



}
