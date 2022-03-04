open System

type LetterState = {
    IsInWord: bool
    IsInCorrectPosition: bool
}

type LetterAnswer = {
    Letter: char
    State: LetterState
}

let IsLetterStateValid (state: LetterState) =
    match state with
    | {IsInWord = false; IsInCorrectPosition = true} -> false
    | _ -> true
    
type Wordle(answer: string) =
    member __.Guess(word: string) =
        let chars = word.ToUpperInvariant().ToCharArray()
        List.map (fun i -> __.AnalyzeLetter(chars.[i])) [0..4]
 
    member __.AnalyzeLetter(letter: char) =
        {Letter = letter; State = {IsInWord = true; IsInCorrectPosition = true}}
    

[<EntryPoint>]
let main argv =
    printfn "Hello world"
    0
    
