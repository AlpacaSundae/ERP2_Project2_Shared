using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] Button[] _buttonList; 
    [Space]
    [SerializeField] double _holdDuration = 0.5; // amount of time (seconds) to hold button for
    [SerializeField] float _xyRange = 0.75f;
    [Space]
    [SerializeField] GameObject  _pointer;
    [Space]
    [SerializeField] Color _defaultColour = Color.white;
    [SerializeField] Color _selectColour = Color.grey;
    [SerializeField] Color _pressColour = Color.green;
    [Space]
    [SerializeField] Camera _camera; 

    FingerTracker _fingerTracker;
    public double timer = 0;

    private readonly float alpha = 0.1f; // factor to move the cursor in direction of new position
    private Vector4[] buttonPos;
    private int buttonIdx = -1;

    Vector3 centre;
    float scale;
    Canvas canvas;

    void Start()
    {
        _fingerTracker = FindObjectOfType<FingerTracker>();
        canvas = GetComponent<Canvas>();
        if (_buttonList.Length == 0)
            _buttonList = canvas.GetComponentsInChildren<Button>();

        centre = (canvas.transform.position);
        RectTransform rectCanvas = canvas.GetComponent<RectTransform>();
        scale = rectCanvas.rect.width * canvas.transform.localScale.x;
        transform.localScale = canvas.transform.localScale;

        buttonPos = new Vector4[_buttonList.Length];
        for (int ii = 0; ii < _buttonList.Length; ii++)
        {
            // Get the position and scale coordinates of each button
            var button = _buttonList[ii];  
            RectTransform rectTransform = button.GetComponent<RectTransform>();

            // Calculate the bounding box via opposing corners
            buttonPos[ii] = new Vector4(
                button.transform.position.x + rectTransform.rect.width/2*rectTransform.localScale.x*canvas.transform.localScale.x,   //x1         ----------------o (x1,y1)
                button.transform.position.x - rectTransform.rect.width/2*rectTransform.localScale.x*canvas.transform.localScale.x,   //x2         |               |
                button.transform.position.y + rectTransform.rect.height/2*rectTransform.localScale.y*canvas.transform.localScale.y,  //y1         |               |
                button.transform.position.y - rectTransform.rect.height/2*rectTransform.localScale.y*canvas.transform.localScale.y   //y2 (x2,y2) o---------------- 
            );
        } 
    }

    // Update is called once per frame
    void Update()
    {
        // get the index coordinates
        Vector3 selPos = _fingerTracker.getPoint(8);
        selPos.x *= scale/_xyRange;
        selPos.y *= scale/_xyRange;
        selPos += centre;

        //selPos = _camera.WorldToScreenPoint(selPos);

        // set pointer position if pointer exists
        if (!(_pointer == null))
        {
            Vector3 prevPos = _pointer.transform.position;
            Vector3 deltaPos = alpha*(selPos - prevPos);

            if (_fingerTracker.confidence) 
            {
                _pointer.transform.position += deltaPos;
                _pointer.SetActive(true);
            }
            else
                _pointer.SetActive(false);
                //_pointer.transform.position = new Vector3(-50f,-50f,-50f);
        }

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
                
                // timer should only reset if the stored button loses contact (reaches this case)
                if (buttonIdx == ii)
                {
                    timer = 0;
                    buttonIdx = -1;
                }
            }

        }
        
    }
}
