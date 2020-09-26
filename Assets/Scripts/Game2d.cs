using UnityEngine;
using PuzzleGame;

public class Game2d : MonoBehaviour
{
    Puzzle puzzle;

    void Start()
    {
        GameObject gameObject = new GameObject();
        puzzle = gameObject.AddComponent<Puzzle>();
        puzzle.Init();
    }
}