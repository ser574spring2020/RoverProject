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
        └── void Update_Obstacles(GameObject gameObj)
        └── class SensorFactory
              └── Sensors GetInstance(int sensorType, GameObject gObj)
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
