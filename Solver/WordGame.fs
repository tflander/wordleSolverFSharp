module Solver.WordGame
open Solver.Types
open Solver.WordListFilters

let Guess(solution: string)(guess: string) =
    
    let mutable solutionCopy = solution
    
    let AnalyzeLetter(pos: int) (letter: char) = 
        let state =
            match solutionCopy.Contains(letter) with
            | true when solutionCopy.[pos] = letter -> Hit
            | true -> NearMiss
            | false -> Miss
        
        solutionCopy <- solutionCopy.Replace(letter, ' ')
        {Letter = letter; State = state}
    
    guess.ToUpperInvariant().ToCharArray()
              |> Array.mapi AnalyzeLetter
              |> Array.toList

let FilterCandidateWords (guessResult: LetterAnswer list) (wordList: string[]) =
    
    let FilterForMiss (result: LetterAnswer) =
        let otherResultsForTheSameLetter =
           guessResult
           |> List.filter (fun (thisResult: LetterAnswer) -> (thisResult.Letter = result.Letter) && (not (thisResult.State = Miss)))
           
        if otherResultsForTheSameLetter.Length > 0 then
            fun _ -> true  // TODO: should this be false if the letter does not repeat in the solution?
        else
            DoesNotContain result
        
    let FilterForLetterAnswer (index: int) (result: LetterAnswer) =
        match result.State with
            | Miss -> FilterForMiss(result)
            | Hit -> ContainsLetterAtPosition index result
            | NearMiss -> (fun (word: string) -> not(word.[index] = result.Letter) && word.Contains(result.Letter))
        
    guessResult
        |> List.mapi(FilterForLetterAnswer)
        |> List.fold (fun (currentWordList: string[]) (filter: string->bool) -> Array.filter filter currentWordList) wordList 
    
let IsGameWon (result: LetterAnswer list) =
    let isHit(la: LetterAnswer) = la.State = Hit
    let hitsInResult = result |> List.filter isHit
    hitsInResult.Length = 5
