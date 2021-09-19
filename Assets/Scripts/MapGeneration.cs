using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public GameObject PylonParent;

    public void GenerateInit()
    {
        Vector3 position = new Vector3(5f, Random.Range(-3, 3), 0f);
        Instantiate(PylonParent, position, Quaternion.identity);
        position = new Vector3(10f, Random.Range(-3, 3), 0f);
        Instantiate(PylonParent, position, Quaternion.identity);
        position = new Vector3(15f, Random.Range(-3, 3), 0f);
        Instantiate(PylonParent, position, Quaternion.identity);
    }

    void Start()
    {
        //initial generation
        GenerateInit();
    }


}
