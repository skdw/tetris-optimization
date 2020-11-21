# tetris-optimization 
![.NET Core](https://github.com/skdw/tetris-optimization/workflows/.NET%20Core/badge.svg)
[![codecov](https://codecov.io/gh/skdw/tetris-optimization/branch/main/graph/badge.svg?token=5VAJVKNU49)](https://codecov.io/gh/skdw/tetris-optimization)

The algorithm arranges tetris puzzles to minimize their area. Overlapping is not permitted. 

# Input parameters
The program is being carried out using parameters indicated in a .json format configuration file and implemented by BlocksSolverFactory class. The parameters are specified for each of the 4 algorithms. Each of the algorithms accepts a block size and a block list (in the configuration file it is a list of numbers, which is turned into a block list by the program). 

The optimal algorithms, working on consecutive combinations, take a ParallelStep parameter from the configuration file. It indicates how many combinations are being processed simultaneaosuly. After each such step, the result is put in a ConcurrentBag collection, where the most optimal board found is kept. If the optimal solution is not found, the ConcurrentBag collection is cleared and the algorithm moves on to next combinations

The heuristic algorithms accept following additional arguments:

HPnumPermutation - a number of random block permutations (rectangle heuristic & square heuristic)

HPmultiplier - a multiplier of board area with respect to coveted rectangle area (rectangle heuristic)

HKnumLists - a number of block lists consisting of block position and rotation (square heuristic)

HKpercentage - a fraction of best block lists accepted in each while loop (square heuristic)

HPercentageBoardSize - a fraction of max board area, on which the blocks will be put (square heuristic)

# Stages

1. Optimal square
2. Approximately optimal rectangle
3. Optimal rectangle
4. Approximately optimal square

# Sources

https://github.com/mzmousa/tetris-ai

http://cslibrary.stanford.edu/112/Tetris-Architecture.html

https://github.com/uiucanh/tetris





