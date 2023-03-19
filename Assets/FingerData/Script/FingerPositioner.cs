// very direct way of controlling model based on the angle calcualtions
//
// Modified: 12/03/2023 by Jaicob Schott

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static FingerTracker.AngleDef;

public class FingerPositioner : MonoBehaviour
{
    #region Public fields
    [SerializeField] GameObject thumb1;
    [SerializeField] GameObject thumb2;
    [SerializeField] GameObject thumb3;
    [SerializeField] GameObject index1;
    [SerializeField] GameObject index2;
    [SerializeField] GameObject index3;
    [SerializeField] GameObject middle1;
    [SerializeField] GameObject middle2;
    [SerializeField] GameObject middle3;
    [SerializeField] GameObject ring1;
    [SerializeField] GameObject ring2;
    [SerializeField] GameObject ring3;
    [SerializeField] GameObject pinky1;
    [SerializeField] GameObject pinky2;
    [SerializeField] GameObject pinky3;
    [Space]
    [SerializeField] FingerTracker landmarkDetector;

    #endregion

    #region Privates
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        var thumb1Angle = landmarkDetector.angles[(int)Thumb1];
        var thumb2Angle = landmarkDetector.angles[(int)Thumb2];
        var thumb3Angle = landmarkDetector.angles[(int)Thumb3];

        thumb1.transform.localRotation = Quaternion.Euler(20, -60, 130-thumb1Angle);
        thumb2.transform.localRotation = Quaternion.Euler(0, 0, 180-thumb2Angle);
        thumb3.transform.localRotation = Quaternion.Euler(0, 0, 180-thumb3Angle);

        var index1Angle = landmarkDetector.angles[(int)Index1];
        var index2Angle = landmarkDetector.angles[(int)Index2];
        var index3Angle = landmarkDetector.angles[(int)Index3];

        index1.transform.localRotation = Quaternion.Euler(180-index1Angle, 0, 0);
        index2.transform.localRotation = Quaternion.Euler(180-index2Angle, 0, 0);
        index3.transform.localRotation = Quaternion.Euler(180-index3Angle, 0, 0);

        var middle1Angle = landmarkDetector.angles[(int)Middle1];
        var middle2Angle = landmarkDetector.angles[(int)Middle2];
        var middle3Angle = landmarkDetector.angles[(int)Middle3];

        middle1.transform.localRotation = Quaternion.Euler(180-middle1Angle, 0, 0);
        middle2.transform.localRotation = Quaternion.Euler(180-middle2Angle, 0, 0);
        middle3.transform.localRotation = Quaternion.Euler(180-middle3Angle, 0, 0);

        var ring1Angle = landmarkDetector.angles[(int)Ring1];
        var ring2Angle = landmarkDetector.angles[(int)Ring2];
        var ring3Angle = landmarkDetector.angles[(int)Ring3];

        ring1.transform.localRotation = Quaternion.Euler(180-ring1Angle, 0, 0);
        ring2.transform.localRotation = Quaternion.Euler(180-ring2Angle, 0, 0);
        ring3.transform.localRotation = Quaternion.Euler(180-ring3Angle, 0, 0);

        var pinky1Angle = landmarkDetector.angles[(int)Pinky1];
        var pinky2Angle = landmarkDetector.angles[(int)Pinky2];
        var pinky3Angle = landmarkDetector.angles[(int)Pinky3];

        pinky1.transform.localRotation = Quaternion.Euler(180-pinky1Angle, 0, 0);
        pinky2.transform.localRotation = Quaternion.Euler(180-pinky2Angle, 0, 0);
        pinky3.transform.localRotation = Quaternion.Euler(180-pinky3Angle, 0, 0);
    }
}