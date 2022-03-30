module Solver.Types

type LetterState =
    | Hit
    | NearMiss
    | Miss


type LetterAnswer = {
    Letter: char
    State: LetterState
}
