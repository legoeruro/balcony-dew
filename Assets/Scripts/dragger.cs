using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragger : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 lastPos;
    public bool isDown = false;
    bool draggable = true;
    bool undragPress = false;
    void Start()
    {
    }

    private Vector3 mousePos()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(p.x, p.y);
    }

    private void OnMouseDown()
    {
        lastPos = mousePos();

        print(isDown);
        undragPress = true;
        isDown = true;
    }

    private void OnMouseUp()
    {
        isDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDown && draggable)
        {
            Vector3 dPos = mousePos() - lastPos;
            gameObject.transform.position = gameObject.transform.position + dPos;
            lastPos = mousePos();
        }
    }

    public void setDraggable(bool b)
    {
        draggable = b;
        undragPress = false;
    }

    public bool getState()
    {
        return undragPress;
    }


}
