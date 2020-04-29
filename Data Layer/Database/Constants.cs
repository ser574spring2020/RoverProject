/// <summary>
/// This is the encapsulation for Database.
///
/// Author: Bingrui Feng
/// </summary>
namespace Assets.Scripts.Database {
    public static class Constants {
        public enum MAZE_OBJECT {
            OpenArea = 0,
            Wall = 1,
            Starting = 2,
            Ending = 3,
            Path = 4,
            Robot = 5
        }

        public static readonly int RESPONSE_CODE_SUCCESS = 1;
        public static readonly int RESPONSE_CODE_FAILURE = 0;

        public static readonly string DATABASE_NAME = "Rover.db";

        public static readonly string TABLE_MAZE = "Maze";
        public static readonly string TABLE_COMMANDS = "Commands";
        public static readonly string TABLE_PATH = "Path";
        public static readonly string TABLE_SIMULATIONTYPE = "SimulationType";
        public static readonly string TABLE_SENSOR = "Sensor";
        public static readonly string TABLE_ALGORITHMTYPE = "AlgorithmType";

        public static readonly string COLUMN_ID = "Id";
        public static readonly string COLUMN_NODE = "Node";
        public static readonly string COLUMN_CONNECTTO = "ConnectTo";
        public static readonly string COLUMN_DIRECTION = "Direction";
        public static readonly string COLUMN_DESCRIPTION = "Description";

        public static readonly string SENSOR_CONTENT = "Content";
        public static readonly string SENSOR_TIMESTAMP = "TimeStamp";

        public static readonly string MAZE_MATRIX = "Matrix";
        public static readonly string MAZE_COVERAGE = "Coverage";
        public static readonly string MAZE_TIMETAKEN = "Timetaken";
        public static readonly string MAZE_HISTORY = "History";
        public static readonly string MAZE_POINTS = "Points";
    }
}
