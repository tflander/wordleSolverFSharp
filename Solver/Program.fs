open System
open System.Collections.Generic
open System.Text.RegularExpressions

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

let fiveLetterWordPattern = Regex("^[A-Za-z]{5}$")
let isFiveLetterWord(word: string) =
    fiveLetterWordPattern.IsMatch(word)

let ReadFiveLetterWords(filePath: string) =
    (IO.File.ReadAllText filePath).Split("\n")
        |> Array.filter isFiveLetterWord
        |> Array.map (fun word -> word.ToUpperInvariant())
    
[<EntryPoint>]
let main argv =
    printfn "Reading words file..."
    let fiveLetterWords = ReadFiveLetterWords("words.txt")
    printfn $"%i{fiveLetterWords.Length}"
    
    let letterCounts = new Dictionary<char, int>()
    
    let IncCharCount(c: char) =
        letterCounts.[c] <- letterCounts.GetValueOrDefault(c, 0) + 1
        
    let CountLettersInWord(word: string) =
        word.ToCharArray() |> Seq.iter IncCharCount
    
    fiveLetterWords |> Seq.iter CountLettersInWord
    
    // fiveLetterWords |> Seq.iter (fun x -> printfn $"%s{x}")
    0
    
