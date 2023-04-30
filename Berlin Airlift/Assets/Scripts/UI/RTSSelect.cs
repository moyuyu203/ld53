using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RTSSelect : MonoBehaviour
{
    public RectTransform selectionBox;
    public Canvas canvas;
    public LayerMask selectableLayer;

    private Vector2 startPosition;
    private List<GameObject> selectedObjects;

    void Start()
    {
        selectedObjects = new List<GameObject>();
        selectionBox.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            selectionBox.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
            SelectObjectsInBounds();
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false);
        }
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

        Collider[] selectableColliders = Physics.OverlapBox((max + min) / 2, (max - min) / 2, Quaternion.identity, selectableLayer);

        selectedObjects.Clear();
        foreach (Collider collider in selectableColliders)
        {
            selectedObjects.Add(collider.gameObject);
        }
    }
}
