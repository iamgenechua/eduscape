using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    public bool shouldOpen;

    // Start is called before the first frame update
    void Start()
    {
        shouldOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldOpen)
        {
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.World);
            shouldOpen = false;
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;
        
        if (collidedObject.GetComponent<Element>().ElementType == Element.Type.WATER || collidedObject.GetComponent<Element>().ElementType == Element.Type.AIR)
        {
            shouldOpen = true;
        }
    }
}
