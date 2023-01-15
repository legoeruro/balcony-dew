using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hover : MonoBehaviour
{
    public bool hovering = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnMouseOver()
    {
        hovering = true;
    }
    private void OnMouseExit()
    {
        hovering = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
