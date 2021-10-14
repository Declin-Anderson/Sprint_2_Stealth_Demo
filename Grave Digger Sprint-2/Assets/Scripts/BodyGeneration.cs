using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyGeneration : MonoBehaviour
{
    //if tresure is in the grave
    bool Treasure = false;
    //random number to see if tresure is in grave
    int TreasureNumber = 0;
    // if body is skeleton
    bool IsSkleton = false;

    //important body parts decay levels 
    //body decay is teh cap of all other organs 
    int DecayLevelBody = 100;
    int DecayLevelBrain = 100;
    int DecayLevelHeart = 100;
    int DecayLevelLiver = 100;
    int DecayLevelLungs = 100;
    int DecayLevelKidneys = 100;

    void GenerateLowTierBody()
    {
        
    }

    void GenerateMidTierBody()
    {

        TreasureNumber = Random.Range(0, 100);
        if(TreasureNumber >= 70)
        {
            Treasure = true;
        }
        
    }

    void GenerateHighTierBody()
    {

    }

}
