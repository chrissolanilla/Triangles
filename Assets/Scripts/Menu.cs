using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject directionsPopUp;
    public GameObject winnerPopUp;
    public GameObject UIButtons;
    public GameObject defaultTriangles;
    private GameObject[] oldTriangles;
    private bool directionsActive = false;
    private bool solutionView = false;

    private void Start()
    {
        if(directionsPopUp) directionsPopUp.SetActive(false);   
        if(winnerPopUp) winnerPopUp.SetActive(false);
        if(UIButtons) UIButtons.SetActive(true);
    }
    public void ClearBoard()
    {
         oldTriangles = GameObject.FindGameObjectsWithTag("whiteboard");

        for (var i = 0; i < oldTriangles.Length; i++)
        {
            Destroy(oldTriangles[i]);
        }
    }

    public void PopulateBoard()
    {
        ClearBoard();
        Instantiate(defaultTriangles, defaultTriangles.transform.position, defaultTriangles.transform.rotation);
    }

    public void DirectionsPopUp()
    {
        if (!directionsPopUp) return;
        if (directionsActive == false)
        {
            directionsPopUp.SetActive(true);
            UIButtons.SetActive(false);
            directionsActive = true;
        }
        else if (directionsActive == true)
        {
            directionsPopUp.SetActive(false);
            UIButtons.SetActive(true);
            directionsActive = false;
        }
    }

    public void WinGameUI()
    {
        if (winnerPopUp && winnerPopUp.activeSelf || solutionView) return;
        if (directionsPopUp)
        {
            directionsPopUp.SetActive(false);
            directionsActive = false;
        }
        if (UIButtons) UIButtons.SetActive(false);
        if (winnerPopUp) winnerPopUp.SetActive(true);
    }

    public void ViewSolution()
    {
        solutionView = true;
        if (winnerPopUp && winnerPopUp.activeSelf) winnerPopUp.SetActive(false);
        if (UIButtons)
        {
            UIButtons.SetActive(true);
            UnityEngine.UI.Button hintButton = GameObject.Find("Hint Button").GetComponent<UnityEngine.UI.Button>();
            if (hintButton) hintButton.interactable = false;
            UnityEngine.UI.Button directionsButton = GameObject.Find("DirectionsButton").GetComponent<UnityEngine.UI.Button>();
            if (directionsButton) directionsButton.interactable = false;
        }

        DragDrop[] dragDrops = Resources.FindObjectsOfTypeAll<DragDrop>();
        foreach(DragDrop dragDrop in dragDrops)
        {
            dragDrop.SetToViewOnly();
        }
        Selectable[] selectables = Resources.FindObjectsOfTypeAll<Selectable>();
        foreach (Selectable selectable in selectables)
        {
            selectable.SetToViewOnly();
        }
    }
}
