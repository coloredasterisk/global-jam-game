using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public enum Interactions
{
    Moved,
    Faced,
    Switched,
    Created,
    Destroyed,
}

public class InteractionLog : MonoBehaviour
{
    
    [System.Serializable]
    public struct DataLog
    {
        public Interactions interaction;
        public MovementLog movementLog;
        public FacedLog facedLog;
        public SwitchedLog switchedLog;
    }
    [System.Serializable]
    public struct MovementLog
    {
        public Vector2Int movementDirection;
        public GridComponent movedObject;
        public GridComponent pushedObject;
        //public List<GridComponent> pushedObjects;
        //hold triggered items like pressure plates
    }
    [System.Serializable]
    public struct FacedLog
    {
        public PlayerController playerThatFaced;
        public Facing direction;
    }
    [System.Serializable]
    public struct SwitchedLog
    {
        public bool switchedToPresent;
        public LevelBehavior levelThatSwitched;
        public List<LevelBehavior> adjacentLevelsThatSwitched;
        public List<GameObject> clonedGameObjects;
        public List<ReconstructObject> replacedGameObjects;
    }
    public struct ReconstructObject
    {
        public GridComponent deadComponent;
        public Transform clonedParent;
        //extra states like on/off 
        
    }

    public static List<DataLog> history = new List<DataLog>();

    /// <summary>
    /// This function uses the history of user interactions stored in a list and attempts
    /// to undo the most recent action
    /// </summary>
    public static void Undo()
    {
        if(history.Count > 0)
        {
            DataLog recentLog = history[history.Count - 1];
            

            if(recentLog.interaction == Interactions.Moved)
            {
                recentLog.movementLog.movedObject.MovePosition(MovementInverse(recentLog.movementLog), true);
                history.RemoveAt(history.Count - 1);
                //undo again to push back pushabled objects
                if (recentLog.movementLog.pushedObject != null)
                {
                    Undo();
                }

            } else if(recentLog.interaction == Interactions.Faced)
            {
                recentLog.facedLog.playerThatFaced.facing = recentLog.facedLog.direction;
                recentLog.facedLog.playerThatFaced.DirectionToAnimation();
                history.RemoveAt(history.Count - 1);
                Debug.Log(">>>" +history.Count);
                //undo since it is usually accompanied by movement
                if(history.Count > 0)
                {
                    Undo();
                }
                
            } else if(recentLog.interaction == Interactions.Switched)
            {
                FindObjectOfType<GameManager>().TogglePlayer();
                LevelBehavior level = recentLog.switchedLog.levelThatSwitched;
                bool isPresent = recentLog.switchedLog.switchedToPresent;

                //set adjacent levels to same state
                LevelBehavior.SetStates(level, isPresent);
                foreach (LevelBehavior adjacentLevels in recentLog.switchedLog.adjacentLevelsThatSwitched)
                {
                    LevelBehavior.SetStates(adjacentLevels, isPresent);
                }

                //restore removed objects 
                if(recentLog.switchedLog.replacedGameObjects != null)
                {
                    foreach (ReconstructObject recon in recentLog.switchedLog.replacedGameObjects)
                    {
                        recon.deadComponent.transform.parent = recon.clonedParent;
                        recon.deadComponent.transform.localScale = Vector3.one;
                        GridManager.InsertSelf(recon.deadComponent);
                    }
                }
                
                //destroy created stuff
                if (recentLog.switchedLog.clonedGameObjects != null)
                {
                    foreach (GameObject clone in recentLog.switchedLog.clonedGameObjects)
                    {
                        if (clone != null)
                        {
                            clone.GetComponent<GridComponent>().RemoveFromGrid();
                            Destroy(clone);
                        }

                    }
                }
                
                history.RemoveAt(history.Count - 1);

            }

            
        }
        
    }

    public static void NewMovementLog(GridComponent movedObject, Vector2Int direction, GridComponent pushedObject)
    {
        MovementLog moveLog = new MovementLog();
        moveLog.movementDirection = direction;
        moveLog.movedObject = movedObject;
        moveLog.pushedObject = pushedObject;

        DataLog datalog = new DataLog();
        datalog.interaction = Interactions.Moved;
        datalog.movementLog = moveLog;

        history.Add(datalog);
    }
    private static Vector2Int MovementInverse(MovementLog log)
    {
        return log.movementDirection * -1;
    }


    public static void NewFacingLog(PlayerController player, Facing facing)
    {
        FacedLog facedLog = new FacedLog();
        facedLog.playerThatFaced = player;
        facedLog.direction = facing;

        DataLog datalog = new DataLog();
        datalog.interaction = Interactions.Faced;
        datalog.facedLog = facedLog;

        history.Add(datalog);
    }

    public static void NewSwitchedLog(bool isPresent, LevelBehavior level, List<GameObject> clones, List<ReconstructObject> replacements)
    {
        List<LevelBehavior> adjacents = new List<LevelBehavior>();
        foreach(LevelBehavior.levelAdajcent adajcentLevel in level.AdjacentLevels)
        {
            adjacents.Add(adajcentLevel.level);
        }

        SwitchedLog switchedLog = new SwitchedLog();
        switchedLog.switchedToPresent = isPresent;
        switchedLog.levelThatSwitched = level;
        switchedLog.adjacentLevelsThatSwitched = adjacents;
        switchedLog.clonedGameObjects = clones;
        switchedLog.replacedGameObjects = replacements;

        DataLog datalog = new DataLog();
        datalog.interaction = Interactions.Switched;
        datalog.switchedLog = switchedLog;

        history.Add(datalog);
    }
    public static ReconstructObject PrepareReconstruction(GridComponent gridComponent, Transform parent)
    {
        ReconstructObject recon = new ReconstructObject();
        recon.deadComponent = gridComponent;
        recon.clonedParent = parent;   
        return recon;
    }


}
