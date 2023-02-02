using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject presentPlayer;
    public GameObject pastPlayer;
    public GameObject postProcessing;

    public LevelBehavior currentLevel;
    public bool isPresent = true;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLevel.ToggleState();
            TogglePlayer();
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



    



}
