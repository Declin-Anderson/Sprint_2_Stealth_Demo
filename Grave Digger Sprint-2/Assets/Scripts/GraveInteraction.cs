using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using cakeslice;

public class GraveInteraction : MonoBehaviour
{
    public GameObject OutlineObject;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OutlineObject.GetComponent<Outline>().color = 1;
    }

    private void OnTriggerExit(Collider other)
    {
        OutlineObject.GetComponent<Outline>().color = 0;
    }
}
