using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct InterceptorCount
{
    public Interceptor plane;
    public int count;
}
public class InterceptorBase : Airbase, IMouseInput
{
    public List<InterceptorCount> PlaneInventory = new List<InterceptorCount>();

   
    private void Awake()
    {
        foreach(InterceptorCount pCount in PlaneInventory)
        {
            for (int i = 0; i < pCount.count; i++)
            {
                Plane interceptor = Instantiate(pCount.plane);
                interceptor.SetHomeBase(this);
                LineUp(interceptor);
            }
        }
        
    }

    public Plane ReceiveOrder(Transform target)
    {
        if(RunwayClear && m_takeoffQueue.Count > 0)
        {
            OrderTakeoff();
            m_currTakeoffPlane.Target = target;
            return m_currTakeoffPlane;
        }
        else
        {
            return null;
        }
    }
    public void MouseHover()
    {

    }
    public void MouseClick()
    {
        if (PlaneInventory.Count > 0 && PlaneInventory[0].plane.gameObject)
        {
            IntelPanelUI IntelPanel = SovietCommand.Instance.IntelPanel;
            PlaneInfo intel = PlaneInventory[0].plane.gameObject.GetComponent<PlaneInfo>();
            IntelPanel.gameObject.SetActive(true);
            //TextMeshProUGUI nameSlot = IntelPanel.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            //nameSlot.text = intel.name;
            IntelPanel.IntelField[0].text = "" + intel.Name;
            IntelPanel.IntelField[1].text = "Interceptor";
            IntelPanel.IntelField[2].text = "Speed : " + intel.Speed;
            IntelPanel.IntelField[3].text = "Range : " + intel.Range;
            IntelPanel.IntelField[4].text = "Payload : " + intel.Payload;
            IntelPanel.IntelField[5].text = "Number Remain: " + m_takeoffQueue.Count.ToString();

            IntelPanel.IntelImage.sprite = intel.PlaneImage;
        }
        else
        {
            IntelPanelUI IntelPanel = SovietCommand.Instance.IntelPanel;
            
            IntelPanel.gameObject.SetActive(true);
            //TextMeshProUGUI nameSlot = IntelPanel.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            //nameSlot.text = intel.name;
            IntelPanel.IntelField[0].text = "";
            IntelPanel.IntelField[1].text = "";
            IntelPanel.IntelField[2].text = "";
            IntelPanel.IntelField[3].text = "";
            IntelPanel.IntelField[4].text = "";
            IntelPanel.IntelField[5].text = "";

            //IntelPanel.IntelImage.sprite = intel.PlaneImage;
        }

    }

    

  
}
