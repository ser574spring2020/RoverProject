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

        float encode(String direction)
        {
            return direction == "North" ? 1 :
                direction == "East" ? 2 :
                direction == "South" ? 3 :
                direction == "West" ? 4 : 0;
        }

        string Decode(double direction)
        {
            return direction == 1 ? "North" :
                direction == 2 ? "East" :
                direction == 3 ? "South" :
                direction == 4 ? "West" : "";
        }

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

        public static string HitOrNot(string res, float[] testData, float[] prediction)
        {
            if (res.Equals("North"))
            {
                if (testData[1] == -1 || testData[1] == 1)
                {
                    double locMax = 0;
                    int locMaxInd = 0;
                    for (int i = 0; i < prediction.Length; i++)
                    {
                        if (i != 0)
                        {
                            if (prediction[i] > locMax)
                            {
                                locMax = prediction[i];
                                locMaxInd = i;
                            }
                        }
                    }
                    return locMaxInd == 1
                        ? HitOrNot("South", testData, prediction)
                        : HitOrNot(locMaxInd == 2 ? "East" : "West", testData, prediction);
                }
                return "North";
            }

            if (res.Equals("South"))
            {
                if (testData[7] != -1 && testData[7] != 1) return "South";
                double loc_max = 0;
                int locMaxInd = 0;
                for (int i = 0; i < prediction.Length; i++)
                {
                    if (i != 1)
                    {
                        if (prediction[i] > loc_max)
                        {
                            loc_max = prediction[i];
                            locMaxInd = i;
                        }
                    }
                }
                return locMaxInd == 0
                    ? HitOrNot("North", testData, prediction)
                    : HitOrNot(locMaxInd == 2 ? "East" : "West", testData, prediction);
            }

            if (res.Equals("East"))
            {
                if (testData[5] != -1 && testData[5] != 1) return "East";
                double locMax = 0;
                int locMaxInd = 0;
                for (int i = 0; i < prediction.Length; i++)
                {
                    if (i == 2) continue;
                    if (!(prediction[i] > locMax)) continue;
                    locMax = prediction[i];
                    locMaxInd = i;
                }
                return locMaxInd == 0
                    ? HitOrNot("North", testData, prediction)
                    : HitOrNot(locMaxInd == 1 ? "South" : "West", testData, prediction);
            }

            if (testData[3] != -1 && testData[3] != 1) return "West";
            {
                double locMax = 0;
                int locMaxInd = 0;
                for (int i = 0; i < prediction.Length; i++)
                {
                    if (i == 3) continue;
                    if (!(prediction[i] > locMax)) continue;
                    locMax = prediction[i];
                    locMaxInd = i;
                }

                switch (locMaxInd)
                {
                    case 0:
                        return HitOrNot("North", testData, prediction);
                    case 1:
                        return HitOrNot("South", testData, prediction);
                    default:
                        return HitOrNot("East", testData, prediction);
                }
            }

            return "West";
        }

        public string GetDirectionFromFeedForward(string sensorType, float[] sensorData)
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
            StreamReader reader = new StreamReader(File.OpenRead(@"Assets/" + sensorType + ".csv"));
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
    }
}