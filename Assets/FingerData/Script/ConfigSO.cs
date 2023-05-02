using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Some settings will need controlling by user in runtime
// using this should allow these settings to persist (given set up correctly)
// some other settings shouldn't be here as they only exist in the editor for testing

[CreateAssetMenu]
public class ConfigSO : ScriptableObject
{
    [SerializeField]
    private bool _mirrored;
    public bool Mirrored
    {
        get { return _mirrored; }
        set { _mirrored = value; }
    }
}