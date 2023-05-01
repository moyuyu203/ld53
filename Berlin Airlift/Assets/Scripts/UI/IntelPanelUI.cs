using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntelPanelUI : MonoBehaviour
{
    public List<TextMeshProUGUI> IntelField = new List<TextMeshProUGUI>();
    public Image IntelImage;

    private void Awake()
    {
        IntelField.AddRange(GetComponentsInChildren<TextMeshProUGUI>());
        IntelImage = GetComponentInChildren<Image>();
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
