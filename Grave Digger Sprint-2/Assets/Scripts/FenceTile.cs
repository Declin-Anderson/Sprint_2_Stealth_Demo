using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceTile : MonoBehaviour
{
    // Holds each of the disabled fence objects
    public GameObject FenceUp;
    public GameObject FenceRight;
    public GameObject FenceDown;
    public GameObject FenceLeft;

    public void GenerateFence(int[] connections)
    {
        if (connections[0] == 3)
        {
            FenceUp.SetActive(true);
        }
        if (connections[1] == 3)
        {
            FenceRight.SetActive(true);
        }
        if (connections[2] == 3)
        {
            FenceDown.SetActive(true);
        }
        if (connections[3] == 3)
        {
            FenceLeft.SetActive(true);
        }
    }
}
