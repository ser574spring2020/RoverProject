using System;
using System.IO;
using UnityEngine;
using Random = System.Random;

namespace DRL
{
    public class BackPropagation
    {
        public static String Driver(string func, string sensor_type, double[] sensor_data)
        {
            int numInput; //changes with sensor type
            int numHidden = 6; //Constant value
            int numOutput = 4; //constant value
            if (sensor_type.Equals("Proximity"))
            {
                numInput = 9;
            }
            else if (sensor_type.Equals("Range"))
            {
                numInput = 25;
            }
            else
            {
                //Radar Sensor
                numInput = 25;
            }

            NeuralNetwork nn = new NeuralNetwork(numInput, numHidden, numOutput);


            //Check whether to Train or Find direction
            if (func.Equals("Train"))
            {
                //Gets training data from csv file and stores in roverTrainData
                double[][] roverTrainData = ExtractTrainData(sensor_type);

                int maxEpochs = 1000;
                double learnRate = 0.05;
                double momentum = 0.01;

                //Train Neural Network and Store trained weights
                Console.WriteLine("\nStarting training");
                double[] weights = nn.Train(roverTrainData, maxEpochs, learnRate, momentum);

                //Write trained weights in a file
                StreamWriter file = new StreamWriter("./MachineLearning/BPWeights/" + sensor_type + "_wts.csv");
                for (int i = 0; i < weights.Length; i++)
                {
                    file.WriteLine(weights[i]);
                }

                file.Close();
                return "New weights have been written into the file MachineLearning/BPWeights/" + sensor_type + "_wts.csv";
                //Training over
            }
            else if (func.Equals("Command"))
            {
                //Get trained weights
                double[] wts = GetTrainedWts(sensor_type, numInput, numHidden, numOutput);
                nn.SetWeights(wts);
                double[] yoyo = nn.ComputeOutputs(sensor_data);

                //For loop for debugging
                /*
                for (int i = 0; i < yoyo.Length; i++)
                    Console.Write(yoyo[i] + "  ");*/

                int maxIndex = nn.MaxIndex(yoyo);
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

                return res;
                //Hit or Not under progress
                //string fin_res = HitOrNot(res, sensor_data, yoyo);
            }

            return "";
        } // Driver


        public static double[] GetTrainedWts(String sensor_type, int numInput, int numHidden, int numOutput)
        {
            String st = System.IO.File.ReadAllText("./MachineLearning/BPWeights/" + sensor_type + "_wts.csv");

            String[] elements = st.Split(new String[] {"\n"}, StringSplitOptions.None);
            int numWeights = (numInput * numHidden) +
                             (numHidden * numOutput) + numHidden + numOutput;

            double[] wts = new double[numWeights];
            for (int i = 0; i < numWeights; i++)
                wts[i] = Convert.ToDouble(elements[i].Trim());

            return wts;
        }


        public static string HitOrNot(string res, double[] test_data, double[] yoyo)
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


        public static double[][] ExtractTrainData(string sensor_type)
        {
            //Assuming North is [1,0,0,0], South is [0,1,0,0], East is [0,0,1,0], West is [0,0,0,1] 
            String st = System.IO.File.ReadAllText("/Backup/" + sensor_type + ".csv");
            ;
            int no_of_fields;
            if (sensor_type.Equals("Proximity"))
            {
                no_of_fields = 10;
            }
            else if (sensor_type.Equals("Range"))
            {
                no_of_fields = 26;
            }
            else //Radar Sensor
            {
                no_of_fields = 26;
            }

            String[] elements = st.Split(new String[] {","}, StringSplitOptions.None);
            int count = 0;
            int train_rows = elements.Length / no_of_fields;
            double[][] test = new Double[train_rows][];
            for (int i = 0; i < train_rows; i++)
            {
                test[i] = new double[no_of_fields + 3];
                for (int j = 0; j < no_of_fields; j++)
                {
                    if ((elements[count].Trim()).Equals("North"))
                    {
                        test[i][j] = 1;
                        test[i][j + 1] = 0;
                        test[i][j + 2] = 0;
                        test[i][j + 3] = 0;
                    }
                    else if ((elements[count].Trim()).Equals("South"))
                    {
                        test[i][j] = 0;
                        test[i][j + 1] = 1;
                        test[i][j + 2] = 0;
                        test[i][j + 3] = 0;
                    }
                    else if ((elements[count].Trim()).Equals("East"))
                    {
                        test[i][j] = 0;
                        test[i][j + 1] = 0;
                        test[i][j + 2] = 1;
                        test[i][j + 3] = 0;
                    }
                    else if ((elements[count].Trim()).Equals("West"))
                    {
                        test[i][j] = 0;
                        test[i][j + 1] = 0;
                        test[i][j + 2] = 0;
                        test[i][j + 3] = 1;
                    }
                    else
                    {
                        test[i][j] = Convert.ToDouble(elements[count].Trim());
                    }

                    count = count + 1;
                }
            }

            return test;
        }
    }

