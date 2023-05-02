using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MouseInputManager : MonoBehaviour
{
    public IntelPanelUI IntelPanel;
    public RectTransform selectionBox;
    public Canvas canvas;
    public LayerMask selectableLayer;
    public Camera mainCamera;
    public Waypoint waypoint;

    private Vector2 startPosition;
    public List<Plane> selectedUnits;


    private Vector2 mousePos2D;
    // Update is called once per frame

    private bool m_selecting;
    public bool HasSelectedUnits { get { return selectedUnits.Count != 0; } }
    void Start()
    {
        selectedUnits = new List<Plane>();
        selectionBox.gameObject.SetActive(false);
    }
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            IntelPanel.Hide();
        }

        /*
        if(Input.GetMouseButton(1))
        {
            Debug.Log("Cancel selection=");
            selectedUnits.Clear();
        }
        */
        if (Input.GetMouseButtonDown(1) && !m_selecting)
        {
            StartSelection();
        }
        


        if (HasSelectedUnits && Input.GetMouseButton(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            HandleSelectedUnits(mousePos);
        }
        else if (m_selecting)
        {
            if (Input.GetMouseButton(1))
            {
                UpdateSelectionBox(Input.mousePosition);
                
            }

            if (Input.GetMouseButtonUp(1))
            {
                m_selecting = false;
                SelectObjectsInBounds();
                selectionBox.gameObject.SetActive(false);
            }
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            
            if (hit)
            {
                IMouseInput[] mouseInputs = hit.collider.gameObject.GetComponents<IMouseInput>();
                /*
                if (mouseInput != null)
                {
                    mouseInput.MouseHover();
                    if (Input.GetMouseButtonDown(0))
                    {
                        mouseInput.MouseClick();
                    }
                }
                */
                foreach(IMouseInput mouseInput in mouseInputs)
                {
                    mouseInput.MouseHover();
                }
                if (mouseInputs.Length > 0 && Input.GetMouseButtonDown(0))
                {
                    mouseInputs[0].MouseClick();
                }


            }
            

           
            
        }
    }

    void StartSelection()
    {
        m_selecting = true;
        startPosition = Input.mousePosition;
        selectionBox.gameObject.SetActive(true);
        UpdateSelectionBox(Input.mousePosition);
    }

    void UpdateSelectionBox(Vector2 endPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, startPosition, canvas.worldCamera, out Vector2 localStart);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, endPosition, canvas.worldCamera, out Vector2 localEnd);

        Vector2 boxSize = localEnd - localStart;
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(boxSize.x), Mathf.Abs(boxSize.y));
        selectionBox.anchoredPosition = localStart + boxSize * 0.5f;
    }

    void SelectObjectsInBounds()
    {
        Vector2 min = new Vector2(Mathf.Min(startPosition.x, Input.mousePosition.x), Mathf.Min(startPosition.y, Input.mousePosition.y));
        Vector2 max = new Vector2(Mathf.Max(startPosition.x, Input.mousePosition.x), Mathf.Max(startPosition.y, Input.mousePosition.y));

        //selectedObjects.Clear();

        Vector2 boxSizeInScreenSpace = new Vector2(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y));
        Vector2 boxCenterInWorldSpace = mainCamera.ScreenToWorldPoint((max + min) / 2);
        ///boxCenterInWorldSpace.z = 0; // Reset Z to zero, as the camera can have a non-zero Z value

        // Calculate size in world space using the camera's orthographic size and screen dimensions
        float worldScreenHeight = mainCamera.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight * mainCamera.aspect;
        Vector2 boxSizeInWorldSpace = new Vector2((boxSizeInScreenSpace.x / Screen.width) * worldScreenWidth, (boxSizeInScreenSpace.y / Screen.height) * worldScreenHeight);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenterInWorldSpace, boxSizeInWorldSpace, 0, selectableLayer);

        foreach (Collider2D collider in colliders)
        {
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(collider.transform.position);

            if (screenPoint.x >= min.x && screenPoint.x <= max.x && screenPoint.y >= min.y && screenPoint.y <= max.y)
            {
                //selectedObjects.Add(collider.gameObject);
                //Debug.Log("Do Add unit");
                Plane plane = collider.gameObject.GetComponent<Plane>();
                if(plane && plane.Group == PlaneGroup.Nato && plane.State == PlaneState.OnTask)
                {
                    selectedUnits.Add(plane);
                    Debug.Log("Do Add unit");
                }
            }
        }
    }

    void HandleSelectedUnits(Vector3 mousePosition)
    {
        Debug.Log("Handle Selection");
        Waypoint wp = Instantiate(waypoint, mousePosition, Quaternion.identity);
        foreach(Plane plane in selectedUnits)
        {
            plane.Target = wp.transform;
        }
        selectedUnits.Clear();
    }
}
