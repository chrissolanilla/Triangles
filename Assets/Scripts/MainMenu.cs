using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject PuzzleSelectUI;

    private void Start()
    {
        if(PuzzleSelectUI) PuzzleSelectUI.SetActive(false);
    }
    
    public void PuzzleSelectMenu()
    {
        if (PuzzleSelectUI.activeSelf) PuzzleSelectUI.SetActive(false);
        else PuzzleSelectUI.SetActive(true);
    }
}
