module WordleSolverTests.Support

open Solver.Wordle

let GivenGameWithSolution(solution: string) =
    solution
    
let WhenGuess(guess: string) (solution: string) =
    Guess solution guess

