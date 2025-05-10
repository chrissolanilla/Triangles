using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCheck : MonoBehaviour
{
    [SerializeField]
    private string triangleType = "";
    GameObject puzzle;

    void Start()
    {
        puzzle = GameObject.FindGameObjectWithTag("puzzle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string TriangleType()
    {
        return triangleType;
    }
}
