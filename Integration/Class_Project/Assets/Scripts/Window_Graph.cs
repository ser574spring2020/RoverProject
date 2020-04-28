/* 
    ------------------- Code Monkey -------------------
    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!
               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;
using ExperimentalDesignDatabase;


public class Window_Graph : MonoBehaviour
{

    [SerializeField] private Sprite dotSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    public TMPro.TMP_Dropdown yAxis;
    //public TMPro.TMP_Dropdown yAxis_1;
    //private RectTransform dashTemplateX;
    //private RectTransform dashTemplateY;
    private List<GameObject> gameObjectList;
    //private Button_UI BarChart;
    //private Button_UI LineChart;
    private Boolean GraphFlag;
    public Dropdown InputAlgorithm;
    public Dropdown MazeSize;
    public Dropdown SensorType;
    public InputField Thresholdvalue;
    public InputField Maxvalue;
    public InputField MinValue;
    public InputField AverageValue;
    public TMPro.TMP_Text StatusText;
    public InputField EXPID;
    public InputField Algovalue;
    public InputField Mazevalue;
    public InputField frequencyval;
    public InputField sensorvalues;
    public TMPro.TMP_Text pathcoverage;
    public InputField trailvalue;
    public Boolean isexpid;

    private void Awake()
    {
        //BarChart = transform.Find("barChartBtn").GetComponent<Button_UI>();
        //LineChart = transform.Find("lineGraphBtn").GetComponent<Button_UI>();

        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        //dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        //dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();

        List<int> valueList = new List<int>(15);

        System.Random randNum = new System.Random();
        yAxis = GameObject.Find("yAxis").GetComponent<TMPro.TMP_Dropdown>();
        string yAxisValue = yAxis.captionText.text;
        //print(yAxisValue);

        // yAxis_1 = GameObject.Find("yAxis_1").GetComponent<TMPro.TMP_Dropdown>();
        // string yAxisValue_1 = yAxis.captionText.text;
        // print(yAxisValue_1);
        //GraphFlag = true;








    }

    public void onBarGraphButtonClick()

    {

        GraphFlag = true;
        if (!isexpid)
        {
            onButtonClicked();
        }
        else
        {
            onButtonClicked2();
        }


    }

    public void onLineGraphButtonClick()
    {
        GraphFlag = false;
        if (!isexpid)
        {
            onButtonClicked();
        }
        else
        {
            onButtonClicked2();
        }


    }

    public void changeUIScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
    public void onButtonClicked2()
    {
        isexpid = true;
        string yAxisValue = yAxis.captionText.text;
        int id = Int32.Parse(EXPID.text);
        List<int> valueList = new List<int>(400);
        database db = new database();
        float average, minimum, maximum;
        List<float> range;
        List<string> algorithmval;
        List<string> mazesizeval;
        List<float> thresholdval;
        List<string> sensorval;
        List<string> pathcovered;
        List<int> scrambledWatermark = new List<int>(400);
        try { 
        average = db.averagevalue(yAxisValue, id);
        minimum = db.minvalue(yAxisValue, id);
        maximum = db.maxvalue(yAxisValue, id);
        Maxvalue.text = maximum.ToString();
        MinValue.text = minimum.ToString();
        AverageValue.text = average.ToString();
        algorithmval = db.selectValuesofAlgorithm(id);
        Algovalue.text = algorithmval[0];
        mazesizeval = db.selectvaluesofmazesize(id);
        Mazevalue.text = mazesizeval[0];
        thresholdval = db.selectvaluesofthreshold(id);
        frequencyval.text = thresholdval[0].ToString();
        sensorval = db.selectvaluesofsensor(id);
        sensorvalues.text = sensorval[0];
        range = db.selectValuesfromDB(yAxisValue, id);
        db.UpdateMaze(new int[,] { { 1, 0 }, { 0, 1 }, { 1, 1 }, { 1, 0 } });
        pathcovered = db.selectpathcovered(id);
        if (trailvalue.text != "")
        {
            string coveredpath = pathcovered[Convert.ToInt32(trailvalue.text) - 1];
            pathcoverage.text = (coveredpath).Replace(";", "\n");
        }
        else
        {
            pathcoverage.text = "";
        }
            StatusText.text = "";
            scrambledWatermark = range.ConvertAll(Convert.ToInt32);
    }
        catch (Exception e)
        {
            average = 0.0F;
            minimum = 0.0F;
            maximum = 0.0F;
            StatusText.text = "Sorry No Results stored in Database. Try again with different filters.";
            return;
        }




        if (GraphFlag)
        {

            switch (yAxisValue)
            {
                case "TimeTaken":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " Minutes");
                    break;
                case "PointsScored":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " Points");
                    break;
                case "MazeCoverage":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " % ");
                    break;
                case "DroneLife":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + "% Drone Life");
                    break;
                default:
                    Debug.LogError("Improper command given!");
                    break;
            }

        }
        else
        {

            switch (yAxisValue)
            {
                case "TimeTaken":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowLineGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " Minutes");
                    break;
                case "PointsScored":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowLineGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " Points");
                    break;
                case "MazeCoverage":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowLineGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + "% ");
                    break;
                case "DroneLife":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowLineGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " % Drone Life");
                    break;
                default:
                    Debug.LogError("Improper command given!");
                    break;
            }

        }


    }

    public void onButtonClicked()
    {
        isexpid = false;
        string yAxisValue = yAxis.captionText.text;
        List<int> valueList = new List<int>(400);
        System.Random randNum = new System.Random();

        string InputAlgorithmValue = InputAlgorithm.captionText.text;
        string MazeSizeValue = MazeSize.captionText.text;
        float Threshold = 0;
        try
        {
            Threshold = float.Parse(Thresholdvalue.text);
        }catch(Exception e)
        {
            StatusText.text = "Please enter a valid value for Threshold Frequency.";
            return;
        }
        //print(Threshold);
        string SensorTypeValue = SensorType.captionText.text;
        database db = new database();

        // print(db.averagevalue(yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue));
        float average, minimum, maximum;
        List<float> range;
        List<int> scrambledWatermark = new List<int>(400) ;
        try
        {
            average = db.averagevalue(yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
            minimum = db.minvalue(yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
            maximum = db.maxvalue(yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
            Maxvalue.text = maximum.ToString();
            MinValue.text = minimum.ToString();
            AverageValue.text = average.ToString();
            range = db.selectValuesfromDB(yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
            scrambledWatermark = range.ConvertAll(Convert.ToInt32);
        }
        catch(Exception e)
        {
            average = 0.0F;
            minimum = 0.0F;
            maximum = 0.0F;
            StatusText.text = "Sorry No Results stored in Database. Try again with different filters.";
            return;
        }
        
        


        if (GraphFlag)
        {

            switch (yAxisValue)
            {
                case "TimeTaken":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " Minutes");
                    break;
                case "PointsScored":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " Points");
                    break;
                case "MazeCoverage":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " % ");
                    break;
                case "DroneLife":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + "% Drone Life");
                    break;
                default:
                    Debug.LogError("Improper command given!");
                    break;
            }

        }
        else
        {

            switch (yAxisValue)
            {
                case "TimeTaken":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowLineGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " Minutes");
                    break;
                case "PointsScored":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowLineGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " Points");
                    break;
                case "MazeCoverage":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowLineGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + "% ");
                    break;
                case "DroneLife":
                    foreach (int item in scrambledWatermark)
                    {
                        valueList.Add(item);
                    }
                    ShowLineGraph(valueList, -1, (int _i) => " Trail " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + " % Drone Life");
                    break;
                default:
                    Debug.LogError("Improper command given!");
                    break;
            }

        }


    }
    /*
    public void onDropDownChange2()
    {
        string yAxisValue = yAxis.captionText.text;
        List<int> valueList = new List<int>(15);
        System.Random randNum = new System.Random();




        switch (yAxisValue)
        {
            case "Time":
                for (int i = 0; i < 15; i++)
                {
                    valueList.Add(randNum.Next(1, 120));
                }
                ShowLineGraph(valueList, -1, (int _i) => "", (float _f) => Mathf.RoundToInt(_f) + " Minutes");
                break;
            case "Points":
                for (int i = 0; i < 15; i++)
                {
                    valueList.Add(randNum.Next(1000, 2000));
                }
                ShowLineGraph(valueList, -1, (int _i) => "", (float _f) => Mathf.RoundToInt(_f) + " Points");
                break;
            case "Objects Collected":
                for (int i = 0; i < 15; i++)
                {
                    valueList.Add(randNum.Next(0, 50));
                }
                ShowLineGraph(valueList, -1, (int _i) => "", (float _f) => Mathf.RoundToInt(_f) + " Objects");
                break;
            case "Maze Coverage":
                for (int i = 0; i < 15; i++)
                {
                    valueList.Add(randNum.Next(0, 100));
                }
                ShowLineGraph(valueList, -1, (int _i) => "", (float _f) => Mathf.RoundToInt(_f) + "% ");
                break;
            case "Drone Life":
                for (int i = 0; i < 15; i++)
                {
                    valueList.Add(randNum.Next(0, 100));
                }
                ShowLineGraph(valueList, -1, (int _i) => "", (float _f) => Mathf.RoundToInt(_f) + "% Drone Life");
                break;
            default:
                Debug.LogError("Improper command given!");
                break;
        }


        
        if (yAxisValue.Equals("Time")) {
            for (int i = 0; i < 15; i++)
            {
                valueList.Add(randNum.Next(1, 120));
            }
            ShowGraph(valueList, -1, (int _i) => "", (float _f) => Mathf.RoundToInt(_f) + " Minutes");
        } else if (yAxisValue.Equals("Points"))
        {
            for (int i = 0; i < 15; i++)
            {
                valueList.Add(randNum.Next(1000, 2000));
            }
            ShowGraph(valueList, -1, (int _i) => "", (float _f) => Mathf.RoundToInt(_f) + " Points");
        } 


    }*/
    private void ShowGraph(List<int> valueList, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        if (maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valueList.Count;
        }

        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMaximum = valueList[0];
        float yMinimum = valueList[0];

        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            int value = valueList[i];
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0)
        {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        yMinimum = 0f; // Start the graph at zero

        float xSize = graphWidth / (maxVisibleValueAmount + 1);

        int xIndex = 0;

        //GameObject lastDotGameObject = null;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject barGameObject = CreateBar(new Vector2(xPosition, yPosition), xSize * .9f);
            gameObjectList.Add(barGameObject);

            //GameObject dotGameObject = CreateDot(new Vector2(xPosition, yPosition));
            //gameObjectList.Add(dotGameObject);
            //if (lastDotGameObject != null) {
            //    GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
            //    gameObjectList.Add(dotConnectionGameObject);
            //}
            //lastDotGameObject = dotGameObject;


            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            //RectTransform dashX = Instantiate(dashTemplateX);
            //dashX.SetParent(graphContainer, false);
            //dashX.gameObject.SetActive(true);
            //dashX.anchoredPosition = new Vector2(xPosition, -3f);
            //gameObjectList.Add(dashX.gameObject);

            xIndex++;
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            gameObjectList.Add(labelY.gameObject);

            //RectTransform dashY = Instantiate(dashTemplateY);
            //dashY.SetParent(graphContainer, false);
            //dashY.gameObject.SetActive(true);
            //float normalizedValue = i * 1f / separatorCount;
            //dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            //gameObjectList.Add(dashY.gameObject);
        }
    }
    private void ShowLineGraph(List<int> valueList, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        if (maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valueList.Count;
        }

        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMaximum = valueList[0];
        float yMinimum = valueList[0];

        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            int value = valueList[i];
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0)
        {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        yMinimum = 0f; // Start the graph at zero

        float xSize = graphWidth / (maxVisibleValueAmount + 1);

        int xIndex = 0;

        GameObject lastDotGameObject = null;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            //GameObject barGameObject = CreateBar(new Vector2(xPosition, yPosition), xSize * .9f);
            //gameObjectList.Add(barGameObject);

            GameObject dotGameObject = CreateDot(new Vector2(xPosition, yPosition));
            gameObjectList.Add(dotGameObject);
            if (lastDotGameObject != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastDotGameObject = dotGameObject;


            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            //RectTransform dashX = Instantiate(dashTemplateX);
            //dashX.SetParent(graphContainer, false);
            //dashX.gameObject.SetActive(true);
            //dashX.anchoredPosition = new Vector2(xPosition, -3f);
            //gameObjectList.Add(dashX.gameObject);

            xIndex++;
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            gameObjectList.Add(labelY.gameObject);

            //RectTransform dashY = Instantiate(dashTemplateY);
            //dashY.SetParent(graphContainer, false);
            //dashY.gameObject.SetActive(true);
            //float normalizedValue = i * 1f / separatorCount;
            //dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            //gameObjectList.Add(dashY.gameObject);
        }
    }

    private GameObject CreateDot(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("dot", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = dotSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        //rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        return gameObject;
    }

    private GameObject CreateBar(Vector2 graphPosition, float barWidth)
    {
        GameObject gameObject = new GameObject("bar", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
        rectTransform.sizeDelta = new Vector2(barWidth, graphPosition.y);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(.5f, 0f);
        return gameObject;
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }


}