# API Docs: Sensors Team

- [API Access](https://docs.google.com/document/d/1pZVMKACP_liasknJdSZ44ZHgd-G1dpZ5On7DsjcA6tY/edit)

## SensorFactory : Class

- Namespace	:	SensorsComponent
- Assembly 	:	SensorsComponent.dll

> SensorFactory Class - This class creates an instance of the Sensor chosen on the Rover object.

<table>
  <tr>
   <td><strong>Class</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td>Sensors.GetInstance(GameObject)
   </td>
   <td>Gets the instance of sensor selected.
   </td>
  </tr>
  <tr>
   <td>Sensors.GetInstance()
   </td>
   <td>Gets the instance of sensor selected.
   </td>
  </tr>
</table>

### Sensors.GetInstance Method (Int32, GameObject)

Gets the instance of sensor selected.

Namespace: SensorsComponent
Assembly: SensorsComponent (in SensorsComponent.dll)

``` 
  public static Sensors GetInstance(
    int sensorType,
    GameObject gObj
  )
```

#### Parameters

- gameObj
    - Type: GameObject
    - The game object of Rover.

- sensorType
    - Type: Int32
    - Type of the sensor.
    
#### Return Value
- Instance of the sensor selected on the rover
    
### Sensors.GetInstance Method 

Gets the instance of sensor selected.

Namespace: SensorsComponent
Assembly: SensorsComponent (in SensorsComponent.dll)

``` 
  public static Sensors GetInstance(
    int sensorType,
    GameObject gObj
  )
```

#### Return Value
- Instance of the sensor selected on the rover


___


## Sensors : Class

- Namespace	:	SensorsComponent
- Assembly 	:	SensorsComponent.dll

> Sensors Class - This class is the parent class for all the Sensor classes. This class is used to access all other classes.

<table>
  <tr>
   <td><strong>Class</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td>Sensors.Update_Obstacles()
   </td>
   <td>Updates the obstacles in the obstacle_matrix
   </td>
  </tr>
  <tr>
   <td>Sensors.GetRoverObject()
   </td>
   <td>Gets the rover object
   </td>
  </tr>
  <tr>
   <td>Sensors.Get_Obstacle_Matrix() 
   </td>
   <td>Gets the obstacle matrix of current position of Rover
   </td>
  </tr>
  <tr>
   <td>Sensors.GetSensorType() 
   </td>
   <td>Gets the value of SensorType set.
   </td>
  </tr>
  <tr>
   <td>Sensors.GetCurrentSensor() 
   </td>
   <td>Gets the information for current sensor
   </td>
  </tr>
</table>

### Sensors.Update_Obstacles Method 

Updates the obstacles in the obstacle_matrix

Namespace: SensorsComponent
Assembly: SensorsComponent (in SensorsComponent.dll)

``` 
  public virtual void Update_Obstacles(
    GameObject gameObj,
    int[,] mazeData,
    string Direction
  ) 
```

#### Parameters

- gameObj
    - Type: GameObject
    - The game object of Rover.

- mazeData
    - Type: Int32[,]
    - The maze matrix chosen

- Direction
    - Type: string
    - The direction in which the rover starts
    
***

### Sensors.GetRoverObject Method 

Gets the rover object.

Namespace: SensorsComponent
Assembly: SensorsComponent (in SensorsComponent.dll)

``` 
  public GameObject GetRoverObject()
```

#### Return Value
  - Returns the Rover Object

***

### Sensors.Get_Obstacle_Matrix Method 

Gets the obstacle matrix.

Namespace: SensorsComponent
Assembly: SensorsComponent (in SensorsComponent.dll)

``` 
  public int[,] Get_Obstacle_Matrix()
```

#### Return Value
  - Returns the obstacle matrix at the current position of the rover
  
***

### Sensors.GetSensorType Method 

Gets the type of the sensor.

Namespace: SensorsComponent
Assembly: SensorsComponent (in SensorsComponent.dll)

``` 
  public int GetSensorType()
```

#### Return Value
  - Returns SensorType value

***

### Sensors.GetCurrentSensor Method 

Gets the current sensor.

Namespace: SensorsComponent
Assembly: SensorsComponent (in SensorsComponent.dll)

``` 
  public String GetCurrentSensor()
```

#### Return Value
  - Returns string logging which sensor is currently been used
  
