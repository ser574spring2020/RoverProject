  using System;
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
	 int numWeights = (numInput * numHidden) +
        (numHidden * numOutput) + numHidden + numOutput;
      double[] initialWeights = new double[numWeights];
      for (int i = 0; i < initialWeights.Length; ++i)
        initialWeights[i] = (0.001 - 0.0001) * rnd.NextDouble() + 0.0001;
      this.SetWeights(initialWeights);
	}
	
	
	public void SetWeights(double[] weights)
    {
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
		 for (int j = 0; j < numHidden; ++j)  // compute i-h sum of weights * inputs
        for (int i = 0; i < numInput; ++i)
          hSums[j] += this.inputs[i] * this.ihWeights[i][j]; // note +=

      for (int i = 0; i < numHidden; ++i)  // add biases to hidden sums
        hSums[i] += this.hBiases[i];

      for (int i = 0; i < numHidden; ++i)   // apply activation
        this.hOutputs[i] = HyperTan(hSums[i]); // hard-coded

      for (int j = 0; j < numOutput; ++j)   // compute h-o sum of weights * hOutputs
        for (int i = 0; i < numHidden; ++i)
          oSums[j] += hOutputs[i] * hoWeights[i][j];

      for (int i = 0; i < numOutput; ++i)  // add biases to output sums
        oSums[i] += oBiases[i];

      double[] softOut = Softmax(oSums); // all outputs at once for efficiency
      Array.Copy(softOut, outputs, softOut.Length);

      double[] retResult = new double[numOutput]; // could define a GetOutputs 
      Array.Copy(this.outputs, retResult, retResult.Length);
      return retResult;
	}

  }