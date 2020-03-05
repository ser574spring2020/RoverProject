using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    float xStart = 0, yStart = 0;
    float xSpace = 0.5f, ySpace = 0.5f;
    public GameObject wallPrefab, endPointPrefab, startPointPrefab, floorPrefab, pathPrefab;
    public Button createMaze;
    public int mazeHeight, mazeWidth, sparsity;
    GameObject[] mazeObjects;
    int counter = 0;
    // int[,] maze ;
    int[,] maze;
    // = new int[,]{{0, 1, 0, 1, 0, 1, 8},
    //                         {0, 1, 0, 2, 2, 2, 2},
    //                         {1, 1, 1, 2, 1, 1, 1},
    //                         {9, 1, 2, 2, 0, 1, 0},
    //                         {2, 2, 2, 0, 0, 1, 0}};
    System.Random rand = new System.Random();
    void Start()
    {
        mazeObjects = new GameObject[mazeHeight*mazeWidth];
        createMaze.onClick.AddListener(createMazeButtonListener);
    }

    void createMazeButtonListener(){
        destroyMaze();
        createMazeArray();
        buildMaze();
    }

    void createMazeArray(){
        maze = new int[mazeHeight,mazeWidth];
        for(int i =0; i<mazeHeight;i++){
          for(int j =0; j<mazeWidth;j++){
              if(i==1 && j==1){
                  maze[i,j]=0;
              }
              else if(i==mazeHeight-2 && j==mazeWidth-2){
                  maze[i,j]=9;
              }
              else if(i==0 || j==0 || i==mazeHeight-1 || j==mazeWidth-1){
                  maze[i,j] = 1;
              }
              else
              {
                  maze[i,j] = rand.Next(1,sparsity+3);
              }
            }  
        }
    }

    void buildMaze(){
        for(int i =0;  i<mazeHeight; i++){
                  for(int j=0; j<mazeWidth; j++){
                      if(maze[i,j]==1){
                          mazeObjects[counter++] = Instantiate(wallPrefab,new Vector3(xStart + (xSpace*i),0.1f,yStart+(j*ySpace)), Quaternion.identity);
                      }
                      else if(maze[i,j]==0){
                          mazeObjects[counter++] = Instantiate(startPointPrefab,new Vector3(xStart + (xSpace*i),1,yStart+(j*ySpace)), Quaternion.identity);
                      }
                      else if(maze[i,j]==99){
                          mazeObjects[counter++] = Instantiate(pathPrefab,new Vector3(xStart + (xSpace*i),0.25f,yStart+(j*ySpace)), Quaternion.identity);
                      }
                      else if(maze[i,j]==9){
                          mazeObjects[counter++] = Instantiate(endPointPrefab,new Vector3(xStart + (xSpace*i),1,yStart+(j*ySpace)), Quaternion.identity);
                      }
                      else{
                          mazeObjects[counter++] = Instantiate(floorPrefab,new Vector3(xStart + (xSpace*i),0.1f,yStart+(j*ySpace)), Quaternion.identity);
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
