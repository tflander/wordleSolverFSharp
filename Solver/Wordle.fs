module Solver.Wordle

type LetterState =
    | Hit
    | NearMiss
    | Miss


type LetterAnswer = {
    Letter: char
    State: LetterState
}

let FilterWords (guessResult: LetterAnswer list) (wordList: string[]) =
    
    let FilterForMiss (index: int, result: LetterAnswer) =
        let otherResultsForTheSameLetter = List.filter (fun (thisResult: LetterAnswer) -> (thisResult.Letter = result.Letter) && (not (thisResult.State = Miss))) guessResult
        if otherResultsForTheSameLetter.Length > 0 then
            fun _ -> true
        else
            fun (word: string) -> not (word.Contains(result.Letter))
        
    let FilterForLetterAnswer (index: int, result: LetterAnswer) =
        match result.State with
            | Miss -> FilterForMiss(index, result)
            | Hit -> (fun (word: string) -> word.[index] = result.Letter)
            | NearMiss -> (fun (word: string) -> not(word.[index] = result.Letter) && word.Contains(result.Letter))
        
    let filtersToApply = List.mapi(fun i (result: LetterAnswer) -> FilterForLetterAnswer(i, result)) guessResult
    
    List.fold (fun (currentWordList: string[]) (filter: string->bool) -> Array.filter filter currentWordList) wordList filtersToApply
    
let IsGameWon (result: LetterAnswer list) =
    let hits = List.filter(fun (la: LetterAnswer) -> la.State = Hit) result
    hits.Length = 5

type Wordle(answer: string) =
    member __.Guess(word: string) =
        
        let mutable answerCopy = answer
        
        let AnalyzeLetter(letter: char, pos: int) = 
            let state =
                match answerCopy.Contains(letter) with
                | true when answerCopy.[pos] = letter -> Hit
                | true -> NearMiss
                | false -> Miss
            
            answerCopy <- answerCopy.Replace(letter, ' ')
            {Letter = letter; State = state}
        
        let chars = word.ToUpperInvariant().ToCharArray()
        List.map (fun i -> AnalyzeLetter(chars.[i], i)) [0..4]
