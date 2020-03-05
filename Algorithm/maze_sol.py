# A utility function to print solution matrix sol 
def printSolution(sol):    
    for i in sol: 
        for j in i: 
            print(str(j) + " ", end ="") 
        print("")

# A utility function to check if x, y is valid for Maze 
def isSafe(maze,x,y):
    if 0 <= x < len(maze) and 0 <= y < len(maze[0]):
        val = maze[x][y]
        if(val == 0 or val == 8 or val == 9): 
            return True
    return False

#Function to return location of a particular index in 2D array
def index_2d(arr, v):
    for i, x in enumerate(arr):
        if v in x:
            return (i, x.index(v))

def solveMaze(maze):
    # 2D list
    rows = len(maze)
    cols = len(maze[0])
    nopath = [[0 for j in range(cols)] for i in range(rows)]
    
    print(maze)
    start_x,start_y = index_2d(maze,8)
    
    if solveMazeUtil(maze, start_x, start_y, nopath) == False: 
        print("Solution doesn't exist"); 
        return False
      
    printSolution(maze)
    return True
      
# A recursive utility function to solve Maze problem 
def solveMazeUtil(maze, x, y, nopath):      
    # if (x, y is goal) return True
    print(maze)
    if isSafe(maze, x, y) == True:
        if maze[x][y] == 9:
            return True
  
    # Check if maze[x][y] is valid 
    if isSafe(maze, x, y) == True and nopath[x][y] != 1: 
        # mark x, y as part of solution path 
        if(maze[x][y] == 8):
            maze[x][y] = 8
        else:
            maze[x][y] = 2
        
        # down, right, left, up
        directions = [[1,0],[0,1],[0,-1],[-1,0]]
        # Move forward in x direction 
        
        for d in directions:
            xd = x + d[0]
            yd = y + d[1]
            if solveMazeUtil(maze, xd, yd, nopath) == True:
                return True

        # If none of the above movements work then  
        # BACKTRACK: unmark x, y as part of solution path 
        maze[x][y] = 0
        nopath[x][y] = 1
        print("No Path",nopath)
        return False

# Driver program to test above function 
if __name__ == "__main__": 
    # Initialising the maze 
    maze = [[1, 8, 0, 0], 
            [1, 1, 0, 1], 
            [0, 1, 0, 9], 
            [1, 1, 1, 1]]
    maze = [[0, 1, 0, 1, 0, 1, 8],
            [0, 1, 0, 0, 0, 0, 0],
            [1, 1, 1, 0, 1, 1, 1],
            [9, 1, 0, 0, 0, 1, 0],
            [0, 0, 0, 0, 0, 1, 0]]
               
    print(solveMaze(maze))