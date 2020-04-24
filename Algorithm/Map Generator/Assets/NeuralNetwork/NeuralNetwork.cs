using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace NeuralNet
{
    public class NeuralNetwork 
    {
        private string nextCommands;

      public  NeuralNetwork()
        {
            nextCommands = "";
        }
    
        public string getNextCommands(int[,] sensorData)
        {
             nextCommands=NeuralNetwork.NeuralNEtworkConnection(sensorData);
            // Direction as per North
            // If you are facing North then Left is West
            // Right is East
            
            if (nextCommands.Contains('3'))
            {
                nextCommands = "South";
            }
            else if (nextCommands.Contains('4'))
            {
                nextCommands = "West";
            }
            else if (nextCommands.Contains('2'))
            {
                nextCommands = "East";
            }
            else if (nextCommands.Contains('1'))
            {
                nextCommands = "North";
            }
            else {
                nextCommands = "Error";
            }
            UnityEngine.Debug.Log("Neural Network Prediction :" + nextCommands);
            return nextCommands;
        }
        
        public static string NeuralNEtworkConnection(int[,] sensorData)
        {
           
            var psi = new ProcessStartInfo();
            // Python path and Neural Network Script path 
            psi.FileName = @"C:\Users\Mayank PC\AppData\Local\Programs\Python\Python38-32\python.exe";

            string m_Path;
            //Get the path of the Neural Network Scipt data folder
            m_Path = Application.dataPath+ "/NeuralNetwork/NN.py";

            //Output the Neural Network Script path to the console
            UnityEngine.Debug.Log("dataPath : " + m_Path);
            var script = @m_Path;
            var testData="" ;
            for (int i=0;i<3;i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    testData += sensorData[i, j]+ ",";
                }
            }
            //  UnityEngine.Debug.Log(testData + "testData");
            string dataSet = Application.dataPath + "/NeuralNetwork/Dataset.csv";
            psi.Arguments = $"\"{script}\" \"{testData}\" \"{dataSet}\"";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            var errors = "";
            string results = "";
            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }
            // UnityEngine.Debug.Log("Prediction"+results);
            return results;
        }
    }
}