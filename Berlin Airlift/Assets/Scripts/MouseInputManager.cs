using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : MonoBehaviour
{

    private Vector2 mousePos2D;
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit)
        {
            IMouseInput mouseInput = hit.collider.gameObject.GetComponent<IMouseInput>();
            if (mouseInput != null)
            {
                mouseInput.MouseHover();
                if (Input.GetMouseButtonDown(0))
                {
                    mouseInput.MouseClick();
                }


            }
        }
    }
}
