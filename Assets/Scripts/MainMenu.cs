using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PuzzleGame;

public class MainMenu : MonoBehaviour
{
    public List<Texture2D> images;
    GameObject ImagesButtons;
    GameObject PlayButton;
    GameObject SelectImageButton;
    Puzzle puzzle;
    GameObject puzzleObj;

    void Start()
    {
        ImagesButtons = GameObject.Find("Images");
        PlayButton = GameObject.Find("PlayButton");
        SelectImageButton = GameObject.Find("SelectImage");
        ImagesButtons.SetActive(false);
        ScenePropertirs.GameImage = images[0];
        puzzleObj = new GameObject();
        puzzle = puzzleObj.AddComponent<Puzzle>();
        puzzle.Init();
        
    }

    void Update()
    { 
    }

    public void ToGame()
    {
        SceneManager.LoadScene("Game2D");
    }

    public void ShowImages()
    {
        PlayButton.SetActive(false);
        SelectImageButton.SetActive(false);
        ImagesButtons.SetActive(true);
    }

    public void SelectDragonImage()
    {
        ScenePropertirs.GameImage = images[0];
        ImagesButtons.SetActive(false);
        PlayButton.SetActive(true);
        SelectImageButton.SetActive(true);
        Destroy(puzzleObj);
        puzzleObj = new GameObject();
        puzzle = puzzleObj.AddComponent<Puzzle>();
        puzzle.Init();
    }

    public void SelectSummerImage()
    {
        ScenePropertirs.GameImage = images[1];
        ImagesButtons.SetActive(false);
        PlayButton.SetActive(true);
        SelectImageButton.SetActive(true);
        Destroy(puzzleObj);
        puzzleObj = new GameObject();
        puzzle = puzzleObj.AddComponent<Puzzle>();
        puzzle.Init();
    }
}
