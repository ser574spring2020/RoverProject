/*
    Authors   : Sumanth Paranjape
    Function  : Factory Pattern for creating different Sensors. Inherits Sensors.
    Version   : V2
    Email     : sparanj2@asu.edu | Arizona State University.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorFactory : Sensors{
    /*
        Uses Factory pattern and polymorphism to return the desired 
        Sensor type.
    */
    public static Sensors getInstance(int sensorType, GameObject gObj){

        Cube = gObj;
        Sensors _sensor;

        switch (sensorType)
            {
                case 1: _sensor = new ProximitySensor();
                        break;

                case 2: _sensor = new RangeSensor();
                        break;

                case 3: _sensor = new LidarSensor();
                        break;

                default: Debug.Log("The chosen sensor doesn't exist!");
                         _sensor = new ProximitySensor();
                         break;
            }
            sensors = _sensor;
            return sensors;
    }

    public static Sensors getInstance(){
        return sensors;
    }
}