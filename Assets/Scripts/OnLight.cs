using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLight : MonoBehaviour
{

    private Light light;
    public bool isEnabled;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        isEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            light.enabled = true;
        }
        else
        {
            light.enabled = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            isEnabled = !isEnabled;
        }
    }

}
