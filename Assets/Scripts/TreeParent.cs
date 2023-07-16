using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Voxell;
using Voxell.NLP.Parser;
using Voxell.NLP.Util;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

public class TreeParent : MonoBehaviour {

    private static Vector3[] childDirections = {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    private static Quaternion[] childOrientations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    public GameObject prefab;
    public float childScale;
    public string text;
    public Parse[] parse;

    private Parse[] previousParse;
    private Dictionary<string, Transform> dict = new Dictionary<string, Transform>();

    private void Start()
    {
        parse = null;
        previousParse = null;
    }

    /* private void LateUpdate()
    {
        if (parse != null && parse != previousParse)
        {
            Recurse(parse);
            previousParse = parse;
        }
        else if (parse == null && previousParse != null)
        {
            DeleteTree();
        }
    } */

    public void Recurse(Parse p, int offset)
    {
        if (p.Type != MaximumEntropyParser.TokenNode)
        {
            CreateCube(p, offset);
        }

        foreach (Parse childParse in p.GetChildren())
        {
            Recurse(childParse, offset);
        }
    }

    private void CreateCube(Parse p, int offset)
    {
        // instantiate cube except for punctuation
        if (p.Type != ".")
        {
            GameObject g = Instantiate(prefab, this.transform.position, Quaternion.identity);
            g.name = p.Show();
            Vector3 pos = g.transform.position;

            // adjust position relative to parent if not TOP node
            if (p.Type != MaximumEntropyParser.TopNode)
            {
                Parse parent = p.GetCommonParent(p);
                int cc = parent.ChildCount;

                // make transform parent that of parent string in dict
                Transform parTrans = dict[parent.Show()];
                g.transform.parent = parTrans;

                if (cc > 1)
                {
                    g.transform.SetSiblingIndex(parTrans.childCount);
                    int si = g.transform.GetSiblingIndex();
                    float x = 0;

                    if ((cc % 2) == 0)
                    {
                        // even branching
                        x = 0.5f;

                        if ((si % 2) == 0)
                        {
                            // even child indices
                            x += (int) (si / 2) - 1;
                        }
                        else
                        {
                            // odd child indices
                            x += (int) (si / 2);
                        }
                    }
                    else
                    {
                        // odd branching
                        x = (int) (si / 2);
                    }

                    // apply sign WRT parity
                    if ((si % 2) == 0)
                    {
                        x *= -1.0f;
                    }

                    pos.x = x;
                }

                Vector3 childScale = 0.95f * g.transform.parent.localScale;
                g.transform.localScale = childScale;
                pos.y -= 1;
            }
            else
            {
                g.transform.parent = this.transform;
                pos.x += offset * 3;
            }

            g.transform.localPosition = pos;

            if (!dict.ContainsKey(p.Show()))
            {
                dict.Add(p.Show(), g.transform);
            }

            EditCubeText(g, p);
        }
    }

    private void EditCubeText(GameObject g, Parse p)
    {
        TMP_Text txt = g.GetComponentInChildren<TMP_Text>();
        int pCount = 0;
        txt.text = g.name;

        for (int i = 0; i < g.name.Length; ++i)
        {
            if (g.name[i] == '(')
            {
                pCount++;
            }

            if (pCount > 1)
            {
                txt.text = p.Type;
                break;
            }
        }

        txt.text = txt.text.Replace("(", "").Replace(")", "");
    }

    public void DeleteTree()
    {
        foreach (Transform tc in transform)
        {
            Destroy(tc.gameObject);
        }

        dict.Clear();
        previousParse = null;
    }
}