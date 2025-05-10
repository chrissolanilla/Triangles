using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private bool _mouseOver = false;
    private bool _modified = false;
    private float _angleIncrement = 15.0f;
    private bool hotkeysLocked = false;
    public bool flippedHorizontal = false;
    public bool flippedVertical = false;
    DragDrop movementControlScript;

    private void Start()
    {
        movementControlScript = GetComponent<DragDrop>();
    }

    void Update()
    {
        hotkeysLocked = Selectable.selectionActive;
        if (!hotkeysLocked && _mouseOver && Input.GetKeyDown(KeyCode.H))
        {
            FlipHorizontal();
        }
        if (!hotkeysLocked && _mouseOver && Input.GetKeyDown(KeyCode.V))
        {
            FlipVertical();
        }
        if (!hotkeysLocked && _mouseOver && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateLeft();
        }
        else if (!hotkeysLocked && _mouseOver && Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateRight();
        }
        else if(!hotkeysLocked && !_mouseOver && _modified)
        {
            if (movementControlScript) movementControlScript.StartCoroutine("RequestPositionConfirmation");
            _modified = false;
        }
    }

    public void FlipVertical()
    {
        flippedVertical = !flippedVertical;
        transform.RotateAround(transform.position, Vector3.right, 180.0f);
        GetComponent<DragDrop>().FlipOffsetVertical();
    }

    public void FlipHorizontal()
    {
        flippedHorizontal = !flippedHorizontal;
        transform.RotateAround(transform.position, Vector3.up, 180.0f);

        GetComponent<DragDrop>().FlipOffsetHorizontal();
    }

    public void RotateLeft()
    {
        transform.RotateAround(transform.position, Vector3.forward, _angleIncrement);
        GetComponent<DragDrop>().RotateOffset(_angleIncrement);
        _modified = true;
    }

    public void RotateRight()
    {
        transform.RotateAround(transform.position, Vector3.forward, -_angleIncrement);
        GetComponent<DragDrop>().RotateOffset(-_angleIncrement);
        _modified = true;
    }

    private void OnMouseEnter()
    {
        _mouseOver = true;
    }
    private void OnMouseExit()
    {
        _mouseOver = false;
    }
}
