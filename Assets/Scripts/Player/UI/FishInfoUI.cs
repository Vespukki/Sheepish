using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishInfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI headerTextmesh;
    [SerializeField] TextMeshProUGUI bodyTextmesh;


    string _header;
    public string header { get { return _header; } set { _header = value; SetLakeName(value); } }
    string _body;
    public string body { get { return _body; } set { _body = value; SetLakeDescription(value); } }

    void SetLakeName(string value)
    {
        headerTextmesh.SetText(value);
    }

    void SetLakeDescription(string value)
    {
        bodyTextmesh.SetText(value);
    }

}

public class FishInfo
{
    public string header;
    public string body;

    public FishInfo(string _header, string _body)
    {
        header = _header;
        body = _body;
    }
}