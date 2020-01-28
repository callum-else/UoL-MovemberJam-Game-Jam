using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the puzzle handling in the game.
public class PuzzleManager : MonoBehaviour
{
    // Neccessary Components.
    public InputManager inputManager;
    public TextAnimator textAnimator;
    public MainManager mainManager;
    public CameraShake cameraShake;
    public MusicManager musicManager;

    //Puzzle for booting sequence.
    public PuzzleBase startupSequence;
    public PuzzleBase gameOverSequenceWin;
    public PuzzleBase gameOverSequenceLose;

    public int targetPuzzles = 10;

    // Private variables for storing semi-perminant information.
    private Object[] allPuzzles = new PuzzleBase[] { };
    private List<int> usedPuzzles = new List<int>();
    private PuzzleBase currentPuzzle;
    private int currentQuestionIndex;

    private void Start()
    {
        //Gets all of the puzzles from resources folder, waits for 20 frames, then
        //continues with the program. Ensures everything is loaded before calls are made.
        //Janky as fuck but it works - I wrote this at like 4am leave me alone.
        allPuzzles = Resources.LoadAll("Puzzles", typeof(PuzzleBase));
        for (int i = 0; i < 20; i++)
            continue;

        StartPuzzle(startupSequence);
        PlayQuestion();
    }

    private PuzzleBase GetPuzzle()
    {
        //Randomly returns a puzzle.
        int randIndex = Random.Range(0, allPuzzles.Length);
        if (!usedPuzzles.Contains(randIndex))
        {
            usedPuzzles.Add(randIndex);
            return allPuzzles[randIndex] as PuzzleBase;
        }
        else if (usedPuzzles.Count == allPuzzles.Length || usedPuzzles.Count >= targetPuzzles)
        {
            inputManager.canType = false;
            mainManager.WinGame();
            return gameOverSequenceWin;
        }
        return GetPuzzle();
    }

    public void SubmitAnswer(string input)
    {
        if (usedPuzzles.Count == 0)
            mainManager.showBanner();
        // Checks if the submitted answer is correct
        if (currentPuzzle.puzzleSegments[currentQuestionIndex].CheckAnswer(input))
        {
            mainManager.AddTime();
            // Increments question or gets new puzzle if correct.
            textAnimator.AddAnimations(new TextAnimation(currentPuzzle.puzzleSegments[currentQuestionIndex].successMessage, 0.5f));
            if (currentPuzzle.puzzleSegments.Length != 1 && currentQuestionIndex < currentPuzzle.puzzleSegments.Length - 1)
                currentQuestionIndex++;
            else
                StartPuzzle();
        }
        else
        {
            //Displays failure message, and starts a new puzzle.
            textAnimator.AddAnimations(new TextAnimation(currentPuzzle.puzzleSegments[currentQuestionIndex].failureMessage, 0.5f));
            if (usedPuzzles.Count > 0) { usedPuzzles.RemoveAt(usedPuzzles.Count - 1); }
            cameraShake.ShakeCam(1f, 0.05f);
            musicManager.PlayExplosion();
            StartPuzzle();
        }

        // Plays new question.
        PlayQuestion();
    }

    public void StartPuzzle()
    {
        // Gets a random puzzle and starts it.
        currentPuzzle = GetPuzzle();
        currentQuestionIndex = 0;
    }

    public void StartPuzzle(PuzzleBase puzzle)
    {
        // Gets a pre-selected puzzle and starts it.
        currentPuzzle = puzzle;
        currentQuestionIndex = 0;
    }

    private void PlayQuestion()
    {
        // Begins the puzzle animation.
        textAnimator.AddAnimations(currentPuzzle.puzzleSegments[currentQuestionIndex].animations);
    }
}
