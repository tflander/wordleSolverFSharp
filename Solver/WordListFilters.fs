module Solver.WordListFilters
open Solver.Types

let DoesNotContain(quessResult: LetterAnswer) (word: string) = not (word.Contains(quessResult.Letter))
let ContainsLetterAtPosition(index: int) (guessResult: LetterAnswer) (word: string) = word.[index] = guessResult.Letter 
