using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonClear : MonoBehaviour
{
    public TMP_Text t;
    public ReadInput r;
    public Button bc;
    public ButtonEnter be;

    public GameObject inp;
    TMP_InputField field;
    string oldText = null;

    void Start()
    {
        field = inp.GetComponent<TMP_InputField>();
        bc.onClick.AddListener(Clear);
    }

    void Clear()
    {
        be.oldText = null;

        field.Select();
        field.text = null;

        t.text = null;
        r.ReadsInput(null);
    }
}
