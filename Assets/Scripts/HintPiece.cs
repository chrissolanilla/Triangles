using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPiece : MonoBehaviour
{
    [SerializeField]
    private string triangleType = "";
    [SerializeField]
    private float postOverlapHintTime = 2.5f;
    [SerializeField]
    bool leniantOverlapTest = false;

    private bool readyToDeactivate = false;
    private HintManager hintManager = null;
    private SpriteRenderer hintPieceRenderer;
    private GameObject associatedPuzzlePiece = null;
    private List<GameObject> overlappingObjects;

    void Start()
    {
        overlappingObjects = new List<GameObject>();
        hintPieceRenderer = GetComponent<SpriteRenderer>();
        if (hintPieceRenderer) hintPieceRenderer.enabled = false;
    }

    private void Update()
    {
        if (hintPieceRenderer && hintPieceRenderer.enabled) AttemptToDeactivateHint();
    }

    public void Detatch()
    {
        gameObject.transform.parent = null;
    }

    public void AssignToManager(HintManager manager)
    {
        hintManager = manager;
    }

    public void ActivateHint(ref GameObject associatedPiece)
    {
        if (hintPieceRenderer && hintPieceRenderer.enabled) return;
        if (IsCurrentlyOverlapped()) 
        {
            HintPiece synonymousHint = FindSynonymousHint();
            if (synonymousHint)
            {
                synonymousHint.ActivateHint(ref associatedPuzzlePiece);
                return;
            }
        }
        
        associatedPuzzlePiece = associatedPiece;
        hintPieceRenderer.enabled = true;
    }

    private void AttemptToDeactivateHint()
    {
        if (!readyToDeactivate && IsCurrentlyOverlapped()) StartCoroutine(DeactivateHint());
    }

    private IEnumerator DeactivateHint()
    {
        readyToDeactivate = true;
        yield return new WaitForSeconds(postOverlapHintTime);
        if(IsCurrentlyOverlapped())
        {
            if(hintPieceRenderer) hintPieceRenderer.enabled = false;
            if (hintManager) hintManager.ToggleExplicitHintUI();
        }
        readyToDeactivate = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PuzzleCheck puzzleComp = collision.gameObject.GetComponent<PuzzleCheck>();
        if (puzzleComp && puzzleComp.TriangleType() == triangleType) overlappingObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        overlappingObjects.Remove(collision.gameObject);
    }

    public bool IsCurrentlyOverlapped()
    {
        foreach(GameObject overlappingObject in overlappingObjects)
        {
            float dist = Vector3.Distance(gameObject.transform.position, overlappingObject.transform.position);
            Debug.Log(dist);
            if (dist < 0.2 || (dist < 0.4 && leniantOverlapTest)) return true;
        }

        return false;
    }

    public string TriangleType()
    {
        return triangleType;
    }

    private HintPiece FindSynonymousHint()
    {
        GameObject[] triangles = GameObject.FindGameObjectsWithTag("hint");
        foreach (GameObject triangle in triangles)
        {
            HintPiece hintPiece = triangle.GetComponentInChildren<HintPiece>();
            if (hintPiece && !hintPiece.IsCurrentlyOverlapped() && triangleType == hintPiece.TriangleType()) return hintPiece;
        }
        return null;
    }
}
