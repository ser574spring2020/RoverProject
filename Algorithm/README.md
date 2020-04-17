# Algorithms Team

#### Latest DLL
- [DRL.dll](/Algorithm/DRL/DRL/bin/Debug)

#### How to run?
- [Windows Build](/Algorithm/Map%20Generator/WindowsBuild) - *(RoverSimulation-AlgorithmsTeam.exe)*
- [Linux Build](/Algorithm/Map%20Generator/LinuxBuild) - *( 	LinuxBuild.x86_64)*

## Links
- [Documents](https://drive.google.com/open?id=1gdqUG-G0XlVtYJZFXqgZue7gCKFo_VYj)
- [API Document - Github](https://github.com/ser574spring2020/RoverProject/tree/master/Algorithm/API%20Docs)
- [Sprint 1 - Youtube Video](https://www.youtube.com/watch?v=gztgx2QT6So)
- [Sprint 2 - Youtube Video](https://www.youtube.com/watch?v=kdmzKdTH5-k)

## How to generate dataset?
- Start the Unity Project
- Click on *Create Maze*
- Use the <kbd>W</kbd> <kbd>A</kbd> <kbd>S</kbd> <kbd>d</kbd> to move north, west, south and east.
- The dataset is saved at [/Datasets/Dataset.csv](https://github.com/ser574spring2020/RoverProject/tree/master/Algorithm/Map%20Generator/Datasets)


## Contributors
- Amit Pandey
- Jonathan Bush
- Karandeep Singh Grewal
- Mayank Batra
- Mayank Rawat
- Ria Mehta

## File Structure (For team use only)
Algorithm (Root)

```
├── algofind.cs                                                                   (AMIT PANDEY)
│
├── Back Propagation
│   └── Back Propagation.cs
│
├── DRL
│   └── DRL
│       └── bin
│           └── Debug
│                  └── DRL.dll                                                   (FOR DEEP REINFORCEMENT LEARNING)
│
├── Map Generator                                                                 (MAIN UNITY PROJECT)
│   └── Assets
│       ├── Scenes
│       │   └── SampleScene.unity                                                 (DEFAULT SCENE)
│       └── Scripts
│           └── AlgorithmsSimulation.cs
│
├── Maze Navigator
│   └── Assets
│       └── Scripts
│           └── MazeExplorationMap.cs                                             (OBJECT ORIENTED STRUCTURE FOR MAZE - JONATHAN)
│
├── maze_sol.py                                                                   (AMIT PANDEY)
│
├── README.md
│
├── RobotCommands
│   └── RobotCommands.cs                                                          (MAYANK BATRA)
│
└── ShortestPathBfs.cs                                                            (MAYANK RAWAT)
```
