using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorVisual : MonoBehaviour
{
    [SerializeField] private Texture2D cursorPoint;
    [SerializeField] private Texture2D cursorClose;
    [SerializeField] private Texture2D cursorOpen;


    void Start()
    {
        UpdateCursor("point");
    }

    public void UpdateCursor(string type)
    {
        switch (type)
        {
            case "point":
                Cursor.SetCursor(cursorPoint, Vector2.zero, CursorMode.Auto);
                break;
            case "close":
                Cursor.SetCursor(cursorClose, Vector2.zero, CursorMode.Auto);
                break;
            case "open":
                Cursor.SetCursor(cursorOpen, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}
