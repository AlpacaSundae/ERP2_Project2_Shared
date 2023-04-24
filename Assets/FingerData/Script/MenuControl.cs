using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] Button[] _buttonList; 
    [SerializeField] FingerTracker _fingerTracker;
    [SerializeField] double _holdDuration = 0.5; // amount of time (seconds) to hold button for
    [Space]
    [SerializeField] Color _defaultColour = Color.white;
    [SerializeField] Color _selectColour = Color.grey;
    [SerializeField] Color _pressColour = Color.green;
    [Space]
    [SerializeField] Camera _camera; 

    private Vector4[] buttonPos;
    public double timer = 0;
    private int buttonIdx = -1;

    void Start()
    {
        buttonPos = new Vector4[_buttonList.Length];
        for (int ii = 0; ii < _buttonList.Length; ii++)
        {
            // Get the position and scale coordinates of each button
            var button = _buttonList[ii];  
            RectTransform rectTransform = button.GetComponent<RectTransform>();

            // Calculate the bounding box via opposing corners
            buttonPos[ii] = new Vector4(
                button.transform.position.x + rectTransform.rect.width/2*rectTransform.localScale.x,   //x1         ----------------o (x1,y1)
                button.transform.position.x - rectTransform.rect.width/2*rectTransform.localScale.x,   //x2         |               |
                button.transform.position.y + rectTransform.rect.height/2*rectTransform.localScale.y,  //y1         |               |
                button.transform.position.y - rectTransform.rect.height/2*rectTransform.localScale.y   //y2 (x2,y2) o---------------- 
            );
        } 
    }

    // Update is called once per frame
    void Update()
    {
        // get the index coordinates
        Vector3 selPos = _camera.WorldToScreenPoint(_fingerTracker.getPoint(8));

        for (int ii = 0; ii < _buttonList.Length; ii++)
        {
            Image buttonImage = _buttonList[ii].GetComponent<Image>();
            // setting the corners of the button
            var pos = buttonPos[ii];
            if ((pos.x > selPos.x) & (pos.y <  selPos.x) & (pos.z >  selPos.y) & (pos.w <  selPos.y))
            {
                // prevents holding on one button to influence the next
                if (buttonIdx != ii)
                {
                    buttonImage.color = _selectColour;
                    buttonIdx = ii;
                    timer = 0;
                }
                // click when hold time satisfied
                else if (timer >= _holdDuration)
                {
                    buttonImage.color = _pressColour;
                    _buttonList[ii].onClick.Invoke();
                    timer = -1; // stop timer while still held but not clicking
                }
                // else increment timer (excluding timer = -1 case)
                else if (! (timer < 0))
                {
                    timer += Time.deltaTime;
                }
            }
            else
            {
                buttonImage.color = _defaultColour;
            }

        }
        
    }
}
