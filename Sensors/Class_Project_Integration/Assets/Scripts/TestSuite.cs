using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TestSuite : MonoBehaviour
{



    private string[] inputs;
    // Start is called before the first frame update


    // Update is called once per frame
    public void testUpdateText(string[] inputs)
    {


        Debug.Log("Testing the Inputs to the System");
        Assert.IsNotNull(inputs);
        Debug.Assert(inputs[0].Contains("Back Propagation") || inputs[0].Contains("Forward Propagation") || inputs[0].Contains("Deep Learning NN"), "Invalid Algorithm Type");
        Debug.Assert(inputs[1].Contains("30 X 30") || inputs[1].Contains("30 X 50") || inputs[1].Contains("50 X 50") || inputs[1].Contains("50 X 70"), "Invalid Maze Size");
        Debug.Assert(inputs[2].Contains("Bumper Sensor") || inputs[2].Contains("LiDar Sensor") || inputs[2].Contains("Proximity Sensor") || inputs[2].Contains("Radar Sensor") || inputs[2].Contains("Range Sensor"), "Invalid Sensor Type");
        //Debug.Assert(inputs[3].Contains("Training Data") || inputs[3].Contains("Test Data"));

    }
}


