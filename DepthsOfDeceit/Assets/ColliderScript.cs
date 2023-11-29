using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision) {

        transform.parent.GetComponent<ShapeScript>().OnTriggerEnter2DChild(collision);
    }
}
