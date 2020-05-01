# Sensors Team

## Rover Simulation - Sensors

## How to use (for Integration)
- `SensorsComponent.dll` can be found [here](https://github.com/aneeshdalvi/SER574_sensors/tree/master/Assets/Scenes)
- Copy it into the `Assets` folder of your Unity Project 

## How to run our component individually in Unity 
- Open our `Sensors` Unity project
- Go to `Scenes` folder and add `Sensors_Scene` to the Hierarchy project section of Unity
- Click on `play` game button at the top in Unity
- Your game will get started and you will see a `play` button in the game. Click on that button.
- Select the sensor you want to see. Click on `start`
- You will be redirected to a new scene where you can control the rover and the sensor will be attached to the rover. 
- Move the rover as you need.
- Go to console and you will see if obstacle matrix generated as required.

## How to run our component in class project integration
- Open our `Integration/Class_Project` in Unity project
- Go to the `MainScreen` and hit `play` 
- Select any UI or Exp Design button
- Select the appropriate Sensor to be selected for the Simulation
- You will move to the respective screen where you will se `Create maze` option.
- Hit that and a maze will be generated. Click on `Manual` or `Automatic` to see the Sensors in action.
- After the iteration of exploring maze ends, the sensors logs are updated in the database `rover.db` and `experimental_results.db` also gets updated with the sensorType selected after all the experimenets are over.
- On the UI screen, a maze is generated using the sensor matrix generated at each step of the exploration.

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
