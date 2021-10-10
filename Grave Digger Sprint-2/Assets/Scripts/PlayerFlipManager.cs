using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlipManager : MonoBehaviour
{
    public GameObject FlipObject;
    private float TurningRate = 30.0f;
    private Quaternion _TargetRotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        SetBlendedEulerAngles(new Vector3(-90,0,0));
    }

    public void SetBlendedEulerAngles(Vector3 angles)
    {
        _TargetRotation = Quaternion.Euler(angles);
    }

    // Update is called once per frame
    void Update()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        if (HorizontalInput > 0) {
            SetBlendedEulerAngles(new Vector3(-90, 0, 0));
        }
        else if (HorizontalInput < 0) { 
            SetBlendedEulerAngles(new Vector3(-90, 0, -180));
        }
        FlipObject.transform.rotation = Quaternion.RotateTowards(FlipObject.transform.rotation, _TargetRotation, TurningRate * Time.deltaTime* 80);

    }
}
