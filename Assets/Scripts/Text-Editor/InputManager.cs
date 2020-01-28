using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

delegate void CharacterHandler();

//Main Input manager for the game, handles user input and the customn
//Light-weight text editor. Has custom functions for special keys that
//can be expanded to support additional features.
public class InputManager : MonoBehaviour
{
    //Stores the input of the user as a list of characters.
    private List<char> input;

    //Needs access to these items
    //Could make puzzlemanager a required component (?) as they are on
    //the same object.
    public CursorController cursorController;
    public PuzzleManager puzzleManager;

    //Text fields for the display area and command line.
    public Text consoleText;
    public Text inputText;

    //Storage for special character dictionary and public values for 
    //regular character allowance, max characters and the ability to
    //type in the console. The dictionary holds a character and a function
    //cast as a delegate.
    private Dictionary<char, CharacterHandler> specialCharacters;
    public string acceptedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ?";
    public int maxChars = 40;
    public bool canType = false;

    private void Start()
    {
        //Initialising the input list and special character mappings.
        input = new List<char>();
        specialCharacters = new Dictionary<char, CharacterHandler>()
        {
            { '\r', new CharacterHandler(Submit) },
            { '\b', new CharacterHandler(Backspace)},
            { '\u001B', new CharacterHandler(Exit) } 
        };
    }

    private void Update()
    {
        //Loops through each character input every frame using inputString,
        //acts as an input stream for all characters. Checks all characters
        //for validity and performs actions accordingly.
        if (canType && Input.anyKey)
        {
            foreach(char c in Input.inputString)
            {
                CheckChar(c);
                if(input.Count < maxChars)
                    UpdateInputText();
            }
        }
    }

    #region Special Character Functions
    private void Backspace()
    {
        //Code to perform a backspace operation.
        if(input.Count > 0)
            input.RemoveAt(input.Count-1);
    }

    private void Submit()
    {
        //Submits the command / phrase to the puzzlemanager.
        //Writes text to the command line as well.
        if (input.Count > 0)
        {
            string output = CreateString(ref input);
            consoleText.text = consoleText.text + "\n" + output;
            puzzleManager.SubmitAnswer(output);
        }
        input = new List<char>();
    }

    private void Exit()
    {
        Application.Quit();
    }
    #endregion

    #region Utility Functions
    //Converts input list to string.
    private static string CreateString(ref List<char> input)
    {
        string output = "";
        foreach (char c in input)
            output += c;
        return output;
    }

    private void CheckChar(char character)
    {
        //Checks character against special character list and then against the 
        string upperChar = character.ToString().ToUpper();

        if (specialCharacters.ContainsKey(character))
            specialCharacters[character]();
        else if (acceptedCharacters.Contains(upperChar) && input.Count < maxChars)
            input.Add(upperChar[0]);
    }
    #endregion

    #region Display Functions
    void UpdateInputText()
    {
        //Edits input text, keeping cautious of the cursor.
        if (!cursorController.cursorActive)
            inputText.text = CreateString(ref input);
        else
            inputText.text = CreateString(ref input) + cursorController.cursorChar;
    }
    #endregion
}
