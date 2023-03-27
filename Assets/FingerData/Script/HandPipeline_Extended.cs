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
        public static double MIN_DETECT_SCORE = 0.8;
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

        // returns true if score indicates probable detection
        public bool getHandDetected()
        {
            if (_getScore() >= MIN_DETECT_SCORE)
                return true;
            else
                return false;
        }

        private float _getHandedness()
            => _detector.landmark.Handedness;

        private float _getScore()
            => _detector.landmark.Score;
    }
}