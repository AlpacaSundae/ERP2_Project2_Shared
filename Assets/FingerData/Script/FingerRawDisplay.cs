// Optional script, mainly for dev purposes to directly draw a hand using the
// positional data. Uses adapted implementation from HPB's "HandAnimator.cs"
//
// Modified: 12/03/2023 by Jaicob Schott
// Modified: 3/25/2023 by Samuel Tan -> Edited colouring + poses

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class FingerRawDisplay : MonoBehaviour
{
    [SerializeField] FingerTracker landmarkDetector;
    [Space]
    [SerializeField] Mesh _jointMesh = null;
    [SerializeField] Mesh _boneMesh = null;
    [Space]

    [SerializeField] Material _desiredMaterial = null;
    [SerializeField] Material _undesiredMaterial = null;
    [SerializeField] Material _ignoredMaterial = null;
    [SerializeField] Material _boneMaterial = null;
    [Space]
    //[SerializeField] int selectedPose = 0;
    //[Space]
    //[SerializeField] int tolerance = 25;
    [SerializeField] ButtonUI button;

    Matrix4x4 CalculateJointXform(Vector3 pos)
        => Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * 0.07f);

    Matrix4x4 CalculateBoneXform(Vector3 p1, Vector3 p2)
    {
        var length = Vector3.Distance(p1, p2) / 2;
        var radius = 0.03f;

        var center = (p1 + p2) / 2;
        var rotation = Quaternion.FromToRotation(Vector3.up, p2 - p1);
        var scale = new Vector3(radius, length, radius);

        return Matrix4x4.TRS(center, rotation, scale);
    }

    static readonly (int, int)[] BonePairs =
    {
        (0, 1), (1, 2), (1, 2), (2, 3), (3, 4),     // Thumb
        (5, 6), (6, 7), (7, 8),                     // Index finger
        (9, 10), (10, 11), (11, 12),                // Middle finger
        (13, 14), (14, 15), (15, 16),               // Ring finger
        (17, 18), (18, 19), (19, 20),               // Pinky
        (0, 17), (2, 5), (5, 9), (9, 13), (13, 17)  // Palm
    };

    //pose test for foward facing 
    // joints: 5,6,7, 
    //        9,10,11, 
    //        13,14,15
    //        17,18,19
    // should all be arround 170, 180 degrees
    // unsure of joints:
    //         2,3 (thumb)

    //the thumb angles are ignored for now

    static readonly int[,] poseType =
    {
        //nothing selected
        {
            (-1), (-1), (-1), (-1), (-1),    //Thumb + Palm
            (-1),(-1),(-1),(-1),           //Index finger
            (-1),(-1),(-1),(-1),           //Middle finger
            (-1),(-1),(-1),(-1),           //Ring finger
            (-1),(-1),(-1),(-1)           //Pinky
        },


        //flat hand
        {
            (-1), (-1), (-1), (-1), (-1),    //Thumb + Palm
            (170),(170),(170),(-1),           //Index finger
            (170),(170),(170),(-1),           //Middle finger
            (170),(170),(170),(-1),           //Ring finger
            (170),(170),(170),(-1)           //Pinky
        },

        //claw
        {
            (-1), (-1), (-1), (-1), (-1),    //Thumb + Palm
            (160),(145),(135),(-1),           //Index finger
            (160),(130),(130),(-1),           //Middle finger
            (165),(123),(135),(-1),           //Ring finger
            (160),(138),(135),(-1)           //Pinky
        },

        //closed
        {
            (-1), (-1), (-1), (-1), (-1),    //Thumb + Palm
            (25),(160),(120),(-1),           //Index finger
            (70),(95),(105),(-1),           //Middle finger
            (105),(80),(100),(-1),           //Ring finger
            (90),(105),(105),(-1)           //Pinky
        }

    };


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var layer = gameObject.layer;

        if ((landmarkDetector._desiredHandedness == landmarkDetector.handedness) && (landmarkDetector.palmAngle < 45))
        {
            StreamWriter writer = new StreamWriter("C:/Users/samue/OneDrive/Curtin Uni/Thesis/ERP2_Project2_Shared/Assets/test.csv", true );
            //Joint balls
            var counter = 3;
            for (var i = 0; i < 21; i++)
            {
                
                if (poseType[button.selectedPose, i] != -1)
                {
                    writer.Write(landmarkDetector.angles[counter]);
                    print(landmarkDetector.angles[counter]);
                    writer.Write(",");
                    if ((landmarkDetector.angles[counter] >= poseType[button.selectedPose, i] - button.tolerance ) && (landmarkDetector.angles[counter] <= poseType[button.selectedPose, i] + button.tolerance )  )
                    {
                        var xform = CalculateJointXform(landmarkDetector.getPoint(i));
                        Graphics.DrawMesh(_jointMesh, xform, _desiredMaterial, layer);
                        counter++;
                    }
                    else
                    {
                        var xform = CalculateJointXform(landmarkDetector.getPoint(i));
                        Graphics.DrawMesh(_jointMesh, xform, _undesiredMaterial, layer);
                        counter++;
                    }
                }
                else
                {
                    
                    var xform = CalculateJointXform(landmarkDetector.getPoint(i));
                    Graphics.DrawMesh(_jointMesh, xform, _ignoredMaterial, layer);
                }
            }
            writer.Write(System.Environment.NewLine);
            writer.Close();
            // Bones
            foreach (var pair in BonePairs)
            {
                var p1 = landmarkDetector.getPoint(pair.Item1);
                var p2 = landmarkDetector.getPoint(pair.Item2);
                var xform = CalculateBoneXform(p1, p2);
                Graphics.DrawMesh(_boneMesh, xform, _boneMaterial, layer);
            }

        }

        
    }
}
