using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleScript : MonoBehaviour
{
    [SerializeField] bool complete = false;
    GameObject[] triangles;
    [SerializeField] int correctTriangles = 0;

    [SerializeField] Collider2D EdgeCollider;
    [SerializeField] Collider2D AreaCollider;

    [SerializeField] Color completeColor;
    [SerializeField] Color incompleteColor;

    [SerializeField]
    private Menu puzzleUIControls;

    // Start is called before the first frame update
    void Start()
    {
        triangles = GameObject.FindGameObjectsWithTag("triangle");
        GetComponent<SpriteRenderer>().color = incompleteColor;
    }

    // Update is called once per frame
    void Update()
    {
        if(complete)
        {
            GetComponent<SpriteRenderer>().color = completeColor;
            if (puzzleUIControls) puzzleUIControls.WinGameUI();
            PlayerStatistics.CompeltePuzzleLevel();
        }
        else
        {
            GetComponent<SpriteRenderer>().color = incompleteColor;
        }
    }

    /// <summary>
    /// Checks if a Puzzle is Complete
    /// All pieces in Puzzle must be inside puzzle
    /// and not touching the edge of the puzzle.
    /// </summary>
    public void checkComplete()
    {
        complete = true;
        Debug.Log("CHECKING COMPLETE:");
        foreach(GameObject tri in triangles)
        {
            Debug.Log("Checking " + tri.name);
            if (EdgeCollider.IsTouching(tri.GetComponent<PolygonCollider2D>()))
            {
                Debug.Log("TOUCHING EDGE");
            }
            if (!AreaCollider.IsTouching(tri.GetComponent<PolygonCollider2D>()))
            {
                Debug.Log("NOT TOUCHING AREA");
            }
            if (EdgeCollider.IsTouching(tri.GetComponent<PolygonCollider2D>()) || !AreaCollider.IsTouching(tri.GetComponent<PolygonCollider2D>()))
            {
                complete = false;
                break;
            }
            else
            {
                Debug.Log("ALL GOOD");
            }
        }
    }

    public GameObject GetUnsolvedPiece()
    {
        //separate for loops to ensure triangles not in the area at all are prioritized
        foreach (GameObject tri in triangles)
        {
            if (!AreaCollider.IsTouching(tri.GetComponent<PolygonCollider2D>())) return tri;
        }
        foreach (GameObject tri in triangles)
        {
            if (EdgeCollider.IsTouching(tri.GetComponent<PolygonCollider2D>())) return tri;
        }

        return null;
    }
}
