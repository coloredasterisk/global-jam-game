using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateBehavior : MonoBehaviour
{
    private AudioSource clicked;
    public List<GridComponent> turnObjectsOn;
    public List<GridComponent> turnObjectsOff;
    public ShooterBehavior laserShooter;
    public bool isOn = false;

    private void Start()
    {
        clicked = GetComponent<AudioSource>();
        TurnListOff(turnObjectsOn);
        TurnListOn(turnObjectsOff);
    }

    public void DynamicReset()
    {
        isOn = false;
        TurnListOff(turnObjectsOn);
        TurnListOn(turnObjectsOff);
    }

    public void SwitchOn()
    {
        isOn = true;
        TurnListOff(turnObjectsOff);
        TurnListOn(turnObjectsOn);
        clicked.Play();
        laserShooter.CreateLaser();


    }
    public void SwitchOff()
    {
        isOn = false;
        TurnListOff(turnObjectsOn);
        TurnListOn(turnObjectsOff);
        if( clicked != null)
        {
            clicked.Play();
        }
        
    }

    private void TurnListOff(List<GridComponent> list)
    {
        if (list != null)
        {
            foreach (GridComponent component in list)
            {
                component.RemoveFromGrid();
                component.gameObject.SetActive(false);
            }
        }
    }
    private void TurnListOn(List<GridComponent> list)
    {
        if (list != null)
        {
            foreach (GridComponent component in list)
            {
                bool placed = component.AddToGrid(false);
                if (placed)
                {
                    component.gameObject.SetActive(true);
                }
            }
        }
    }

}
