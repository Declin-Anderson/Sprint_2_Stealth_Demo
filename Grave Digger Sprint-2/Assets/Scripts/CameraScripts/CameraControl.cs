using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject Parent;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(Parent.transform.position.x, this.transform.position.y, this.transform.position.z);
    }
}
