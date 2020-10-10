using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PuzzleGame;

public class MainMenu : MonoBehaviour
{
    public List<Texture2D> images;
    int imageSelectedId;
    GameObject PlayButton;
    Puzzle puzzle;
    GameObject puzzleObj;

    void Start()
    {
        PlayButton = GameObject.Find("PlayButton");
        if (ScenePropertirs.GameImage == null)
        {
            imageSelectedId = 0;
            ScenePropertirs.GameImage = images[imageSelectedId];
        }
        puzzleObj = new GameObject();
        puzzle = puzzleObj.AddComponent<Puzzle>();
        puzzle.Init(Puzzle.GameMode.Demo);
        
    }

    void Update()
    { 
    }

    public void ToGame()
    {
        SceneManager.LoadScene("Game2D");
    }

    public void SelectLeftImage()
    {
        imageSelectedId--;
        if (imageSelectedId < 0)
        {
            imageSelectedId = images.Count - 1;
        }
        selectImage();
        
    }

    public void SelectRightImage()
    {
        imageSelectedId++;
        if (imageSelectedId == images.Count)
        {
            imageSelectedId = 0;
        }
        selectImage();
    }

    private void selectImage()
    {
        ScenePropertirs.GameImage = images[imageSelectedId];
        PlayButton.SetActive(true);
        Destroy(puzzleObj);
        puzzleObj = new GameObject();
        puzzle = puzzleObj.AddComponent<Puzzle>();
        puzzle.Init(Puzzle.GameMode.Demo);
    }
}
