using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public delegate void OnGoalScored();
    public OnGoalScored onGoalScored;


    // Start is called before the first frame update
    void Start()
    {
        onGoalScored += GoalScored;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void GoalScored(){
        return;
    }


    
}
