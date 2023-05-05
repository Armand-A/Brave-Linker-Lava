using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapHandler : MonoBehaviour
{
    Collider2D col;


    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool checkTouch(Vector2 cursorPos)
    {
        return col == Physics2D.OverlapPoint(cursorPos) ? true : false;

    }
}
