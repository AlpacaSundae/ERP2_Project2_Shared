//  Additional functions required for project, split into a second class to allow
//  updating HandPoseBarracuda without overwriting extensions  
//
//  Modified: 11/03/2023 by Jaicob Schott
//      Refactored out from project 1

using UnityEngine;

namespace MediaPipe.HandPose {

    partial class HandPipeline
    {
        // Gets the handedness of detected hand
        // -1 = unknown, 0 = right, 1 = left
        public int getHandedness()
        {
            var handedness = -1;
            var hand_tol = 0.025;
            var value = _getHandedness();

            if ((Mathf.Abs(value - 1)) < hand_tol)
            {
                handedness = 0;
            }
            else if ((Mathf.Abs(value)) < hand_tol)
            {
                handedness = 1;
            }

            return handedness;
        }

        private float _getHandedness()
            => _detector.landmark.Handedness;
    }
}