using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        maze = FromDimensions(mazeHeight,mazeWidth);
        buildMaze();
    }

    public int[,] FromDimensions(int sizeRows, int sizeCols)    // 2
    {
        int[,] maze = new int[sizeRows, sizeCols];

        for (int i = 0; i < sizeRows; i++)
        {
            for (int j = 0; j < sizeCols; j++)
            {
                if (i == 0 || j == 0 || i == sizeRows-1 || j == sizeCols-1)
                {
                    maze[i, j] = 1;
                }

                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (Random.value > placementThreshold)
                    {
                        //3
                        maze[i, j] = 1;

                        int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                        maze[i + a, j + b] = 1;
                    }
                }
            }
        }
        maze[1,1]=2;
        maze[sizeRows-2,sizeCols-2] = 3;
        return maze;
    }

    void buildMaze(){
        for(int i =0;  i<mazeHeight; i++){
                  for(int j=0; j<mazeWidth; j++){
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
