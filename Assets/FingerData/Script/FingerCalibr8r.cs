using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FingerCalibr8r : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI handiness;
    [SerializeField] TextMeshProUGUI status;
    [Space]
    [SerializeField] ConfigSO _config;
    
    FingerTracker _fingerTracker;

    private bool locked = false;

    public void setHandedness()
    {
        if (!locked)
        {
            locked = true;
            status.text = "setting hand";
            StartCoroutine( _setHandedness());
        }
    }

    IEnumerator _setHandedness()
    {
        var pos = _fingerTracker.getPoint(0).x;
        yield return new WaitForSeconds(0.5f);

        status.text = "move hand to your left";
        yield return new WaitForSeconds(3f);
        var posL = _fingerTracker.getPoint(0).x;

        status.text = "move hand to your right";
        yield return new WaitForSeconds(3f);
        var posR = _fingerTracker.getPoint(0).x;

        if ((posL < pos) && (posL < posR))
        {
            status.text = "L/R was already correct";
            yield return new WaitForSeconds(0.5f);
        }
        else if ((posL > pos) && (posL > posR))
        {
            _config.Mirrored = ! _config.Mirrored;
            status.text = "L/R has been flipped";
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            status.text = "error setting L/R";
            yield return new WaitForSeconds(0.5f);
        }

        locked = false; //when completed
    }

    void Display()
    {
        if (_fingerTracker.handedness == 1)
            handiness.text =  " Left hand"; 
        else if(_fingerTracker.handedness == 0)
            handiness.text =  "Right hand"; 
        else
            handiness.text =  "~~idk hand"; 
    }

    void Start()
    {
        _fingerTracker = FindObjectOfType<FingerTracker>();
    }

    void Update()
    {
        Display();
    }
}
