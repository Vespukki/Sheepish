using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bodyTextmesh;
    [SerializeField] TextMeshProUGUI headerTextmesh;
    string _bodyText;
    public string bodyText { get { return _bodyText; } set { _bodyText = value; SetBody(value); } }
    string _headerText;
    public string headerText { get { return _headerText; } set { _headerText = value; SetHeader(value); } }

    void SetBody(string value)
    {
        bodyTextmesh.SetText(value);
    }

    void SetHeader(string value)
    {
        headerTextmesh.SetText(value);
    }
}
