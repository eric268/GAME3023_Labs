using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public static int value = 0;
    int c = 0;
    // Start is called before the first frame update
    void Start()
    {
        value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            c = 1;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            c = 2;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            c = 3;
        }

        switch (c)
        {

        }


    }
}