    public class NeuralNetwork
    {
        private int numInput; // number input nodes
        private int numHidden;
        private int numOutput;

        private double[] inputs;
        private double[][] ihWeights; // input-hidden
        private double[] hBiases;
        private double[] hOutputs;

        private double[][] hoWeights; // hidden-output
        private double[] oBiases;
        private double[] outputs;

        private Random rnd;

        public NeuralNetwork(int numInput, int numHidden, int numOutput)
        {
            this.numInput = numInput;
            this.numHidden = numHidden;
            this.numOutput = numOutput;

            this.inputs = new double[numInput];

            this.ihWeights = MakeMatrix(numInput, numHidden, 0.0);
            this.hBiases = new double[numHidden];
            this.hOutputs = new double[numHidden];

            this.hoWeights = MakeMatrix(numHidden, numOutput, 0.0);
            this.oBiases = new double[numOutput];
            this.outputs = new double[numOutput];

            this.rnd = new Random(0);
            this.InitializeWeights(); // all weights and biases
        } // ctor

        private static double[][] MakeMatrix(int rows,
            int cols, double v) // helper for ctor, Train
        {
            double[][] result = new double[rows][];
            for (int r = 0; r < result.Length; ++r)
                result[r] = new double[cols];
            for (int i = 0; i < rows; ++i)
            for (int j = 0; j < cols; ++j)
                result[i][j] = v;
            return result;
        }

        private void InitializeWeights() // helper for ctor
        {
            // initialize weights and biases to small random values
            int numWeights = (numInput * numHidden) +
                             (numHidden * numOutput) + numHidden + numOutput;
            double[] initialWeights = new double[numWeights];
            for (int i = 0; i < initialWeights.Length; ++i)
                initialWeights[i] = (0.001 - 0.0001) * rnd.NextDouble() + 0.0001;
            this.SetWeights(initialWeights);
        }

        public void SetWeights(double[] weights)
        {
            // copy serialized weights and biases in weights[] array
            // to i-h weights, i-h biases, h-o weights, h-o biases
            int numWeights = (numInput * numHidden) +
                             (numHidden * numOutput) + numHidden + numOutput;
            if (weights.Length != numWeights)
                throw new Exception("Bad weights array in SetWeights");

            int k = 0; // points into weights param

            for (int i = 0; i < numInput; ++i)
            for (int j = 0; j < numHidden; ++j)
                ihWeights[i][j] = weights[k++];
            for (int i = 0; i < numHidden; ++i)
                hBiases[i] = weights[k++];
            for (int i = 0; i < numHidden; ++i)
            for (int j = 0; j < numOutput; ++j)
                hoWeights[i][j] = weights[k++];
            for (int i = 0; i < numOutput; ++i)
                oBiases[i] = weights[k++];
        }

        public double[] GetWeights()
        {
            int numWeights = (numInput * numHidden) +
                             (numHidden * numOutput) + numHidden + numOutput;
            double[] result = new double[numWeights];
            int k = 0;
            for (int i = 0; i < ihWeights.Length; ++i)
            for (int j = 0; j < ihWeights[0].Length; ++j)
                result[k++] = ihWeights[i][j];
            for (int i = 0; i < hBiases.Length; ++i)
                result[k++] = hBiases[i];
            for (int i = 0; i < hoWeights.Length; ++i)
            for (int j = 0; j < hoWeights[0].Length; ++j)
                result[k++] = hoWeights[i][j];
            for (int i = 0; i < oBiases.Length; ++i)
                result[k++] = oBiases[i];
            return result;
        }

