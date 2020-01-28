using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Puzzle Question", menuName = "Puzzle/Puzzle Question")]
[System.Serializable]
// Container class for a question, part of a Base.
public class PuzzleQuestion : ScriptableObject
{
    public TextAnimation[] animations;
    public string[] answers;
    public string successMessage;
    public string failureMessage;

    public bool CheckAnswer(string input)
    {
        foreach (string a in answers)
            if (input == a.ToUpper())
                return true;
        return false;
    }
}
