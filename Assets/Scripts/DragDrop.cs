using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private enum MovementState{
        Stationary,
        Dragging,
        AutoPositioning,
        ManualPositioning,
        RequestingConfirmation
    }

    [SerializeField] private bool mouseOver;
    private bool viewOnlyMode = false;
    private Vector3 offset;

    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidBody; 
    private PolygonCollider2D _collider;
    private int _defaultSortingLayer = 0;
    private int _dragSortingLayer = 5;
    private int _ignoreRaycastLayer;
    private int _defaultRacastLayer;
    private Vector2 _destination;
    private MovementState _state = MovementState.Stationary;
    [SerializeField] float _maxOverlapAmount = 0.02f;
    [SerializeField] float _manualPositionSpeed = 5f;
    [SerializeField] bool _startStationary = true;
    private Selectable _selectionControlScript;

    private GameObject _puzzle;

    void Awake()
    {
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _collider = gameObject.GetComponent<PolygonCollider2D>();
        _selectionControlScript = GetComponent<Selectable>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        _ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
        _defaultRacastLayer = gameObject.layer;
        _sprite.sortingOrder = _defaultSortingLayer;
        _puzzle = GameObject.FindGameObjectWithTag("puzzle");
    }

    void Update()
    {
        if (viewOnlyMode) return;

        //if they click on a docked whiteboard piece, spawn in that triangle to play with
        if (Input.GetMouseButtonDown(0) && mouseOver && gameObject.tag == "docked")
        {
            var newTriangle = gameObject;
            newTriangle = Instantiate(gameObject, new Vector3 ((gameObject.transform.position.x), gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
            newTriangle.tag = "whiteboard";
            newTriangle.GetComponent<DragDrop>().Select();

            gameObject.GetComponent<Selectable>().Unselect();
            newTriangle.GetComponent<Selectable>().Select();
        }

        //moving whiteboard or puzzle triangles
        if (Input.GetMouseButtonDown(0) && mouseOver && (gameObject.tag == "whiteboard" || gameObject.tag == "triangle") )
        {
            Select();
        }

        //if they release the mouse button, begin requesting a non-overlapping position
        if (_state == MovementState.Dragging && Input.GetMouseButtonUp(0))
        {
            _sprite.sortingOrder = _defaultSortingLayer;
            _state = MovementState.AutoPositioning;
            StartCoroutine("RequestPositionConfirmation");
        }

        UpdatePosition();
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
    }
    private void OnMouseExit()
    {
        mouseOver = false;
    }

    public void SetToViewOnly()
    {
        viewOnlyMode = true;
    }

    public void Select()
    {
        mouseOver = true;
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _sprite.sortingOrder = _dragSortingLayer;
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _state = MovementState.Dragging;
    }

    private void UpdatePosition()
    {
        if (_state == MovementState.Dragging && _selectionControlScript && !_selectionControlScript.MightBeDragging())
        {
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }

        if (_selectionControlScript && _selectionControlScript.IsSelected()) return;
        else if (_state == MovementState.ManualPositioning) ManualMovement();
        else if (_state == MovementState.RequestingConfirmation) CheckForPositionLock();
    }

    //give the physics system only a fraction of a second to figure it out, then prepare to take over
    public IEnumerator RequestPositionConfirmation()
    {
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.3f);
        _state = MovementState.RequestingConfirmation;
    }

    //check to see if this piece is not overlapping anything, and is therefore ready to have its position locked again.
    private void CheckForPositionLock()
    {
        Collider2D[] colliders = new Collider2D[10];
        int colliderCount = _collider.OverlapCollider(new ContactFilter2D(), colliders);
        if (colliderCount != 0) RequestOverlapSFX();

        //if it's failed to get even close to not overlapping, just manually take over
        for (int i = 0; i < colliderCount; ++i)
        {
            ColliderDistance2D distance = Physics2D.Distance(_collider, colliders[i]);
            if (Mathf.Abs(distance.distance) > _maxOverlapAmount)
            {
                _state = MovementState.ManualPositioning;
                _destination = CalculateNewPosition(colliders[i], distance);
                break;
            }
        }

        //if it's succeeded in finding a non overlapping position, re-freeze the object.
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        if (_state == MovementState.RequestingConfirmation)
        {
            _state = MovementState.Stationary;
            if (_puzzle != null)
            {
                _puzzle.GetComponent<PuzzleScript>().checkComplete();
            }
        }
    }

    //manually calculate a position where the piece won't overlap with anything
    private Vector2 CalculateNewPosition(Collider2D other, ColliderDistance2D distance)
    {
        Vector2 adjustment = distance.normal * distance.distance;
        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y) + adjustment;

        while (Physics2D.OverlapBox(newPosition, _sprite.size, transform.rotation.z) != null)
        {
            newPosition += adjustment;
        }
        return newPosition;
    }

    //if the phsyics system fails to find a non overlapping position quickly, manually find a position.
    private void ManualMovement()
    {
        transform.position = Vector2.Lerp(transform.position, _destination, Time.deltaTime * _manualPositionSpeed);

        gameObject.layer = _ignoreRaycastLayer;
        Vector2 spriteScale = _sprite.size * new Vector2(transform.localScale.x + _maxOverlapAmount, transform.localScale.y + _maxOverlapAmount);
        bool isStillOverlapping = (Physics2D.OverlapBox(transform.position, spriteScale, transform.rotation.z) != null);
        gameObject.layer = _defaultRacastLayer;

        if (Vector2.Distance(transform.position, _destination) < _maxOverlapAmount || !isStillOverlapping)
        {
            _state = MovementState.Stationary;
            if (_puzzle != null)
            {
                _puzzle.GetComponent<PuzzleScript>().checkComplete();
            }
        }
    }

    public void RotateOffset(float angle)
    {
        offset = Quaternion.AngleAxis(angle, Vector3.forward) * offset;
        UpdatePosition();
    }

    public void FlipOffsetHorizontal()
    {
        offset.x = -offset.x;
        UpdatePosition();
    }

    public void FlipOffsetVertical()
    {
        offset.y = -offset.y;
        UpdatePosition();
    }

    private void RequestOverlapSFX()
    {
        GameObject manager = GameObject.FindWithTag("SoundManager");
        if (manager)
        {
            SoundManager soundManager = manager.GetComponent<SoundManager>();
            if (soundManager) SoundManager.instance.PlayOverlap();
        }
    }
}
