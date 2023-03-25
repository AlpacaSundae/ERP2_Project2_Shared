using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI handiness;
    [SerializeField] TextMeshProUGUI palm_angle;
    [SerializeField] TextMeshProUGUI correct;
    [SerializeField] FingerTracker fintracker;
    
    void Start()
    {
        handiness.text = "bottom text";
    }

    // Update is called once per frame
    void Update()
    {
        if (fintracker.handedness == 1)
        {
                handiness.text =  "Handed?: Left"; 
        }
        else if(fintracker.handedness == 0)
        {
            handiness.text =  "Handed?: Right"; 
        }
        else
        {
            handiness.text =  "Handed?:"; 
        }
      palm_angle.text = "Deviant: " + fintracker.palmAngle.ToString();
      if (fintracker.palmAngle < 45)
      {
            correct.text = "In postion?: Yes";
            correct.color = Color.green;
      }
      else
      {
            correct.text = "In postion?: No";
            correct.color = Color.red;
      }
    }
}
