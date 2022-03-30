module WordleSolverTests.Support

open Solver.WordGame

let GivenGameWithSolution(solution: string) =
    solution
    
let WhenGuess(guess: string) (solution: string) =
    Guess solution guess

