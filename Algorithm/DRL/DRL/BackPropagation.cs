using System;
using System.IO;
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
                foreach (var t in weights)
                {
                    file.WriteLine(t);
                }

                file.Close();
                return "New weights have been written into the file MachineLearning/BPWeights/" + sensor_type +
                       "_wts.csv";
                //Training over
            }
            else if (func.Equals("Command"))
            {
                //Get trained weights
                double[] wts = GetTrainedWts(sensor_type, numInput, numHidden, numOutput);
                nn.SetWeights(wts);
                double[] prediction = nn.ComputeOutputs(sensor_data);
                //For loop for debugging
                /*
                for (int i = 0; i < prediction.Length; i++)
                    Console.Write(prediction[i] + "  ");*/
                int maxIndex = nn.MaxIndex(prediction);
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
                //string fin_res = HitOrNot(res, sensor_data, prediction);
            }

            return "";
        } // Driver

        public static double[] GetTrainedWts(String sensorType, int numInput, int numHidden, int numOutput)
        {
            String st = System.IO.File.ReadAllText("./MachineLearning/BPWeights/" + sensorType + "_wts.csv");
            String[] elements = st.Split(new String[] {"\n"}, StringSplitOptions.None);
            int numWeights = (numInput * numHidden) +
                             (numHidden * numOutput) + numHidden + numOutput;
            double[] wts = new double[numWeights];
            for (int i = 0; i < numWeights; i++)
                wts[i] = Convert.ToDouble(elements[i].Trim());
            return wts;
        }

        public static string HitOrNot(string res, double[] testData, double[] prediction)
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

                    if (locMaxInd == 1)
                        return HitOrNot("South", testData, prediction);
                    else if (locMaxInd == 2)
                        return HitOrNot("East", testData, prediction);
                    else
                        return HitOrNot("West", testData, prediction);
                }
                else
                    return "North";
            }
            else if (res.Equals("South"))
            {
                if (testData[7] == -1 || testData[7] == 1)
                {
                    double locMax = 0;
                    int locMaxInd = 0;
                    for (int i = 0; i < prediction.Length; i++)
                    {
                        if (i != 1)
                        {
                            if (prediction[i] > locMax)
                            {
                                locMax = prediction[i];
                                locMaxInd = i;
                            }
                        }
                    }

                    if (locMaxInd == 0)
                        return HitOrNot("North", testData, prediction);
                    else if (locMaxInd == 2)
                        return HitOrNot("East", testData, prediction);
                    else
                        return HitOrNot("West", testData, prediction);
                }
                else
                    return "South";
            }
            else if (res.Equals("East"))
            {
                if (testData[5] == -1 || testData[5] == 1)
                {
                    double locMax = 0;
                    int locMaxInd = 0;
                    for (int i = 0; i < prediction.Length; i++)
                    {
                        if (i != 2)
                        {
                            if (prediction[i] > locMax)
                            {
                                locMax = prediction[i];
                                locMaxInd = i;
                            }
                        }
                    }

                    if (locMaxInd == 0)
                        return HitOrNot("North", testData, prediction);
                    else if (locMaxInd == 1)
                        return HitOrNot("South", testData, prediction);
                    else
                        return HitOrNot("West", testData, prediction);
                }
                else
                    return "East";
            }
            else
            {
                if (testData[3] == -1 || testData[3] == 1)
                {
                    double locMax = 0;
                    int locMaxInd = 0;
                    for (var i = 0; i < prediction.Length; i++)
                    {
                        if (i != 3)
                        {
                            if (prediction[i] > locMax)
                            {
                                locMax = prediction[i];
                                locMaxInd = i;
                            }
                        }
                    }

                    if (locMaxInd == 0)
                        return HitOrNot("North", testData, prediction);
                    else if (locMaxInd == 1)
                        return HitOrNot("South", testData, prediction);
                    else
                        return HitOrNot("East", testData, prediction);
                }
                else
                    return "West";
            }
        }

        public static double[][] ExtractTrainData(string sensorType)
        {
            //Assuming North is [1,0,0,0], South is [0,1,0,0], East is [0,0,1,0], West is [0,0,0,1] 
            String st = System.IO.File.ReadAllText("./Datasets/Backup/" + sensorType + ".csv");
            ;
            int noOfFields;
            if (sensorType.Equals("Proximity"))
            {
                noOfFields = 10;
            }
            else if (sensorType.Equals("Range"))
            {
                noOfFields = 26;
            }
            else //Radar Sensor
            {
                noOfFields = 26;
            }

            String[] elements = st.Split(new String[] {","}, StringSplitOptions.None);
            int count = 0;
            int trainRows = elements.Length / noOfFields;
            double[][] test = new double[trainRows][];
            for (int i = 0; i < trainRows; i++)
            {
                test[i] = new double[noOfFields + 3];
                for (int j = 0; j < noOfFields; j++)
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
        private int _numInput; // number input nodes
        private int _numHidden;
        private int _numOutput;
        private double[] _inputs;
        private double[][] _ihWeights; // input-hidden
        private double[] _hBiases;
        private double[] _hOutputs;
        private double[][] _hoWeights; // hidden-output
        private double[] _oBiases;
        private double[] _outputs;
        private Random rnd;

        public NeuralNetwork(int numInput, int numHidden, int numOutput)
        {
            this._numInput = numInput;
            this._numHidden = numHidden;
            this._numOutput = numOutput;
            _inputs = new double[numInput];
            _ihWeights = MakeMatrix(numInput, numHidden, 0.0);
            _hBiases = new double[numHidden];
            _hOutputs = new double[numHidden];
            _hoWeights = MakeMatrix(numHidden, numOutput, 0.0);
            _oBiases = new double[numOutput];
            _outputs = new double[numOutput];
            rnd = new Random(0);
            InitializeWeights(); // all weights and biases
        } // ctor

        private static double[][] MakeMatrix(int rows,
            int cols, double v) // helper for ctor, Train
        {
            double[][] result = new double[rows][];
            for (int r = 0; r < result.Length; ++r)
                result[r] = new double[cols];
            for (var i = 0; i < rows; ++i)
            for (var j = 0; j < cols; ++j)
                result[i][j] = v;
            return result;
        }

        private void InitializeWeights() // helper for ctor
        {
            // initialize weights and biases to small random values
            int numWeights = (_numInput * _numHidden) +
                             (_numHidden * _numOutput) + _numHidden + _numOutput;
            double[] initialWeights = new double[numWeights];
            for (int i = 0; i < initialWeights.Length; ++i)
                initialWeights[i] = (0.001 - 0.0001) * rnd.NextDouble() + 0.0001;
            this.SetWeights(initialWeights);
        }

        public void SetWeights(double[] weights)
        {
            // copy serialized weights and biases in weights[] array
            // to i-h weights, i-h biases, h-o weights, h-o biases
            int numWeights = (_numInput * _numHidden) +
                             (_numHidden * _numOutput) + _numHidden + _numOutput;
            if (weights.Length != numWeights)
                throw new Exception("Bad weights array in SetWeights");
            int k = 0; // points into weights param
            for (int i = 0; i < _numInput; ++i)
            for (int j = 0; j < _numHidden; ++j)
                _ihWeights[i][j] = weights[k++];
            for (int i = 0; i < _numHidden; ++i)
                _hBiases[i] = weights[k++];
            for (int i = 0; i < _numHidden; ++i)
            for (int j = 0; j < _numOutput; ++j)
                _hoWeights[i][j] = weights[k++];
            for (int i = 0; i < _numOutput; ++i)
                _oBiases[i] = weights[k++];
        }

        public double[] GetWeights()
        {
            int numWeights = (_numInput * _numHidden) +
                             (_numHidden * _numOutput) + _numHidden + _numOutput;
            double[] result = new double[numWeights];
            int k = 0;
            foreach (var t in _ihWeights)
                for (int j = 0; j < _ihWeights[0].Length; ++j)
                    result[k++] = t[j];
            foreach (var t in _hBiases)
                result[k++] = t;
            foreach (var t in _hoWeights)
                for (int j = 0; j < _hoWeights[0].Length; ++j)
                    result[k++] = t[j];
            foreach (var t in _oBiases)
                result[k++] = t;
            return result;
        }

        public double[] ComputeOutputs(double[] xValues)
        {
            double[] hSums = new double[_numHidden]; // hidden nodes sums scratch array
            double[] oSums = new double[_numOutput]; // output nodes sums
            for (int i = 0; i < xValues.Length; ++i) // copy x-values to inputs
                this._inputs[i] = xValues[i];
            // note: no need to copy x-values unless you implement a ToString.
            // more efficient is to simply use the xValues[] directly.
            for (int j = 0; j < _numHidden; ++j) // compute i-h sum of weights * inputs
            for (int i = 0; i < _numInput; ++i)
                hSums[j] += this._inputs[i] * this._ihWeights[i][j]; // note +=
            for (int i = 0; i < _numHidden; ++i) // add biases to hidden sums
                hSums[i] += this._hBiases[i];
            for (int i = 0; i < _numHidden; ++i) // apply activation
                this._hOutputs[i] = HyperTan(hSums[i]); // hard-coded
            for (int j = 0; j < _numOutput; ++j) // compute h-o sum of weights * hOutputs
            for (int i = 0; i < _numHidden; ++i)
                oSums[j] += _hOutputs[i] * _hoWeights[i][j];
            for (int i = 0; i < _numOutput; ++i) // add biases to output sums
                oSums[i] += _oBiases[i];
            double[] softOut = Softmax(oSums); // all outputs at once for efficiency
            Array.Copy(softOut, _outputs, softOut.Length);
            double[] retResult = new double[_numOutput]; // could define a GetOutputs 
            Array.Copy(_outputs, retResult, retResult.Length);
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
            double[][] hoGrads = MakeMatrix(_numHidden, _numOutput, 0.0); // hidden-to-output weight gradients
            double[] obGrads = new double[_numOutput]; // output bias gradients
            double[][] ihGrads = MakeMatrix(_numInput, _numHidden, 0.0); // input-to-hidden weight gradients
            double[] hbGrads = new double[_numHidden]; // hidden bias gradients
            double[]
                oSignals =
                    new double[_numOutput]; // local gradient output signals - gradients w/o associated input terms
            double[] hSignals = new double[_numHidden]; // local gradient hidden node signals
            // back-prop momentum specific arrays 
            double[][] ihPrevWeightsDelta = MakeMatrix(_numInput, _numHidden, 0.0);
            double[] hPrevBiasesDelta = new double[_numHidden];
            double[][] hoPrevWeightsDelta = MakeMatrix(_numHidden, _numOutput, 0.0);
            double[] oPrevBiasesDelta = new double[_numOutput];
            int epoch = 0;
            double[] xValues = new double[_numInput]; // inputs
            double[] tValues = new double[_numOutput]; // target values
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
                    Array.Copy(trainData[idx], xValues, _numInput);
                    Array.Copy(trainData[idx], _numInput, tValues, 0, _numOutput);
                    ComputeOutputs(xValues); // copy xValues in, compute outputs 
                    // indices: i = inputs, j = hiddens, k = outputs
                    // 1. compute output node signals (assumes softmax)
                    for (int k = 0; k < _numOutput; ++k)
                    {
                        errorSignal = tValues[k] - _outputs[k]; // Wikipedia uses (o-t)
                        derivative = (1 - _outputs[k]) * _outputs[k]; // for softmax
                        oSignals[k] = errorSignal * derivative;
                    }

                    // 2. compute hidden-to-output weight gradients using output signals
                    for (int j = 0; j < _numHidden; ++j)
                    for (int k = 0; k < _numOutput; ++k)
                        hoGrads[j][k] = oSignals[k] * _hOutputs[j];
                    // 2b. compute output bias gradients using output signals
                    for (int k = 0; k < _numOutput; ++k)
                        obGrads[k] = oSignals[k] * 1.0; // dummy assoc. input value
                    // 3. compute hidden node signals
                    for (int j = 0; j < _numHidden; ++j)
                    {
                        derivative = (1 + _hOutputs[j]) * (1 - _hOutputs[j]); // for tanh
                        double sum = 0.0; // need sums of output signals times hidden-to-output weights
                        for (int k = 0; k < _numOutput; ++k)
                        {
                            sum += oSignals[k] * _hoWeights[j][k]; // represents error signal
                        }

                        hSignals[j] = derivative * sum;
                    }

                    // 4. compute input-hidden weight gradients
                    for (int i = 0; i < _numInput; ++i)
                    for (int j = 0; j < _numHidden; ++j)
                        ihGrads[i][j] = hSignals[j] * _inputs[i];
                    // 4b. compute hidden node bias gradients
                    for (int j = 0; j < _numHidden; ++j)
                        hbGrads[j] = hSignals[j] * 1.0; // dummy 1.0 input
                    // == update weights and biases
                    // update input-to-hidden weights
                    for (int i = 0; i < _numInput; ++i)
                    {
                        for (int j = 0; j < _numHidden; ++j)
                        {
                            double delta = ihGrads[i][j] * learnRate;
                            _ihWeights[i][j] += delta; // would be -= if (o-t)
                            _ihWeights[i][j] += ihPrevWeightsDelta[i][j] * momentum;
                            ihPrevWeightsDelta[i][j] = delta; // save for next time
                        }
                    }

                    // update hidden biases
                    for (int j = 0; j < _numHidden; ++j)
                    {
                        double delta = hbGrads[j] * learnRate;
                        _hBiases[j] += delta;
                        _hBiases[j] += hPrevBiasesDelta[j] * momentum;
                        hPrevBiasesDelta[j] = delta;
                    }

                    // update hidden-to-output weights
                    for (int j = 0; j < _numHidden; ++j)
                    {
                        for (int k = 0; k < _numOutput; ++k)
                        {
                            double delta = hoGrads[j][k] * learnRate;
                            _hoWeights[j][k] += delta;
                            _hoWeights[j][k] += hoPrevWeightsDelta[j][k] * momentum;
                            hoPrevWeightsDelta[j][k] = delta;
                        }
                    }

                    // update output node biases
                    for (int k = 0; k < _numOutput; ++k)
                    {
                        double delta = obGrads[k] * learnRate;
                        _oBiases[k] += delta;
                        _oBiases[k] += oPrevBiasesDelta[k] * momentum;
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
            double[] xValues = new double[_numInput]; // first numInput values in trainData
            double[] tValues = new double[_numOutput]; // last numOutput values
            // walk thru each training case. looks like (6.9 3.2 5.7 2.3) (0 0 1)
            foreach (var t in trainData)
            {
                Array.Copy(t, xValues, _numInput);
                Array.Copy(t, _numInput, tValues, 0, _numOutput); // get target values
                double[] yValues = this.ComputeOutputs(xValues); // outputs using current weights
                for (int j = 0; j < _numOutput; ++j)
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