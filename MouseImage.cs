using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseImage : MonoBehaviour {

    public Texture2D targetCursor;

    private CursorMode curMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

	void Start () {
        Cursor.SetCursor(targetCursor, hotSpot, curMode);
	}
}
