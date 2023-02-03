using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Dictionary<string, GameObject> levelPrefabs = new Dictionary<string, GameObject>();
    public List<GameObject> levels;

    public GameObject presentPlayer;
    public GameObject pastPlayer;
    public GameObject postProcessing;

    public CinemachineTargetGroup cameraLevelTarget;

    public LevelBehavior currentLevel;
    public bool isPresent = true;
    private void Awake()
    {
        foreach(GameObject level in levels)
        {
            levelPrefabs.Add(level.name, level);
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
    }

    void TogglePlayer()
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
            

        }
    }

    public void RestartLevel()
    {
        GameObject levelPrefab = levelPrefabs[currentLevel.name];
        GameObject pastPrefab = levelPrefab.GetComponent<LevelBehavior>().pastObject;
        GameObject presentPrefab = levelPrefab.GetComponent<LevelBehavior>().presentObject;

        currentLevel.EraseChildrenFromGrid();

        Destroy(currentLevel.pastObject);
        Destroy(currentLevel.presentObject);
        
        Transform parent = currentLevel.gameObject.transform;

        GameObject pastClone = Instantiate(pastPrefab, parent);
        GameObject presentClone = Instantiate(presentPrefab, parent);

        currentLevel.pastObject = pastClone;
        currentLevel.presentObject = presentClone;

        currentLevel.createdParent = pastClone.transform.Find("CreatedObjects").gameObject;
        currentLevel.clonedParent = presentClone.transform.Find("ClonedObjects").gameObject;

        currentLevel.spawnPoint = presentClone.transform.GetChild(1).Find("SpawnPoint").GetComponent<GridComponent>();

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



    



}