        public double[] ComputeOutputs(double[] xValues)
        {
            double[] hSums = new double[numHidden]; // hidden nodes sums scratch array
            double[] oSums = new double[numOutput]; // output nodes sums

            for (int i = 0; i < xValues.Length; ++i) // copy x-values to inputs
                this.inputs[i] = xValues[i];
            // note: no need to copy x-values unless you implement a ToString.
            // more efficient is to simply use the xValues[] directly.

            for (int j = 0; j < numHidden; ++j) // compute i-h sum of weights * inputs
            for (int i = 0; i < numInput; ++i)
                hSums[j] += this.inputs[i] * this.ihWeights[i][j]; // note +=

            for (int i = 0; i < numHidden; ++i) // add biases to hidden sums
                hSums[i] += this.hBiases[i];

            for (int i = 0; i < numHidden; ++i) // apply activation
                this.hOutputs[i] = HyperTan(hSums[i]); // hard-coded

            for (int j = 0; j < numOutput; ++j) // compute h-o sum of weights * hOutputs
            for (int i = 0; i < numHidden; ++i)
                oSums[j] += hOutputs[i] * hoWeights[i][j];

            for (int i = 0; i < numOutput; ++i) // add biases to output sums
                oSums[i] += oBiases[i];

            double[] softOut = Softmax(oSums); // all outputs at once for efficiency
            Array.Copy(softOut, outputs, softOut.Length);

            double[] retResult = new double[numOutput]; // could define a GetOutputs 
            Array.Copy(this.outputs, retResult, retResult.Length);
            return retResult;
        }

        private static double HyperTan(double x)
        {
            if (x < -20.0) return -1.0; // approximation is correct to 30 decimals
            else if (x > 20.0) return 1.0;
            else return Math.Tanh(x);
        }

        private static double[] Softmax(double[] oSums)
        {
            // does all output nodes at once so scale
            // doesn't have to be re-computed each time

            double sum = 0.0;
            for (int i = 0; i < oSums.Length; ++i)
                sum += Math.Exp(oSums[i]);

            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; ++i)
                result[i] = Math.Exp(oSums[i]) / sum;

            return result; // now scaled so that xi sum to 1.0
        }

