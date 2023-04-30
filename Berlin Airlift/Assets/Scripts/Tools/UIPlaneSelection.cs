using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIPlaneSelection : Singleton<UIPlaneSelection>
{
    //public List<GameObject> CardSlots = new List<GameObject>();
    public GameObject UITemplate;

    
    private void Start()
    {
        
        for(int i = 0; i < USAFCommand.Instance.TransportInventory.Count; i++)
        {
            GameObject planeUISlot = Instantiate(UITemplate);
            planeUISlot.transform.SetParent(this.transform);
            RectTransform rectTransform = planeUISlot.GetComponent<RectTransform>();
            //planeUISlot.transform.position = new Vector3(UICalculateCoordX(0, 1), 200, 0);
            rectTransform.anchoredPosition = new Vector3(UICalculateCoordX(i, 1), -200, 0);


            Button selectButton = planeUISlot.transform.Find("Select").GetComponent<Button>();
            int planeIndex = i;
            selectButton.onClick.AddListener(() => PlaneSelection(planeIndex));
        }
        
    }

    private static int UICalculateCoordX(int index, int count)
    {
        return 200 + index * 200;
    }

    private void PlaneSelection(int planeIndex)
    {
        USAFCommand.Instance.SelectDeployment(planeIndex);
        
    }




}
