using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseDisplay : MonoBehaviour
{
    [SerializeField] PoseDetector_ThumbOs _detector;
    [Space]
    [SerializeField] GameObject  _base;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _base.transform.position = _detector.getHandPos();
    }
}