        public double[] Train(double[][] trainData, int maxEpochs,
            double learnRate, double momentum)
        {
            // train using back-prop
            // back-prop specific arrays
            double[][] hoGrads = MakeMatrix(numHidden, numOutput, 0.0); // hidden-to-output weight gradients
            double[] obGrads = new double[numOutput]; // output bias gradients

            double[][] ihGrads = MakeMatrix(numInput, numHidden, 0.0); // input-to-hidden weight gradients
            double[] hbGrads = new double[numHidden]; // hidden bias gradients

            double[]
                oSignals =
                    new double[numOutput]; // local gradient output signals - gradients w/o associated input terms
            double[] hSignals = new double[numHidden]; // local gradient hidden node signals

            // back-prop momentum specific arrays 
            double[][] ihPrevWeightsDelta = MakeMatrix(numInput, numHidden, 0.0);
            double[] hPrevBiasesDelta = new double[numHidden];
            double[][] hoPrevWeightsDelta = MakeMatrix(numHidden, numOutput, 0.0);
            double[] oPrevBiasesDelta = new double[numOutput];

            int epoch = 0;
            double[] xValues = new double[numInput]; // inputs
            double[] tValues = new double[numOutput]; // target values
            double derivative = 0.0;
            double errorSignal = 0.0;

            int[] sequence = new int[trainData.Length];
            for (int i = 0; i < sequence.Length; ++i)
                sequence[i] = i;

            int errInterval = maxEpochs / 10; // interval to check error
            while (epoch < maxEpochs)
            {
                ++epoch;

                if (epoch % errInterval == 0 && epoch < maxEpochs)
                {
                    double trainErr = Error(trainData);
                    Console.WriteLine("epoch = " + epoch + "  error = " +
                                      trainErr.ToString("F4"));
                    //Console.ReadLine();
                }

                Shuffle(sequence); // visit each training data in random order
                for (int ii = 0; ii < trainData.Length; ++ii)
                {
                    int idx = sequence[ii];
                    Array.Copy(trainData[idx], xValues, numInput);
                    Array.Copy(trainData[idx], numInput, tValues, 0, numOutput);
                    ComputeOutputs(xValues); // copy xValues in, compute outputs 

                    // indices: i = inputs, j = hiddens, k = outputs

                    // 1. compute output node signals (assumes softmax)
                    for (int k = 0; k < numOutput; ++k)
                    {
                        errorSignal = tValues[k] - outputs[k]; // Wikipedia uses (o-t)
                        derivative = (1 - outputs[k]) * outputs[k]; // for softmax
                        oSignals[k] = errorSignal * derivative;
                    }

                    // 2. compute hidden-to-output weight gradients using output signals
                    for (int j = 0; j < numHidden; ++j)
                    for (int k = 0; k < numOutput; ++k)
                        hoGrads[j][k] = oSignals[k] * hOutputs[j];

                    // 2b. compute output bias gradients using output signals
                    for (int k = 0; k < numOutput; ++k)
                        obGrads[k] = oSignals[k] * 1.0; // dummy assoc. input value

                    // 3. compute hidden node signals
                    for (int j = 0; j < numHidden; ++j)
                    {
                        derivative = (1 + hOutputs[j]) * (1 - hOutputs[j]); // for tanh
                        double sum = 0.0; // need sums of output signals times hidden-to-output weights
                        for (int k = 0; k < numOutput; ++k)
                        {
                            sum += oSignals[k] * hoWeights[j][k]; // represents error signal
                        }

                        hSignals[j] = derivative * sum;
                    }

                    // 4. compute input-hidden weight gradients
                    for (int i = 0; i < numInput; ++i)
                    for (int j = 0; j < numHidden; ++j)
                        ihGrads[i][j] = hSignals[j] * inputs[i];

                    // 4b. compute hidden node bias gradients
                    for (int j = 0; j < numHidden; ++j)
                        hbGrads[j] = hSignals[j] * 1.0; // dummy 1.0 input

                    // == update weights and biases

                    // update input-to-hidden weights
                    for (int i = 0; i < numInput; ++i)
                    {
                        for (int j = 0; j < numHidden; ++j)
                        {
                            double delta = ihGrads[i][j] * learnRate;
                            ihWeights[i][j] += delta; // would be -= if (o-t)
                            ihWeights[i][j] += ihPrevWeightsDelta[i][j] * momentum;
                            ihPrevWeightsDelta[i][j] = delta; // save for next time
                        }
                    }

                    // update hidden biases
                    for (int j = 0; j < numHidden; ++j)
                    {
                        double delta = hbGrads[j] * learnRate;
                        hBiases[j] += delta;
                        hBiases[j] += hPrevBiasesDelta[j] * momentum;
                        hPrevBiasesDelta[j] = delta;
                    }

                    // update hidden-to-output weights
                    for (int j = 0; j < numHidden; ++j)
                    {
                        for (int k = 0; k < numOutput; ++k)
                        {
                            double delta = hoGrads[j][k] * learnRate;
                            hoWeights[j][k] += delta;
                            hoWeights[j][k] += hoPrevWeightsDelta[j][k] * momentum;
                            hoPrevWeightsDelta[j][k] = delta;
                        }
                    }

                    // update output node biases
                    for (int k = 0; k < numOutput; ++k)
                    {
                        double delta = obGrads[k] * learnRate;
                        oBiases[k] += delta;
                        oBiases[k] += oPrevBiasesDelta[k] * momentum;
                        oPrevBiasesDelta[k] = delta;
                    }
                } // each training item
            } // while

            double[] bestWts = GetWeights();
            return bestWts;
        } // Train

        private void Shuffle(int[] sequence) // instance method
        {
            for (int i = 0; i < sequence.Length; ++i)
            {
                int r = this.rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
            }
        } // Shuffle

        private double Error(double[][] trainData)
        {
            // average squared error per training item
            double sumSquaredError = 0.0;
            double[] xValues = new double[numInput]; // first numInput values in trainData
            double[] tValues = new double[numOutput]; // last numOutput values

            // walk thru each training case. looks like (6.9 3.2 5.7 2.3) (0 0 1)
            for (int i = 0; i < trainData.Length; ++i)
            {
                Array.Copy(trainData[i], xValues, numInput);
                Array.Copy(trainData[i], numInput, tValues, 0, numOutput); // get target values
                double[] yValues = this.ComputeOutputs(xValues); // outputs using current weights
                for (int j = 0; j < numOutput; ++j)
                {
                    double err = tValues[j] - yValues[j];
                    sumSquaredError += err * err;
                }
            }

            return sumSquaredError / trainData.Length;
        } // MeanSquaredError

        public int MaxIndex(double[] vector) // helper for Accuracy()
        {
            // index of largest value
            int bigIndex = 0;
            double biggestVal = vector[0];
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
    }
}