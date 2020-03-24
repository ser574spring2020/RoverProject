using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeGenerator;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    float xStart = 0, yStart = 0;
    float xSpace = 0.5f, ySpace = 0.5f;
    public float placementThreshold; 
    public GameObject wallPrefab, endPointPrefab, startPointPrefab, floorPrefab, pathPrefab;
    public Button createMaze;
    public int mazeHeight, mazeWidth;
    GameObject[] mazeObjects;
    int counter = 0;
    int[,] maze;
    System.Random rand = new System.Random();
    void Start()
    {
        mazeObjects = new GameObject[mazeHeight*mazeWidth];
        createMaze.onClick.AddListener(createMazeButtonListener);
    }

    void createMazeButtonListener(){
        destroyMaze();
        maze = MazeGenerator.MonoBehaviour1.FromDimensions(mazeHeight,mazeWidth,placementThreshold);
        buildMaze();
    }

    
    void buildMaze(){
        for(int i =0;  i<mazeHeight; i++){
                  for(int j=0; j<mazeWidth; j++){
                      Debug.Log(maze[i,j]);
                      Vector3 tempVector = new Vector3(xStart + (xSpace*i),0,yStart+(j*ySpace));
                      if(maze[i,j]==0){
                          mazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                      }
                      else if(maze[i,j]==1){
                          mazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                      }
                      else if(maze[i,j]==2){
                          mazeObjects[counter++] = Instantiate(startPointPrefab, tempVector, Quaternion.identity);
                      }
                      else if(maze[i,j]==3){
                          mazeObjects[counter++] = Instantiate(endPointPrefab, tempVector, Quaternion.identity);
                      }
                  }
              }
    }

    void destroyMaze(){
        for(int i=0;i<counter;i++){
            Destroy(mazeObjects[i]);
        }
        Debug.Log("destructed suceesd");    
        counter=0;
    }
    void Update()
    {
        
    }
}
