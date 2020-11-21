# tetris-optimization 
![.NET Core](https://github.com/skdw/tetris-optimization/workflows/.NET%20Core/badge.svg)
[![codecov](https://codecov.io/gh/skdw/tetris-optimization/branch/main/graph/badge.svg?token=5VAJVKNU49)](https://codecov.io/gh/skdw/tetris-optimization)

The algorithm arranges tetris puzzles to minimize their area. Overlapping is not permitted. 

# Stages

1. Optimal square
2. Approximately optimal rectangle
3. Optimal rectangle

# Sources

https://github.com/mzmousa/tetris-ai

http://cslibrary.stanford.edu/112/Tetris-Architecture.html

https://github.com/uiucanh/tetris

# Parametry wejściowe
Program wykonuje się dla ustawionych poprzez plik konfik=guracyhny, a następnie wprowadzanych poprzez BlocksSolverFactory parametrów wejściowych. Parametry są określone dla każdego z 4 algorytmów. Każdy z nich przyjmuje na wejściu wielkość klocka i listę klocków -podanych w pliku konfiguracyjnym jako listę liczb, która przez program jest przekształcana na listę klocków. Ponadto algorytmy heyrystyczny mają dodatkowe argumenty wejściowe:
HPnumPermutation - liczba losowych permutacji klocków
HPmultiplier - mnożnik
HKnumLists - liczba list klocków z ich położenie na osi x i obrotem
HKpercentage - procent najlepszych list akceptowanych w każdm przejściu pętli while 
HPercentageBoardSize - procent maksymalnej wielkości planszy, na której będą ukłądane klocki




