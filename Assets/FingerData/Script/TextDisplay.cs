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
    [Space]
    [SerializeField] TextMeshProUGUI cur_finger;
    [SerializeField] TextMeshProUGUI hold_timer;
    [SerializeField] TextMeshProUGUI status;
    [Space]
    [SerializeField] FingerTracker fintracker;
    [SerializeField] PoseDetector_ThumbOs _thumbos;

    string[] fingers = {"index", "middle", "ring", "pinky"};

    #region ThumbOs

    void displayThumbos()
    {
        if (_thumbos.curFinger == -1)
            cur_finger.text = "nope.avi";
        else
            cur_finger.text = fingers[_thumbos.curFinger];
        hold_timer.text = _thumbos.holdTimer.ToString();
        if (_thumbos.holdTimer > 0)
            hold_timer.color = Color.green;
        else
            hold_timer.color = Color.red;

        if (_thumbos.complete)
        {
            status.text = "Completed exercise !!1!";
            status.color = Color.green;
        }
        else if (_thumbos.run)
        {
            status.text = "Exercise in progress";
            status.color = Color.red;
        }
        else
        {
            status.text = "idle hands";
            status.color = Color.white;
        }
    }

    public void startExercise()
    {
        _thumbos.run = true;
    }

    public void stopExercise()
    {
        _thumbos.clear = true;
        _thumbos.run = false;
    }

    int[] lmao = {1, 0};
    public void swapHands()
    {
        fintracker._desiredHandedness = lmao[fintracker._desiredHandedness];
    }

    #endregion

    void displayTracker()
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

    // Update is called once per frame
    void Update()
    {
        if (fintracker != null)
            displayTracker();

        if (_thumbos != null)
            displayThumbos();
    }
}
