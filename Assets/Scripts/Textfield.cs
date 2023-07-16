using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Textfield : MonoBehaviour
{
    TMP_Text gt;

    void Start()
    {
        gt = GetComponent<TMP_Text>();
    }

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            // backspace
            if (c == '\b')
            {
                if (gt.text.Length != 0)
                {
                    gt.text = gt.text.Substring(0, gt.text.Length - 1);
                }
            }
            else
            {
                gt.text += c;
            }
        }
    }
}