using System;
using UnityEngine;
namespace SensorsComponent
{
    public class SensorFactory : Sensors
    {
        /*
            Uses Factory pattern and polymorphism to return the desired 
            Sensor type.
        */
        private static String currentSensor;

        /// <summary>Gets the instance of sensor selected.</summary>
        /// <param name="sensorType">Type of the sensor.</param>
        /// <param name="gObj">The game object for the rover.</param>
        /// <returns>Instance of the sensor selected on the rover</returns>
        public static Sensors GetInstance(int sensorType, GameObject gObj)
        {

            Rover = gObj;
            Sensors _sensor;

            switch (sensorType)
            {
                case 1:
                        SetCurrentSensor("ProximitySensor running now");                    
                        _sensor = new ProximitySensor();
                        break;

                case 2:
                        SetCurrentSensor("RangeSensor running now");                    
                        _sensor = new RangeSensor();
                        break;

                case 3:
                        SetCurrentSensor("LidarSensor running now");                    
                        _sensor = new LidarSensor();
                        break;

                case 4:
                        SetCurrentSensor("RadarSensor running now");                    
                        _sensor = new RadarSensor();
                        break;

                case 5:                        
                        SetCurrentSensor("BumperSensor running now");
                        _sensor = new BumperSensor();                       
                        break;

                default:
                        SetCurrentSensor("The chosen sensor doesn't exist! Proximity running now");
                        _sensor = new ProximitySensor();
                        break;
            }
            sensors = _sensor;
            sensors.SetSensorType(sensorType);
            return sensors;
        }

        /// <summary>Gets the instance.</summary>
        /// <returns>instance of Sensor selected on the rover</returns>
        public static Sensors GetInstance()
        {
            return sensors;
        }
    }
}
