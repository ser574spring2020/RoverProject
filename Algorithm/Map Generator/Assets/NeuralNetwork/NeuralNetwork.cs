using System.Diagnostics;
using System.Linq;

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
             nextCommands=NeuralNetwork.NeuralNEtworkConnection();
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
            
            return nextCommands;
        }
        
        public static string NeuralNEtworkConnection()
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
            string results = "";
            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }
            
            return results;
        }
    }
}