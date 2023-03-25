using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseDetector_ThumbOs : MonoBehaviour
{
    //later could add an input to select which fingers to run test on
    [SerializeField] FingerTracker _fingerTracker;
    [SerializeField] int _holdTime;

    // allows for fine tuning the distance between thumb and finger tip required for considering a touch
    static readonly double[] touchDistance = {0.06, 0.06, 0.06, 0.06}; // index, middle, ring, pinky
    
    public int curFinger = 0;
    public int holdTimer = 0;
    public bool complete = false;

    void checkTouching()
    {
        if (_fingerTracker.distances[curFinger] < touchDistance[curFinger])
        {
            holdTimer += 1;
            if (holdTimer > _holdTime)
            {
                curFinger += 1;
            }
            if (curFinger >= touchDistance.Length)
            {
                complete = true;
                curFinger = 0;
            }
        }
        else
        {
            holdTimer = 0;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (!complete)
            checkTouching();
    }
}