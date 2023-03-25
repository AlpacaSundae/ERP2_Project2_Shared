// Optional script, mainly for dev purposes to directly draw a hand using the
// positional data. Uses adapted implementation from HPB's "HandAnimator.cs"
//
// Modified: 12/03/2023 by Jaicob Schott
// Modified: 3/25/2023 by Samuel Tan -> Edited colouring + poses

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerRawDisplay : MonoBehaviour
{
    [SerializeField] FingerTracker landmarkDetector;
    [Space]
    [SerializeField] Mesh _jointMesh = null;
    [SerializeField] Mesh _boneMesh = null;
    [Space]
    [SerializeField] Material _relevantMaterial = null;
    //[SerializeField] Material _boneMaterial = null;
    //[SerializeField] Material _desiredMaterial = null;
    //[SerializeField] Material _undesiredMaterial = null;
    [SerializeField] Material _ignoredMaterial = null;
    [SerializeField] Material _boneMaterial = null;

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

    static readonly int[] FlatPose =
    {
        (-1), (-1), (180),(180), (-1),    //Thumb + Palm
        (180),(180),(180),(-1),           //Index finger
        (180),(180),(180),(-1),           //Middle finger
        (180),(180),(180),(-1),           //Ring finger
        (180),(180),(180),(-1)           //Pinky
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var layer = gameObject.layer;

        //Joint balls
        for (var i = 0; i < 21; i++)
        {
            if (FlatPose[i] != -1)
            {
  

                //Console.WriteLine(FingerTracker.angles.Array.data[0]);
                var xform = CalculateJointXform(landmarkDetector.getPoint(i));
                Graphics.DrawMesh(_jointMesh, xform, _relevantMaterial, layer);

            }
            else
            {
                var xform = CalculateJointXform(landmarkDetector.getPoint(i));
                Graphics.DrawMesh(_jointMesh, xform, _ignoredMaterial, layer);
            }
        }

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
