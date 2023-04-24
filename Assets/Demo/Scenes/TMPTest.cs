using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPTest : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = "啊啊啊啊啊啊啊啊啊啊";
    }

    private void Update()
    {
        Debug.Log(tmp.textInfo.lineInfo[0].characterCount);
    }
}
