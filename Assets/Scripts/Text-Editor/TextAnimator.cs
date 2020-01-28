using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[RequireComponent(typeof(Text))]
public class TextAnimator : MonoBehaviour
{
    public static Text targetTextElement;
    public InputManager inputManager;
    private List<TextAnimation> textAnimations = new List<TextAnimation>();
    private bool animating;

    private void Start()
    {
        targetTextElement = GetComponent<Text>();
    }

    // Overloads for adding animations to the queue.
    #region AddAnimation Overloads
    public void AddAnimations(TextAnimation anim) {
        textAnimations.Add(anim);
        if (!animating)
            StartCoroutine(AnimateText());
    }
    public void AddAnimations(List<TextAnimation> anims) {
        textAnimations.AddRange(anims);
        if(!animating)
            StartCoroutine(AnimateText());
    }
    public void AddAnimations(TextAnimation[] anims)
    {
        textAnimations.AddRange(anims);
        if (!animating)
            StartCoroutine(AnimateText());
    }
    #endregion

    // Function for printing the text to the screen.
    //Replaces \\n with \n to allow for special characters.
    private void PrintText(string text)
    {
        targetTextElement.text += "\n" + text.Replace("\\n", "\n");
    }

    //Coroutine for animating the printing of text, functions like a queue.
    IEnumerator AnimateText()
    {
        animating = true;
        inputManager.canType = false;
        while (textAnimations.Count >= 1)
        {
            PrintText(textAnimations[0].text);
            yield return new WaitForSeconds(textAnimations[0].waitTime);
            textAnimations.RemoveAt(0);
        }
        animating = false;
        inputManager.canType = true;
    }


}

//Container class for Text Animation information.

[System.Serializable]
public class TextAnimation
{
    public string text;
    public float waitTime;

    public TextAnimation(){ text = ""; waitTime = 0f; }

    public TextAnimation(string Text, float WaitTime)
    {
        text = Text;
        waitTime = WaitTime;
    }
}


