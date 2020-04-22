using System;
using System.Diagnostics;
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
    
        public string getNextCommands()
        {
            return nextCommands=NeuralNetwork.NeuralNEtworkConnection();
        }
        
        static string NeuralNEtworkConnection()
        {
           
            var psi = new ProcessStartInfo();
            // Python path and Neural Network Script path 
            psi.FileName = @"C:\Users\Mayank PC\AppData\Local\Programs\Python\Python38-32\python.exe";
            var script = @"F:\NN.py";
            var testData = " ";
            var end = " ";
            psi.Arguments = $"\"{script}\" \"{testData}\" \"{end}\"";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            var errors = "";
            var results = "";
            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }
            
            return results;
        }
    }
}