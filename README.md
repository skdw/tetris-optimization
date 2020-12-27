# tetris-optimization 
![.NET Core](https://github.com/skdw/tetris-optimization/workflows/.NET%20Core/badge.svg)
[![codecov](https://codecov.io/gh/skdw/tetris-optimization/branch/main/graph/badge.svg?token=5VAJVKNU49)](https://codecov.io/gh/skdw/tetris-optimization)

The algorithm arranges tetris puzzles (and other polyominoes) to minimize their area on board.

# Usage

#### App run

```
$ dotnet run -p TetrisOptimization [data_file]
```

#### Run tests
```
$ dotnet test
```

## Input parameter - data_file

Each of the algorithms accept the block size, the selected solving algorithm type and the list of blocks ID-s.

The input data file follows the specification written in [#17](https://github.com/skdw/tetris-optimization/issues/17) (unfortunately in Polish).

# BlocksSolverFactory configuration

The optimal algorithms, working on consecutive combinations, take the ParallelStep parameter. It indicates how many combinations are being processed simultaneaosuly. After each such step, the result is put in a ConcurrentBag collection, where the most optimal board found is kept. If the optimal solution is not found, the ConcurrentBag collection is cleared and the algorithm moves on to next combinations

The heuristic algorithms accept following additional arguments:

- HPnumPermutation - a number of random block permutations (rectangle heuristic & square heuristic)

- HPmultiplier - a multiplier of board area with respect to coveted rectangle area (rectangle heuristic)

- HKnumLists - a number of block lists consisting of block position and rotation (square heuristic)

- HKpercentage - a fraction of best block lists accepted in each while loop (square heuristic)

- HPercentageBoardSize - a fraction of max board area, on which the blocks will be put (square heuristic)

# Algorithms

1. Optimal square
2. Approximately optimal rectangle
3. Optimal rectangle
4. Approximately optimal square

# Sources

https://github.com/mzmousa/tetris-ai

http://cslibrary.stanford.edu/112/Tetris-Architecture.html

https://github.com/uiucanh/tetris
