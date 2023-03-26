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
    public bool clear = false;
    public bool run = false;

    public Vector3 getHandPos()
        => _fingerTracker.getPoint(0);

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

    void resetExercise()
    {
        curFinger = 0;
        complete = false;
        clear = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (clear)
            resetExercise();
        else if (!complete & run)
            checkTouching();
    }
}
