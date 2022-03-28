module WordleSolverTests.Support

open Solver.Wordle

let GivenGameWithSolution(solution: string) =
    Wordle(solution)
    
let WhenGuess(guess: string) (game: Wordle) =
    game.Guess(guess)

