using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonEnter : MonoBehaviour
{
    public TMP_Text t;
    public ReadInput r;
    public Button b;

    public string oldText = null;

    void Start()
    {
        b.onClick.AddListener(Enter);
    }

    void Enter()
    {
        if (t.text.Length > 1 && t.text != oldText)
        {
            r.ReadsInput(t.text);
            oldText = t.text;
        }
    }
}
