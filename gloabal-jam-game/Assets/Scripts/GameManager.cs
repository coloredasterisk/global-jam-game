using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Grid Stuff")]
    public List<GridComponent> gridPrefabs;
    public static Dictionary<TileType, GridComponent> gridPrefabsDict = new Dictionary<TileType, GridComponent>();
    public Tilemap gridLines;

    [Header("\nUndoProperties")]
    private float[] undoInterval = {0.25f, 0.1f, 0.05f};
    public int intervalIndex = 0;
    public int undoCounts = 0;
    public float undoTimer = 0;

    [Header("\nLevel Storage")]
    public Dictionary<string, GameObject> levelPrefabs = new Dictionary<string, GameObject>();
    public List<GameObject> levels;
    public LevelBehavior currentLevel;

    public GameObject pastGround;
    public GameObject presentGround;

    public GameObject presentPlayer;
    public GameObject pastPlayer;

    [Header("\nVisualEffects")]
    public GameObject postProcessing;
    public GameObject colorFilter;
    public CinemachineTargetGroup cameraLevelTarget;

    [Header("\nOther")]
    public bool isPresent = true;
    public GameObject graveYard;

    private AudioSource audioSource;
    public AudioClip[] audioClip;

    

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
        ColorGridLines();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLevel.ToggleVisibleStates();
            TogglePlayer();

            audioSource.clip = audioClip[0];
            audioSource.Play();
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

            presentGround.SetActive(false);
            pastGround.SetActive(true);

            postProcessing.SetActive(true);
            colorFilter.SetActive(true);

        }
        else//switch to present
        {

            isPresent = true;
            presentPlayer.SetActive(true);
            pastPlayer.SetActive(false);

            presentGround.SetActive(true);
            pastGround.SetActive(false);

            postProcessing.SetActive(false);
            colorFilter.SetActive(false);

        }
    }

    //Yes dog poop code i know
    public void RestartLevel()
    {
        GameObject levelPrefab = levelPrefabs[currentLevel.name];

        GameObject createdPrefab = levelPrefab.GetComponent<LevelBehavior>().createdParent;
        GameObject clonedPrefab = levelPrefab.GetComponent<LevelBehavior>().clonedParent;

        currentLevel.EraseChildrenFromGrid();

        Destroy(currentLevel.createdParent);
        Destroy(currentLevel.clonedParent);

        GameObject createdClone = Instantiate(createdPrefab, currentLevel.pastObject.transform);
        GameObject clonedClone = Instantiate(clonedPrefab, currentLevel.presentObject.transform);

        currentLevel.createdParent = createdClone;
        currentLevel.clonedParent = clonedClone;

        currentLevel.presentSpawnPoint = currentLevel.presentObject.transform.Find("StaticObjects").Find("SpawnPoint").GetComponent<GridComponent>();
        currentLevel.pastSpawnPoint = currentLevel.pastObject.transform.Find("StaticObjects").Find("SpawnPoint").GetComponent<GridComponent>();

        ResetPlayerPosition(presentPlayer.GetComponentInChildren<GridComponent>(true), currentLevel.presentSpawnPoint);
        ResetPlayerPosition(pastPlayer.GetComponentInChildren<GridComponent>(true), currentLevel.pastSpawnPoint);

        GridComponent[] dynamicObjects = currentLevel.dynamicPresent.GetComponentsInChildren<GridComponent>();
        foreach (GridComponent dynamic in dynamicObjects)
        {
            dynamic.DynamicReset();
        }

        InteractionLog.history.Clear();

    }
    public void ResetPlayerPosition(GridComponent player, GridComponent spawnPoint)
    {
        player.RemoveFromGrid();
        player.transform.position = spawnPoint.transform.position;
        player.gridPosition = spawnPoint.gridPosition;
        player.AddToGrid(false, true);
    }

    public void ChangeLevel(LevelBehavior level)
    {

        cameraLevelTarget.AddMember(level.gameObject.transform, 1, level.cameraRadius);
        cameraLevelTarget.RemoveMember(currentLevel.transform);

        currentLevel = level;

        ResetPlayerPosition(pastPlayer.GetComponentInChildren<GridComponent>(true), currentLevel.pastSpawnPoint);
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
    public void ColorGridLines()
    {
        gridLines.color = new Color(SettingsManager.gridRed, SettingsManager.gridGreen, SettingsManager.gridBlue, gridLines.color.a);
    }


    public void SendToGraveyard(GameObject item)
    {
        item.transform.parent = graveYard.transform;
    }



}
