using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float ms;
    void Update()
    {
        //gameObject.transform.position = new Vector3(player.transform.position.x, 0f, -10f);
        if (Input.GetKey("a"))
        {
            Vector3 p = new Vector3(-1f, 0f, 0f);
            transform.position += p*ms*Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            Vector3 p = new Vector3(1f, 0f, 0f);
            transform.position += p * ms * Time.deltaTime;
        }
    }
}
