using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BerlinSupplyIndicator : MonoBehaviour
{
    public TextMeshProUGUI Total;
    public TextMeshProUGUI Current;
    

    
    void LateUpdate()
    {
        Total.text =   "Time to Victory   : " + Berlin.Instance.RemainTimeToWin.ToString();
        Current.text = "Current Supplies  : " + Berlin.Instance.CurrentSupply.ToString();
    }
}
