using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            gameObject.transform.Rotate(0.0f, 90.0f, 0.0f, Space.World);
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;
        
        if (collidedObject.GetComponent<Element>().ElementType == Element.Type.WATER || collidedObject.GetComponent<Element>().ElementType == Element.Type.AIR)
        {
            isOpen = true;
        }
    }
}
