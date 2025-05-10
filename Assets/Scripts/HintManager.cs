using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    [SerializeField]
    private bool giveVagueHintFirst = true;
    [SerializeField]
    private GameObject vagueHintUI;
    [SerializeField]
    private GameObject explicitHintUI;
    [SerializeField]
    private float visibilityTime = 8.0f;
    private Dictionary<GameObject, HintPiece> triangleHintPairs = new Dictionary<GameObject, HintPiece>();
    private PuzzleScript puzzleScript;
    private bool hintActive = false;
    private Coroutine vagueHintRoutine;

    void Start()
    {
        if (vagueHintUI) vagueHintUI.SetActive(false);
        if (explicitHintUI) explicitHintUI.SetActive(false);

        GameObject puzzleObject = GameObject.FindWithTag("puzzle");
        if (puzzleObject) puzzleScript = puzzleObject.GetComponent<PuzzleScript>();

        GameObject[] triangles = GameObject.FindGameObjectsWithTag("triangle");
        foreach (GameObject triangle in triangles)
        {
            HintPiece hintPiece = triangle.GetComponentInChildren<HintPiece>();
            triangleHintPairs.Add(triangle, hintPiece);
            hintPiece.AssignToManager(this);
            hintPiece.Detatch();
        }
    }

    public void RequestHint()
    {
        if (hintActive)
        {
            if (vagueHintUI && vagueHintUI.activeInHierarchy) CancelVagueHint();
            else return;
        }

        PlayerStatistics.HintUsed();
        if (giveVagueHintFirst) BeginVagueHintRoutine();
        else BeginExplicitHintRoutine();
    }

    private void BeginVagueHintRoutine()
    {
        giveVagueHintFirst = false;
        vagueHintRoutine = StartCoroutine(ActivateVagueHint());
    }

    private void CancelVagueHint()
    {
        giveVagueHintFirst = false;
        StopCoroutine(vagueHintRoutine);
        if (vagueHintUI) vagueHintUI.SetActive(false);
        hintActive = false;
    }

    private IEnumerator ActivateVagueHint()
    {
        hintActive = true;
        if (vagueHintUI) vagueHintUI.SetActive(true);
        yield return new WaitForSeconds(visibilityTime);
        if (vagueHintUI) vagueHintUI.SetActive(false);
        hintActive = false;
    }

    private void BeginExplicitHintRoutine()
    {
        GameObject unsolvedPiece = puzzleScript.GetUnsolvedPiece();
        if (unsolvedPiece)
        {
            HintPiece unsolvedHintPiece = null;
            triangleHintPairs.TryGetValue(unsolvedPiece, out unsolvedHintPiece);
            if (unsolvedHintPiece)
            {
                unsolvedHintPiece.ActivateHint(ref unsolvedPiece);
                ToggleExplicitHintUI();
            }
        }        
    }

    public void ToggleExplicitHintUI()
    {
        if (!explicitHintUI) return;

        if (hintActive) explicitHintUI.SetActive(false);
        else explicitHintUI.SetActive(true);
        hintActive = !hintActive;
    }
}
