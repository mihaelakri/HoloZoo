using UnityEngine;

public class Dropdown : MonoBehaviour
{
    public GameObject dropdownItem;
    public GameObject parentObject;
    public GameObject alternateObject;

    private float y;
    private bool isExpanded = false;

    public void pushDown()
    {
        if (isExpanded)
        {
            // re-parent item to alternate object
            dropdownItem.transform.SetParent(alternateObject.transform, true);
            // move item off screen
            dropdownItem.transform.position = new Vector3(563, -255, 0);
        }
        else
        {
            // re-parent item to dropdown header
            dropdownItem.transform.SetParent(parentObject.transform, true);
            // adjust item position in hierarchy
            dropdownItem.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex() + 1);
        }

        // move following dropdowns up/down
        foreach (Transform child in transform.parent.gameObject.transform)
        {
            if (child.GetSiblingIndex() > transform.GetSiblingIndex())
            {
                y = child.transform.localPosition.y + (67 * (isExpanded ? 1 : -1));
                child.transform.localPosition = new Vector3(0, y, 0);
            }
        }

        // flip arrow
        gameObject.transform.GetChild(0).transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
        isExpanded = !isExpanded;
    }
}