using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseDetector_ThumbOs : MonoBehaviour
{
    //later could add an input to select which fingers to run test on
    [SerializeField] FingerTracker _fingerTracker;
    [SerializeField] double _holdTime;
    [SerializeField] bool _awaitNext;

    // allows for fine tuning the distance between thumb and finger tip required for considering a touch
    static readonly double[] touchDistance = {0.07, 0.08, 0.08, 0.08}; // index, middle, ring, pinky
    static readonly int[,] angleIndex = {{3,4},{6,7},{9,10},{12,13}};
    static readonly double[,] angleValue = {{100, 100},{100, 100},{100, 100},{100, 100}};
    static readonly double tolerance = 45; // value in degrees, tolerance for accepted angle
    // thumb tip distance calculating vars
    static readonly int ThumbTip = 4;
    static readonly int[] FingerTip = {8, 12, 16, 20};

    public float[] distances = new float[FingerTip.Length];
    double prevHoldTimer = 0;
    
    // public items
    public double[] score = new double[angleIndex.GetLength(0)];
    public int curFinger = 0;
    public double holdTimer = 0;
    public bool complete = false;
    public bool clear = false;
    public bool run = false;

    public Vector3 getHandPos()
        => _fingerTracker.getPoint(0);

    // returns thumb distance to each finger
    void distanceToThumbTip()
    {
        var thumbPos = _fingerTracker.getPoint(ThumbTip);
        for (var ii = 0; ii < FingerTip.Length; ii++)
            distances[ii] = Vector3.Distance(thumbPos, _fingerTracker.getPoint(FingerTip[ii]));
    }

    void checkTouching()
    {
        if (distances[curFinger] < touchDistance[curFinger])
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > _holdTime)
            {
                curFinger += 1;
                holdTimer = 0;
                run = ! _awaitNext;
            }
            if (curFinger >= touchDistance.Length)
            {
                complete = true;
                curFinger = -1;
            }
        }
        else
        {
            holdTimer = 0;
        }
    }

    // score currently is checked by average over hold timer incrementing.
    // for each angle in the correct range a 1 is given, out of range a 0
    // done for each increment of the hold timer
    void checkScoring()
    {
        if (holdTimer > 0)
        {
            var angleCount = angleIndex.GetLength(1);
            for (var ii=0; ii<angleCount; ii++)
            {
                var angle = _fingerTracker.angles[angleIndex[curFinger, ii]];
                var desired = angleValue[curFinger, ii];
                if ((desired - tolerance < angle) & (angle < desired + tolerance))
                {
                    score[curFinger] += (Time.deltaTime/_holdTime) * (1.0/angleCount);
                }
            }
        }
        else
        {
            score[curFinger] = 0.0;
        }
    }

    void resetExercise()
    {
        curFinger = 0;
        complete = false;
        clear = false;
        score = new double[angleIndex.GetLength(0)];
    }

    // Start is called before the first frame update
    void Start()
    {
        clear = true;
        run = false;
    }

    void Update()
    {
        if (clear)
            resetExercise();
        else if (!complete & run & _fingerTracker.confidence)
        {
            distanceToThumbTip();
            checkTouching();
            if (curFinger != -1)
                checkScoring();
        }

        prevHoldTimer = holdTimer;
    }
}
