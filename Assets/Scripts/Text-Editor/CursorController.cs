using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Main script for having a cursor when typing.

[RequireComponent(typeof(Text))]
public class CursorController : MonoBehaviour
{
    // Basic config settings for the cursor.
    public float cursorFlashTime = 0.75f;
    public char cursorChar = '█';
    public bool cursorActive;

    private float countdownTime;
    private Text cursorTextElement;

    private void Start()
    {
        cursorTextElement = GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        //If key pressed, show cursor; if not, start countdown.
        if (Input.anyKey)
        {
            countdownTime = Time.time;
            ToggleCursor(true);
        }
        else if (Time.time - countdownTime >= cursorFlashTime)
        {
            ToggleCursor(!cursorActive);
            countdownTime = Time.time;
        }
    }

    private void ToggleCursor(bool toggle)
    {
        // Simple toggle for the cursor element in the text, either adds or removes cursor from text element.
        if (toggle && (cursorTextElement.text.Length == 0 || cursorTextElement.text[cursorTextElement.text.Length-1] != cursorChar))
        {
            cursorTextElement.text += cursorChar;
            cursorActive = true;
        }
        else if (!toggle && (cursorTextElement.text.Length >= 1 && cursorTextElement.text[cursorTextElement.text.Length - 1] == cursorChar))
        {
            cursorTextElement.text = cursorTextElement.text.Remove(cursorTextElement.text.Length - 1);
            cursorActive = false;
        }
    }


}
