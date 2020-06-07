using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject cam;
    public GameObject player;
    public Transform camBoundsLeft;
    public Transform camBoundsRight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = cam.transform.position;
        camPos.x = player.transform.position.x;
        if (camPos.x < camBoundsLeft.position.x)
        {
            camPos.x = camBoundsLeft.position.x;
        }
        if (camPos.x > camBoundsRight.position.x)
        {
            camPos.x = camBoundsRight.position.x;
        }
        cam.transform.position = camPos;
    }
}
