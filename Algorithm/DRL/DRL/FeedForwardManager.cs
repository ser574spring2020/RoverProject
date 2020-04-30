using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

namespace Algorithms
{
    public class FeedForwardManager
    {
        NeuralNetwork _net;
        string[] _activation = {"sigmoid", "sigmoid"};

        public int MaxIndex(float[] vector) // helper for Accuracy()
        {
            // index of largest value
            int bigIndex = 0;
            float biggestVal = vector[0];
            for (int i = 0; i < vector.Length; ++i)
            {
                if (!(vector[i] > biggestVal)) continue;
                biggestVal = vector[i];
                bigIndex = i;
            }

            return bigIndex;
        }

        public string GetDirectionFromFeedForward(string sensorType, float[] sensorData, int experimentType)
        {
            int[] layers = new int[3] {9, 12, 4};
            if (sensorType == "Proximity")
            {
                layers = new int[3] {9, 12, 4};
            }
            else if (sensorType == "Radar" || sensorType == "Range")
            {
                layers = new int[3] {25, 12, 4};
            }

            this._net = new NeuralNetwork(layers, _activation);
            if (experimentType == 0)
            {
                StreamReader reader = new StreamReader(File.OpenRead(@"MachineLearning/Backup/" + sensorType + ".csv"));
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');
                    float[] direction = new float[] {0, 0, 1, 0};
                    if (values[9] == "North")
                    {
                        direction = new float[] {1, 0, 0, 0};
                    }
                    else if (values[9] == "East")
                    {
                        direction = new float[] {0, 1, 0, 0};
                    }
                    else if (values[9] == "South")
                    {
                        direction = new float[] {0, 0, 1, 0};
                    }
                    else if (values[9] == "West")
                    {
                        direction = new float[] {0, 0, 0, 1};
                    }

                    float[] vs = new float[9];
                    for (int i = 0; i < 9; i++)
                    {
                        vs[i] = float.Parse(values[i]);
                    }

                    _net.Train(vs, direction);
                }

                _net.Save(sensorType+"Weights.txt");
            }
            else
            {
                _net.Load(sensorType+"Weights.txt");
                float[] op = _net.FeedForward(sensorData);
                int maxIndex = MaxIndex(op);
                String res;
                switch (maxIndex)
                {
                    case 0:
                        res = "North";
                        break;
                    case 1:
                        res = "South";
                        break;
                    case 2:
                        res = "East";
                        break;
                    default:
                        res = "West";
                        break;
                }

                Debug.Log(res);
                return res;
            }

            return "";
        }
    }
}