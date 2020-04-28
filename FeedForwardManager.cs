using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class FeedForwardManager
{
    NeuralNetwork net;
    int[] layers = new int[3]{ 9, 12, 4 };
    string[] activation = new string[2] { "sigmoid", "sigmoid" };

    float encode(String direction)
    {
        if (direction == "North")
        {
            return 1;
        } else if (direction=="East")
        {
            return 2;
        } else if (direction == "South")
        {
            return 3;
        } else if (direction == "West")
        {
            return 4;
        }
        return 0;
    }

    string decode(double direction)
    {
        if (direction == 1)
        {
            return "North";
        }
        else if (direction == 2)
        {
            return "East";
        }
        else if (direction ==3 )
        {
            return "South";
        }
        else if (direction == 4)
        {
            return "West";
        }
        return "";
    }

    public int MaxIndex(float[] vector) // helper for Accuracy()
    {
        // index of largest value
        int bigIndex = 0;
        float biggestVal = vector[0];
        for (int i = 0; i < vector.Length; ++i)
        {
            if (vector[i] > biggestVal)
            {
                biggestVal = vector[i];
                bigIndex = i;
            }
        }
        return bigIndex;
    }

    public static string HitOrNot(string res, float[] test_data, float[] yoyo)
    {
        if (res.Equals("North"))
        {
            if (test_data[1] == -1 || test_data[1] == 1)
            {
                double loc_max = 0;
                int loc_max_ind = 0;
                for (int i = 0; i < yoyo.Length; i++)
                {
                    if (i != 0)
                    {
                        if (yoyo[i] > loc_max)
                        {
                            loc_max = yoyo[i];
                            loc_max_ind = i;
                        }
                    }
                }
                if (loc_max_ind == 1)
                    return HitOrNot("South", test_data, yoyo);
                else if (loc_max_ind == 2)
                    return HitOrNot("East", test_data, yoyo);
                else
                    return HitOrNot("West", test_data, yoyo);
            }
            else
                return "North";
        }
        else if (res.Equals("South"))
        {
            if (test_data[7] == -1 || test_data[7] == 1)
            {
                double loc_max = 0;
                int loc_max_ind = 0;
                for (int i = 0; i < yoyo.Length; i++)
                {
                    if (i != 1)
                    {
                        if (yoyo[i] > loc_max)
                        {
                            loc_max = yoyo[i];
                            loc_max_ind = i;
                        }
                    }
                }
                if (loc_max_ind == 0)
                    return HitOrNot("North", test_data, yoyo);
                else if (loc_max_ind == 2)
                    return HitOrNot("East", test_data, yoyo);
                else
                    return HitOrNot("West", test_data, yoyo);
            }
            else
                return "South";
        }
        else if (res.Equals("East"))
        {
            if (test_data[5] == -1 || test_data[5] == 1)
            {
                double loc_max = 0;
                int loc_max_ind = 0;
                for (int i = 0; i < yoyo.Length; i++)
                {
                    if (i != 2)
                    {
                        if (yoyo[i] > loc_max)
                        {
                            loc_max = yoyo[i];
                            loc_max_ind = i;
                        }
                    }
                }
                if (loc_max_ind == 0)
                    return HitOrNot("North", test_data, yoyo);
                else if (loc_max_ind == 1)
                    return HitOrNot("South", test_data, yoyo);
                else
                    return HitOrNot("West", test_data, yoyo);
            }
            else
                return "East";
        }
        else
        {
            if (test_data[3] == -1 || test_data[3] == 1)
            {
                double loc_max = 0;
                int loc_max_ind = 0;
                for (int i = 0; i < yoyo.Length; i++)
                {
                    if (i != 3)
                    {
                        if (yoyo[i] > loc_max)
                        {
                            loc_max = yoyo[i];
                            loc_max_ind = i;
                        }
                    }
                }
                if (loc_max_ind == 0)
                    return HitOrNot("North", test_data, yoyo);
                else if (loc_max_ind == 1)
                    return HitOrNot("South", test_data, yoyo);
                else
                    return HitOrNot("East", test_data, yoyo);
            }
            else
                return "West";
        }
    }

    public string GetDirectionFromFeedForward(string sensor_type, float[] sensor_data)
    {
        int[] layers=new int[3] { 9, 12, 4 }; ;
        if (sensor_type == "Proximity")
        {
             layers = new int[3] { 9, 12, 4 };
        } else if (sensor_type =="Radar" || sensor_type =="Range")
        {
             layers = new int[3] { 25, 12, 4 };
        }
        this.net = new NeuralNetwork(layers, activation);

        StreamReader reader = new StreamReader(File.OpenRead(@"Assets/"+ sensor_type + ".csv"));
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');
            float[] direction = new float[] { 0, 0, 1, 0 };
            if (values[9] == "North")
            {
                direction = new float[] { 1, 0, 0, 0 };
            }
            else if (values[9] == "East")
            {
                direction = new float[] { 0, 1, 0, 0 };
            }
            else if (values[9] == "South")
            {
                direction = new float[] { 0, 0, 1, 0 };
            }
            else if (values[9] == "West")
            {
                direction = new float[] { 0, 0, 0, 1 };
            }
            float[] vs = new float[9];
            for (int i = 0; i < 9; i++)
            {
                vs[i] = float.Parse(values[i]);
            }
            net.Train(vs, direction);
        }

        float[] op = net.FeedForward(sensor_data);

        int maxIndex = MaxIndex(op);
        String res;
        if (maxIndex == 0)
        {
            res = "North";
        }
        else if (maxIndex == 1)
        {
            res = "South";
        }
        else if (maxIndex == 2)
        {
            res = "East";
        }
        else
        {
            res = "West";
        }
        Debug.Log(res);

        return HitOrNot(res, sensor_data, op);

    }


    //void Start()
    //{
    //    this.net = new NeuralNetwork(layers, activation);

    //    StreamReader reader = new StreamReader(File.OpenRead(@"Assets/Proximity.csv"));
        //for(int j=0;j<21000;j++)
        //while (!reader.EndOfStream)
        //    {
        //        string line = reader.ReadLine();
        //        string[] values = line.Split(',');
        //    float[] direction = new float[] { 0, 0, 1, 0 };
        //    if (values[9] == "North")
        //    {
        //         direction = new float[] { 1, 0, 0, 0 };
        //    }
        //    else if (values[9] == "East")
        //    {
        //         direction = new float[] { 0, 1, 0, 0 };
        //    }
        //    else if (values[9] == "South")
        //    {
        //        direction = new float[] { 0, 0, 1, 0 };
        //    }
        //    else if (values[9] == "West")
        //    {
        //         direction = new float[] { 0, 0, 0, 1 };
        //    }
        //    //float direction = encode(values[9]);
        //        float[] vs = new float[9];
        //        for (int i = 0; i < 9; i++)
        //        {
        //            vs[i] = float.Parse(values[i]);
        //        }
        //        net.Train(vs, direction );
        //    }

        //net.Load("Assets/Save.txt");
        //Debug.Log(net.FeedForward(new float[] { -1, 1, -1, 1, 2, 0, -1, 0, -1 })[0]);
        //Debug.Log(net.FeedForward(new float[] { -1, 1, -1, 1, 2, 0, -1, 0, -1 })[1]);
        //Debug.Log(net.FeedForward(new float[] { -1, 1, -1, 1, 2, 0, -1, 0, -1 })[2]);
        //Debug.Log(net.FeedForward(new float[] { -1, 1, -1, 1, 2, 0, -1, 0, -1 })[3]);

       // float[] op= net.FeedForward(new float[] { -1, 1, -1, 1, 2, 0, -1, 0, -1 });


       // int maxIndex = MaxIndex(op);
       // String res;
       // if (maxIndex == 0)
       // {
       //     res = "North";
       // }
       // else if (maxIndex == 1)
       // {
       //     res = "South";
       // }
       // else if (maxIndex == 2)
       // {
       //     res = "East";
       // }
       // else
       // {
       //     res = "West";
       // }
       // Debug.Log(res);

       //Debug.Log( HitOrNot(res, new double[] { -1, 1, -1, 1, 2, 0, -1, 0, -1 }, op));

        //float op = net.FeedForward(new float[] { -1, 0, -1, 4, 2, 0, -1, 0, -1 })[0]; //North
        //Debug.Log(op);
        //op = net.FeedForward(new float[] { -1, 1, -1, 1, 2, 0, -1, 0, -1 })[0]; //East
        //Debug.Log(op);
        //op = net.FeedForward(new float[] { -1, 1, -1, 4, 2, 1, -1, 0, -1 })[0]; //South
        //Debug.Log(op);


        //op = net.FeedForward(new float[] { -1, 1, -1, 0, 2, 4, -1, 1, -1 })[0]; //West
        //Debug.Log(op);
        //op = net.FeedForward(new float[] { -1, 1, -1, 4, 2, 1, -1, 0, -1 })[0];
        //Debug.Log(op);

        //op = Math.Round(op);
        //Debug.Log(op);
        //String dir = decode(op);
        //Debug.Log(dir);
        //net.Save("Assets/Save.txt");

        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 1, 0, 0 })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 0, 1, 0 })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 0, 0, 1 })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 1, 1, 0 })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 0, 1, 1 })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 1, 0, 1 })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 1, 1, 1 })[0]);
        //We want the gate to simulate 3 input or gate (A or B or C)
        // 0 0 0    => 0
        // 1 0 0    => 1
        // 0 1 0    => 1
        // 0 0 1    => 1
        // 1 1 0    => 1
        // 0 1 1    => 1
        // 1 0 1    => 1
        // 1 1 1    => 1
    //}
}
