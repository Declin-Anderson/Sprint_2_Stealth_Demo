using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealth2 : MonoBehaviour
{
    public int AlertLevel = 0;
    int NoiseLevel = 0;
    public bool AlertDecrease = false;
    

    bool DecayStarted = false;
    int DecayTime = 60;

    void update()
    {
        if(Input.GetKeyDown("f"))
        {
            AlertIncrease(1);
        }
       
    }
    


   public int AddNoise(int noise)
   {
        NoiseLevel += noise;
        //adds noise from others scripts
        return 0; 
   }
    public bool NoiseReset()
    {
        //resets noise level
        NoiseLevel = 0;
        return true;
    }
    public int AlertIncrease(int Alert)
    {
        AlertLevel += Alert;
        return 0;
        //increases alert level when called
    }

    public int AlertDecay()
    {
        DecayStarted = true;
        StartCoroutine("Decay");
        return 0;
        //starts a count down of 60sec
    }

    IEnumerator Decay()
    {
        // counts down for decay time in secs
        yield return new WaitForSeconds(DecayTime);
        //checks to see if Alert level is zero just incase silly things happen
        if(AlertLevel != 0)
        {
            AlertLevel -= 1;
        }
        DecayStarted = false;
        //allows another decay to be called
    }
}
