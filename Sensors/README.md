# SER574_sensors

## Rover Simulation - Sensors

## How to use (for other component Teams)
- `Sensors1.dll` can be found [here](https://github.com/ser574spring2020/RoverProject/tree/master/Sensors/Assets/Scenes)
- Copy it into the `Assets` folder of your Unity Project 

## How to run only our component in Unity (for Grader)
- Open our Unity project
- You will see two scenes in the `Scenes` folder. Select and add `Sensors_Scene` to the Hierarchy project Section of Unity. 
- Click on `play` game button at the top in Unity.
- Your game will get started and you will see a input box and a `play` button in the game. 
- Select numbers from `1-6` range for the input box and hit `play` button.
- Wait for 10secs as the unity will automatically load a new Scene `SensorTest`
- You will see two cubes on screen which you can move forward and backward by keys `w` and `s` respectively.
- Go to console and you will see the selected sensor as well as the distance between the two cubes. 
- As you move the cubes, the distance will change.

Sensors.dll

```
class Sensors
  └── void String chooseSensor(int SensorType)
  ```
  
## Contributors
- Aneesh Dalvi
- Abhishek Haskar
- Nabeel Khan
- Sumanth Paranjape
- Vaibhav Singhal

## Acknowledgements
- Michael Findler
