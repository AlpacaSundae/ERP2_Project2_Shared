using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] public int selectedPose = 0;
    [Space]
    [SerializeField] public int tolerance = 25;

    // Start is called before the first frame update
    public void ButtonOpenHand()
    {
        if (selectedPose != 1)
        {
            selectedPose = 1;
            print(selectedPose);
        }
        
    }

    public void ButtonClaw()
    {
        if (selectedPose != 2)
        {
            selectedPose = 2;
            print(selectedPose);
        }

    }

    public void ButtonFist() 
    {
        if (selectedPose != 3)
        {
            selectedPose = 3;
            print(selectedPose);
        }

    }
}
