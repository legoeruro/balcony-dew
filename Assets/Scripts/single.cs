using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class single : MonoBehaviour
{
    // Start is called before the first frame update
    bool pressed = false;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && pressed != true) 
        {
            pressed = true;
            Cursor.SetCursor(Resources.Load<Texture2D>("cat paw - back"), Vector2.zero, CursorMode.Auto); 
        }
        if (!Input.GetMouseButton(0) && pressed != false)
        {
            pressed = false;
            Cursor.SetCursor(Resources.Load<Texture2D>("cat paw - front"), Vector2.zero, CursorMode.Auto);
        }
    }
}
