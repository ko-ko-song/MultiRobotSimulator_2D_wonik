using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using UnityEngine;

public class ROSMessageGenerator
{
    public ROSMessageGenerator()
    {

    }

    public void GenerateROSMessages(string inPath, string outPath)
    {
        bool verbose = true;
        Debug.Log("Auto gen");
        // Generate ROS messages from the specified directory
        var warnings = MessageAutoGen.GenerateDirectoryMessages(inPath, outPath, verbose);

        // Print any warnings generated during the message generation process
        if (warnings.Count > 0)
        {
            Debug.LogWarning("Message generation warnings:");
            foreach (var warning in warnings)
            {
                Debug.LogWarning(warning);
            }
        }
        else
        {
            Debug.Log("Message generation completed successfully.");
        }
        
        warnings = ServiceAutoGen.GenerateDirectoryServices(inPath, outPath, verbose);
        if (warnings.Count > 0)
        {
            Debug.LogWarning("service generation warnings:");
            foreach (var warning in warnings)
            {
                Debug.LogWarning(warning);
            }
        }
        else
        {
            Debug.Log("service generation completed successfully.");
        }
        Debug.Log("Auto gen");
    }
    
}
