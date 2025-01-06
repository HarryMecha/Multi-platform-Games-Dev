using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPuzzleMain : MonoBehaviour
{
    #region Fields
    [SerializeField] private List<RotatingPuzzle> RotatingPuzzles = new List<RotatingPuzzle>();
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Transform[] puzzleHoles = GetComponentsInChildren<Transform>();
        Debug.Log(puzzleHoles.Length);
        // Iterate through each child Transform and check if it has the name "Puzzle Hole"
        foreach (Transform puzzleHole in puzzleHoles)
        {
            if (puzzleHole.name == "PuzzleHole")
            {
                // Get the RotatingPuzzle component attached to the PuzzleHole object
                RotatingPuzzle rotatingPuzzle = puzzleHole.GetComponent<RotatingPuzzle>();
                if (rotatingPuzzle != null)
                {
                    RotatingPuzzles.Add(rotatingPuzzle);
                }
            }

        }
    }

    // This will check whether all the puzzleHoles have had their targets hit by harpoons
    void Update()
    {
        int puzzlesStopped = 0;
        foreach (RotatingPuzzle puzzle in RotatingPuzzles)
        {
            if (puzzle.getCanRotate() == false)
            {
                puzzlesStopped++;
            }
        }
        if (puzzlesStopped == RotatingPuzzles.Count)
        {
            Destroy(gameObject);
        }
    }
}
