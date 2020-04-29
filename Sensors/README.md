# Sensors Team

## Rover Simulation - Sensors

## How to use (for Integration)
- `SensorsComponent.dll` can be found [here](https://github.com/aneeshdalvi/SER574_sensors/tree/master/Assets/Scenes)
- Copy it into the `Assets` folder of your Unity Project 

## How to run only our component in Unity (for Grader)
- Open our `Sensors` Unity project
- Go to `Scenes` folder and add `Sensors_Scene` to the Hierarchy project section of Unity
- Click on `play` game button at the top in Unity
- Your game will get started and you will see a `play` button in the game. Click on that button.
- Go to console and you will see the message "Hello from Sensors Library" 

## Links
- [Documents](https://drive.google.com/drive/folders/1kBlclT7c7DjnoQ3Ft-jvjoEezcS963Ue)
- [Taiga](https://tree.taiga.io/project/aneeshdalvi-ser574-sensors-team/timeline)
- [Sprint 1 - Youtube](https://www.youtube.com/watch?v=UtyDW7NFZY0&feature=youtu.be)
- [Sprint 2 - Youtube](https://www.youtube.com/watch?v=wXnqaxV8J8E)
- [Sprint 3 - Youtube](https://www.youtube.com/watch?v=HyUoxhcbemw&feature=youtu.be)

#### Latest DLL
[SensorsComponent.dll](/Sensors/Sensors/Assets/Scenes/)

#### DLL C Sharp Project
[DLL Project](/Sensors/Sensors/Assets/Dll_Library_Project/)

#### DLL APIs
```
class SensorsComponent
  └── class Sensors
        └── int[,] Get_Obstacle_Matrix() 
        └── string GetCurrentSensor()
        └── void Update_Obstacles(GameObject gameObj, int[,] mazeData, string direction)
        └── class SensorFactory
              └── Sensors GetInstance(int sensorType, GameObject gameObj)
              └── Sensors GetInstance() 
  ```
  
## Contributors
- Aneesh Dalvi
- Abhishek Haskar
- Nabeel Khan
- Sumanth Paranjape
- Vaibhav Singhal

## Acknowledgements
- Michael Findler
