using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





public class SphereController : MonoBehaviour
{
    public Button button;
    public FingerTracker _fingerTracker;

    //colour change on highlight
    public Color newColor;
    public Color defaultColor;

    //Gettign coordinates of the canvas relative to the parent
    public Canvas myCanvas;
    
    // The speed at which the sphere follows the cursor
    public float speed = 10f;

    // The camera used to render the scene
    public Camera mainCamera;

    // Update is called once per frame
    void Update()
    {
        RectTransform canvasRect = myCanvas.GetComponent<RectTransform>();
        Vector3 canvasPosition = canvasRect.localPosition;

        Image buttonImage = button.GetComponent<Image>();

        RectTransform rectTransform = button.GetComponent<RectTransform>();
        //print(rectTransform);

        //print(button.transform.position);

        //get the 2d button position relative to the parent
        //the 553 is distance relative to the caamera
        //Vector3 ButtonPosition = new Vector3(-canvasPosition.x + button.transform.position.x, - canvasPosition.y + button.transform.position.y, 0);

        // Get the current position of the mouse cursor
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a position in the game world
        Vector3 targetPosition = new Vector3(mousePosition.x, mousePosition.y,553);

        // Calculate the direction to move the sphere towards the target position
        //Vector3 direction = (targetPosition - transform.position).normalized;
        // Move the sphere in the direction of the target position
        //transform.position += direction * speed * Time.deltaTime;
        //Vector3 indexDIP = ScreenToWorldPoint(_fingerTracker.getPoint(8));
        Vector3 indexDIP = mainCamera.WorldToScreenPoint(_fingerTracker.getPoint(8));
        //indexDIP.x *= -3000;
        //indexDIP.y *= 3000;
        //indexDIP.z = 500;
        transform.position = indexDIP;
        print(indexDIP);

        //transform.position = targetPosition;

        //print(targetPosition);

        //print(Vector3.Distance(transform.position, OpenHandButtonPos));
        //print((button.transform.position));
        //print(ButtonPosition);
        //print(mainCamera.ScreenToWorldPoint(button.transform.position));
        //print(canvasRect.localPosition);

        float x1 = button.transform.position.x + rectTransform.rect.width/2*rectTransform.localScale.x;
        float x2 = button.transform.position.x - rectTransform.rect.width/2*rectTransform.localScale.x;
        float y1 = button.transform.position.y + rectTransform.rect.height/2*rectTransform.localScale.y;
        float y2 = button.transform.position.y - rectTransform.rect.height/2*rectTransform.localScale.y;

        if ((x1 > transform.position.x) & (x2 < transform.position.x) & (y1 > transform.position.y) & (y2 < transform.position.y))
        {
            button.onClick.Invoke();
            buttonImage.color = newColor;
            
        }
        else
        {
            buttonImage.color = defaultColor;
        }

        
    }
}