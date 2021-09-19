using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonScript : MonoBehaviour
{
    GameObject Camera;
    int HasGenerated = 0;
    private void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Update()
    {
        if(Camera.transform.position.x - gameObject.transform.position.x > 10f)
        {
            //Destroy(gameObject);
        }
        if(Camera.transform.position.x > gameObject.transform.position.x && HasGenerated == 0)
        {
            HasGenerated = 1;
            Vector3 position = new Vector3(gameObject.transform.position.x + 15f, Random.Range(-3, 3), 0f);
            Instantiate(gameObject, position, Quaternion.identity);
        }
    }
}
