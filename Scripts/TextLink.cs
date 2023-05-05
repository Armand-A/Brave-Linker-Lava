using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLink : MonoBehaviour
{
    public Transform linkedEnemy;
    public Vector3 offset = new Vector3(0.5f, 0.95f, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = linkedEnemy.position + offset;
    }
}
