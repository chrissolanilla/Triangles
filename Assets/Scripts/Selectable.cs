using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public static bool selectionActive = false;
    [SerializeField]
    private int degreeIncrement = 15;
    [SerializeField]
    private float sensativity = 10.0f;
    [SerializeField]
    private GameObject rotationHandle;
    [SerializeField]
    private GameObject UIButtons;
    public LayerMask UILayer;

    private bool _isSelected = false;
    private bool _mouseOver = false;
    private bool viewOnlyMode = false;
    private int _clickCount = 0;
    private float _allowedTimeToClick = 0.3f;
    private float _clickTime = 0.0f;
    private bool _draggingPossible = false;
    private UnityEngine.EventSystems.EventSystem eventSystem;
    private Rotate rotationController;

    void Start()
    {
        if (!_isSelected) //when a docked triangle is spawned in whiteboard mode, it is already selected
        {
            UIButtons.SetActive(false);
            rotationHandle.SetActive(false);
        }
        
        eventSystem = GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();
        rotationController = GetComponent<Rotate>();
    }

    void Update()
    {
        if (viewOnlyMode) return;
        Vector3 UItransform = UIButtons.transform.localPosition;

        if (_mouseOver && Input.GetMouseButtonDown(0)) Select();

        Collider2D _targetCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), UILayer);
        if (_targetCollider && _targetCollider.GetComponentInParent<Selectable>() == this)
        {
            if (Input.GetMouseButtonDown(0)) _draggingPossible = true;
        }

        bool overUI = eventSystem.IsPointerOverGameObject();
        if ( !overUI && _isSelected && !_mouseOver && Input.GetMouseButtonDown(0)) Unselect();

        if (!Input.GetMouseButton(0)) _draggingPossible = false;

        UIButtons.transform.rotation = Quaternion.identity;
        UIButtons.transform.localPosition = Vector3.zero;
    }

    public void SetToViewOnly()
    {
        viewOnlyMode = true;
        UIButtons.SetActive(false);
        rotationHandle.SetActive(false);
    }

    public bool IsSelected() {
        return _isSelected;
    }

    public bool MightBeDragging()
    {
        return _draggingPossible;
    }

    public void Select()
    {
        _isSelected = true;
        selectionActive = true;
        rotationHandle.SetActive(true);
        UIButtons.SetActive(true);
    }

    public void Unselect()
    {
        _isSelected = false;
        selectionActive = false;
        rotationHandle.SetActive(false);
        UIButtons.SetActive(false);
    }

    private void OnMouseEnter() {
        _mouseOver = true;
    }

    private void OnMouseExit() {
        _mouseOver = false;
    }

    private void OnMouseDrag()
    {
        if(_isSelected && _draggingPossible)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 a = rotationHandle.transform.position - transform.position;
            Vector3 b = mousePos - transform.position;
            float difference = AngleDifference(b, a);

            int rotations = Mathf.Abs(Mathf.FloorToInt(difference / degreeIncrement));
            for (int i = 0; i < rotations; ++i)
            {
                if (difference < 0) rotationController.RotateLeft();
                else rotationController.RotateRight();
            }
        }        
    }

    private float AngleDifference(Vector3 a, Vector3 b)
    {
        a.z = 0;
        b.z = 0;
        a = a.normalized;
        b = b.normalized;
        return Vector3.SignedAngle(a, b, Vector3.forward);
    }

    private bool DoubleClick() 
    {
        if (_mouseOver && Input.GetMouseButtonDown(0)) if (++_clickCount == 1) _clickTime = Time.time;

        if (_clickCount > 1 && Time.time - _clickTime <= _allowedTimeToClick) {
            _clickTime = 0.0f;
            _clickCount = 0;
            return true;
        }
        else if (_clickCount > 2 || Time.time - _clickTime > _allowedTimeToClick) _clickCount = 0;
        return false;
    }
}
