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
	
	}
	
	
	public void SetWeights(double[] weights)
    {
		
	}

	public double[] GetWeights()
    {
	
	}
	
	public double[] ComputeOutputs(double[] xValues)
    {
		
	}

  }