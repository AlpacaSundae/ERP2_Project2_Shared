using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class EMAFilter {
    private double alpha;
    private double previousAverage;

    public EMAFilter(float smoothingFactor) {
        alpha = smoothingFactor;
        previousAverage = 0;
    }

    public double Filter(float value) {
        float currentAverage = alpha * value + (1 - alpha) * previousAverage;
        previousAverage = currentAverage;
        return currentAverage;
    }
}


public class PointSmoothing : MonoBehaviour
{

    //Activates point smoothing
    [SerializeField] public int smoothAngles = 0;
    [SerializeField] public int windowSize = 60;


    // Private
    HandPipeline _pipeline;
    Queue handPoints = new Queue();
    EMAFilter filter(0.2);

    void coordinateToQueue()
    {
        Vector3[] tempArray = new Vector3[21];
        for (var ii = 0; ii < 21; ii++)
        {
            tempArray[ii] = _pipeline.GetKeyPoint(ii)
        }

        if (handPoints.Count() = windowSize)
        {
            handPoints.Dequeue();
        }
        handPoints.Enqueue(_pipeline.GetKeyPoint(tempArray))
    }

    void smoothCoordinate()
    {
        Vector3[] aveArray = new Vector3[21]
        
        foreach (var array in handPoints)
        {
            int counter = 0;
            //obtain average
            foreach (var vector in array)
            {
                aveArray[counter].x += vector.x; 
                aveArray[counter].y += vector.y; 
                aveArray[counter].z += vector.z; 
                counter++;
            }
            
        }
        
        //get window average for each coordinate of each joint within the frame
        for (var i = 0; i < 21; i++)
        {
            aveArray[i].x = filter(aveArray.x/windowSize)
            aveArray[i].y = filter(aveArray.y/windowSize)
            aveArray[i].z = filter(aveArray.z/windowSize)
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/
