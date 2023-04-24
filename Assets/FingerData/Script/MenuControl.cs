using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] Button[] buttons; 
    [SerializeField] FingerTracker _fingerTracker;
    [Space]
    [SerializeField] Color newColor;
    [SerializeField] Color defaultColor;
    [Space]
    [SerializeField] Camera mainCamera; 

    private Vector4[] buttonPos; 

    void Start()
    {

        buttonPos = new Vector4[buttons.Length];
        for (int ii=0; ii < buttons.Length; ii++)
        {
            //getting the position and scale coordinates of each button
            var button = buttons[ii];  
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            float x1 = button.transform.position.x + rectTransform.rect.width/2*rectTransform.localScale.x;
            float x2 = button.transform.position.x - rectTransform.rect.width/2*rectTransform.localScale.x;
            float y1 = button.transform.position.y + rectTransform.rect.height/2*rectTransform.localScale.y;
            float y2 = button.transform.position.y - rectTransform.rect.height/2*rectTransform.localScale.y;
            buttonPos[ii] = new Vector4(x1, x2, y1, y2);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        //get the index coordinates
        Vector3 indexDIP = mainCamera.WorldToScreenPoint(_fingerTracker.getPoint(8));

        for (int ii=0; ii <buttons.Length; ii++)
        {
            Image buttonImage = buttons[ii].GetComponent<Image>();
            //setting the corners of the button
            var pos = buttonPos[ii];
            if ((pos.x > indexDIP.x) & (pos.y <  indexDIP.x) & (pos.z >  indexDIP.y) & (pos.w <  indexDIP.y))
            {
                buttons[ii].onClick.Invoke();
                buttonImage.color = newColor;
                
            }
            else
            {
                buttonImage.color = defaultColor;
            }

        }
        
    }
}
