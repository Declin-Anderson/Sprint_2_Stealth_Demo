using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCamMove : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    //[SerializeField] private Camera Cam;
    public float speed = 1.5f;
    private float StartPosition;
    public float OffSet = 5f;
    // Start is called before the first frame upda  te
    void Start()
    {
        //StartPosition = Player.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Player.transform.position - new Vector3(0, Player.transform.position.y - transform.position.y, OffSet), speed * Time.deltaTime);


    }
}
