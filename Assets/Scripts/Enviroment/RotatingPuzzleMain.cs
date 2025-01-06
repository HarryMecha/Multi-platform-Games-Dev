using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPuzzleMain : MonoBehaviour
{

    [SerializeField] private List<RotatingPuzzle> RotatingPuzzles = new List<RotatingPuzzle>();
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

    // Update is called once per frame
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
