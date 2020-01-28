using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle Base", menuName = "Puzzle/Puzzle Base")]
public class PuzzleBase : ScriptableObject
{
    public PuzzleQuestion[] puzzleSegments;
}
