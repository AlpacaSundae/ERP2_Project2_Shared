// Code adapted from and using libraries provided by: https://github.com/keijiro/HandPoseBarracuda
// This script uses the MPH Unity implementation and provides finger angles
// It also provides thumb tip distance to each finger tip
//
//  Modified: 11/03/2023 by Jaicob Schott
//      new version using updated HPB version

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Klak.TestTools;
using MediaPipe.HandPose;



public class FingerTracker : MonoBehaviour
{

    #region Interfacing

    public enum AngleDef
    {
        Thumb1,  Thumb2,  Thumb3,
        Index1,  Index2,  Index3,
        Middle1, Middle2, Middle3,
        Ring1,   Ring2,   Ring3,
        Pinky1,  Pinky2,  Pinky3,
    }
    
    // Attributes
    [SerializeField] ImageSource _source = null;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] bool _useAsyncReadback = true;
    [Space]
    [SerializeField] float _minConfidence = 0.75f;

    // Public outputs
    public int _desiredHandedness = 0;
    public int handedness;
    public bool confidence;
    public float[] angles = new float[JointTriples.GetLength(0)];
    public float[] distances = new float[FingerTip.Length];
    public float palmAngle;

    #endregion

    #region Internals

    public Vector3 getPoint(int ii)
        => _pipeline.GetKeyPoint(ii);

    // Private
    HandPipeline _pipeline;
    static readonly (int, int, int)[] JointTriples =
    {
        (0,1,2), (1,2,3), (2,3,4),// Thumb
        // MCP, PIP, DIP
        (0, 5, 6), ( 5, 6, 7), ( 6, 7, 8),// Index
        (0, 9,10), ( 9,10,11), (10,11,12),// Middle
        (0,13,14), (13,14,15), (14,15,16),// Ring
        (0,17,18), (17,18,19), (18,19,20)// Pinky
    };
    static readonly int ThumbTip = 4;
    static readonly int[] FingerTip = {8, 12, 16, 20};
    static readonly int AngleSmoothing = 15;   // count of angle calculations to keep memory of

    float[,] angle_hist = new float[AngleSmoothing, JointTriples.GetLength(0)];
    int angle_idx = 0; // which entry currently used in angle array

    // returns an array of all finger angles
    void getFingerAngles()
    {
        for (var ii = 0; ii < JointTriples.GetLength(0); ii++)
        {
            // find vectors from joint outwards to get angle between 
            var v1 = _pipeline.GetKeyPoint(JointTriples[ii].Item1) - _pipeline.GetKeyPoint(JointTriples[ii].Item2);
            var v2 = _pipeline.GetKeyPoint(JointTriples[ii].Item3) - _pipeline.GetKeyPoint(JointTriples[ii].Item2);

            angle_hist[angle_idx,ii] = Vector3.Angle(v1, v2);
        }
    }

    // smooths angles using average of the calculated angle history
    // AngleSmoothing defines number of previous angles to store
    void smoothFingerAngles()
    {
        for (var ii = 0; ii < JointTriples.GetLength(0); ii++)
        {
            angles[ii] = angle_hist[0,ii] / AngleSmoothing;
            for (var jj = 1; jj < AngleSmoothing; jj++)
            {
                angles[ii] += angle_hist[jj,ii] / AngleSmoothing;
            }
        }

        angle_idx = (angle_idx + 1) % AngleSmoothing;
    }

    // calculates teh angle between palms plane and desired palm position plane
    void getPalmAngles()
    {
        // uses three points to find the palm's plane
        var p1 = new Plane(_pipeline.GetKeyPoint(0), _pipeline.GetKeyPoint(5), _pipeline.GetKeyPoint(17));

        // this represents the normal the palm's plane should be aligned with
        var v1 = new Vector3(0.0f, 0.0f, -1.0f);    

        palmAngle = Vector3.Angle(p1.normal, v1);

        if (_pipeline.getHandedness() == 0)
        {
            palmAngle = 1 - (palmAngle - 180);
        }
        
    }

    // returns thumb distance to each finger
    void distanceToThumbTip()
    {
        var thumbPos = _pipeline.GetKeyPoint(ThumbTip);
        for (var ii = 0; ii < FingerTip.Length; ii++)
            distances[ii] = Vector3.Distance(thumbPos, _pipeline.GetKeyPoint(FingerTip[ii]));
    }

    #endregion

    #region Runtime

    void Start()
    {
        _pipeline = new HandPipeline(_resources);
        for (var ii = 0; ii < JointTriples.GetLength(0); ii++)
            for (var jj = 0; jj < AngleSmoothing; jj++)
                angle_hist[jj,ii] = 180f;
        smoothFingerAngles();
    }

    void OnDestroy()
        => _pipeline.Dispose();

    void LateUpdate()
    {
        // Feed the input image to the Hand pose pipeline.
        _pipeline.UseAsyncReadback = _useAsyncReadback;
        _pipeline.ProcessImage(_source.Texture);

        handedness = _pipeline.getHandedness();
        confidence = _pipeline.getHandDetected(_minConfidence);
        if ((handedness == _desiredHandedness) & confidence)
        {
            getFingerAngles();
            smoothFingerAngles();
            distanceToThumbTip();
            getPalmAngles();
        }

    }

    #endregion
}
