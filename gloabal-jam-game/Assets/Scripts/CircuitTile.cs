using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitTile : MonoBehaviour
{
    private GridComponent gridComponent;

    


    public enum outputDirection 
    {
        Left,
        Right,
        Up,
        Down,
    }

    private List<outputDirection> circuitType;
    private bool powerStatus = false;

    public 
    void Start()
    {
        gridComponent = GetComponent<GridComponent>();
    }

    // Update is called once per frame
    void Update()
    { 
        
    }
    public bool checkPowerStatus(Vector2 current, outputDirection input) { //takes in current position and receiving outputdirection
        bool matching = false;
        foreach(outputDirection b in circuitType) { //checks if previous circuit connects to current circuit
            if ((input == outputDirection.Left && b==outputDirection.Right) || (input == outputDirection.Right && b==outputDirection.Left) ) {
                matching = true;
            } else if ((input == outputDirection.Down && b==outputDirection.Up) || (input == outputDirection.Up && b==outputDirection.Down)) {
                matching = true;
            }

        }

        if (matching == false){
            return powerStatus = false;
        }

        foreach(outputDirection a in circuitType){
            bool success; //local variable to see if even one possible powered connection for lazy evaluation
            if (a == outputDirection.Left) {
                success = checkPowerStatus (transform.position, a);
                if (success == true) {
                    return powerStatus = true;
                }
            } else if (a == outputDirection.Right) {
                success = checkPowerStatus (transform.position, a);
                if (success == true) {
                    return powerStatus = true;
                }

            } else if (a == outputDirection.Up) {
                success = checkPowerStatus (transform.position, a);
                if (success == true) {
                    return powerStatus = true;
                }

            } else if (a == outputDirection.Down) {
                success = checkPowerStatus (transform.position, a);
                if (success == true) {
                    return powerStatus = true;
                }
            }
        }
        return powerStatus;
    }

    


}
