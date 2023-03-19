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
  [SerializeField] int _desiredHandedness = 1;
  [Space]

  // Public outputs
  public int handedness;
  public float[] angles = new float[JointTriples.GetLength(0)];
  public float[] distances = new float[FingerTip.Length];

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

  // returns an array of all finger angles
  // uses "Mediapipe_New.py" from Yuqing Shi as reference
  void getFingerAngles()
  {
    for (var ii = 0; ii < JointTriples.GetLength(0); ii++)
    {
      // find vectors from joint outwards to get angle between 
      var v1 = _pipeline.GetKeyPoint(JointTriples[ii].Item1) - _pipeline.GetKeyPoint(JointTriples[ii].Item2);
      var v2 = _pipeline.GetKeyPoint(JointTriples[ii].Item3) - _pipeline.GetKeyPoint(JointTriples[ii].Item2);

      angles[ii] = Vector3.Angle(v1, v2);
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

  void Start()
  {
    _pipeline = new HandPipeline(_resources);
    for (var ii = 0; ii < JointTriples.GetLength(0); ii++)
      angles[ii] = 180f;
  }

  void OnDestroy()
    => _pipeline.Dispose();

  void LateUpdate()
  {
    // Feed the input image to the Hand pose pipeline.
    _pipeline.UseAsyncReadback = _useAsyncReadback;
    _pipeline.ProcessImage(_source.Texture);

    handedness = _pipeline.getHandedness();
    if (handedness == _desiredHandedness)
    {
      getFingerAngles();
      distanceToThumbTip();
    }
  }
}
