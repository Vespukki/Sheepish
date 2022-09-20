using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoverableObject : ScriptableObject
{
    [SerializeField] string _discoverName;

    [TextArea(10,20)][SerializeField] string _description;
    public string discoverName => _discoverName;
    public string description => _description;

}
