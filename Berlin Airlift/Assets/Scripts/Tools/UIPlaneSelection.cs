using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct PanelHolder
{
    public Image background;
    public Button select;
    public Button intel;
}
public class UIPlaneSelection : Singleton<UIPlaneSelection>
{
    //public List<GameObject> CardSlots = new List<GameObject>();
    public GameObject UITemplate;
    public IntelPanelUI IntelPanel;

    
    private List<PanelHolder> m_planeUISlots = new List<PanelHolder>();
    
    private void Start()
    {
        
        for(int i = 0; i < USAFCommand.Instance.TransportInventory.Count; i++)
        {
            PanelHolder panelHolder = new PanelHolder();

            GameObject planeUISlot = Instantiate(UITemplate);
            planeUISlot.transform.SetParent(this.transform);
            RectTransform rectTransform = planeUISlot.GetComponent<RectTransform>();
            //planeUISlot.transform.position = new Vector3(UICalculateCoordX(0, 1), 200, 0);
            rectTransform.anchoredPosition = new Vector3(UICalculateCoordX(i, 1), -100, 0);
            
            //Find Background image 
            Image background = planeUISlot.transform.Find("Background").GetComponent<Image>();
            panelHolder.background = background;


            Button selectButton = planeUISlot.transform.Find("Select").GetComponent<Button>();
            int planeIndex = i;
            selectButton.onClick.AddListener(() => PlaneSelection(planeIndex));
            panelHolder.select = selectButton;  

            Button intelButton = planeUISlot.transform.Find("Intel").GetComponent<Button>();
            TransportCount tCount = USAFCommand.Instance.TransportInventory[planeIndex];
            PlaneInfo intel = tCount.plane.GetComponent<PlaneInfo>();
            
            intelButton.onClick.AddListener(() => ShowIntel(intel, planeIndex));
            panelHolder.intel = intelButton;


            m_planeUISlots.Add(panelHolder);

        }
        
    }

    private void Update()
    {
        for(int i = 0; i < m_planeUISlots.Count; i++)
        {
            if(i == USAFCommand.Instance.PlaneIndex)
            {
                m_planeUISlots[i].background.gameObject.SetActive(true);
            }
            else
            {
                m_planeUISlots[i].background.gameObject.SetActive(false);
            }
        }
    }
    private static int UICalculateCoordX(int index, int count)
    {
        return 1000 + index * 200;
    }

    private void PlaneSelection(int planeIndex)
    {
        USAFCommand.Instance.SelectDeployment(planeIndex);
        
    }

    public void ShowIntel(PlaneInfo intel, int intelIndex)
    {
        Debug.Log("Show intel panel.");
        if (intel)
        {
            IntelPanel.gameObject.SetActive(true);
            //TextMeshProUGUI nameSlot = IntelPanel.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            //nameSlot.text = intel.name;
            IntelPanel.IntelField[0].text = "Name  : " + intel.name;
            IntelPanel.IntelField[1].text = "Speed : "  + intel.Speed;
            IntelPanel.IntelField[2].text = "Range : " + intel.Range;
            IntelPanel.IntelField[3].text = "Payload : " + intel.Payload;
            IntelPanel.IntelField[4].text = "Number Remain: " + USAFCommand.Instance.TransportInventory[intelIndex].count.ToString();
        }

    }




}
