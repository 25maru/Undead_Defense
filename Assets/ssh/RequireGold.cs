using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RequireGold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    public void ChangeGold(int buildingGold)
    {
        goldText.text = $"X {buildingGold}";
    }
}
