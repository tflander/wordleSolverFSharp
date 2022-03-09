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

module WordListTools = 
    let fiveLetterWordPattern = Regex("^[A-Za-z]{5}$")
    let isFiveLetterWord(word: string) =
        fiveLetterWordPattern.IsMatch(word)

    let ReadFiveLetterWords(filePath: string) =
        (IO.File.ReadAllText filePath).Split("\n")
            |> Array.filter isFiveLetterWord
            |> Array.map (fun word -> word.ToUpperInvariant())

    let BuildLetterValueLookup(wordList: string[]) = 
        let letterCounts = new Dictionary<char, int>()
        
        let IncCharCount(c: char) =
            letterCounts.[c] <- letterCounts.GetValueOrDefault(c, 0) + 1
            
        let CountLettersInWord(word: string) =
            word.ToCharArray() |> Seq.iter IncCharCount
        
        wordList |> Seq.iter CountLettersInWord
        
        let kvToTuple = (fun (KeyValue(k,v)) -> (k,v))
        
        let scoreLetter = (fun i x -> fst x, i+1)

        letterCounts
            |> Seq.map kvToTuple
            |> Seq.sortBy (snd >> (~-))
            |> Seq.mapi scoreLetter
            |> dict

open WordListTools

[<EntryPoint>]
let main argv =
    printfn "Reading words file..."
    let fiveLetterWords = ReadFiveLetterWords("words.txt")
    printfn $"%i{fiveLetterWords.Length}"
            
    let letterValueLookup = BuildLetterValueLookup(fiveLetterWords)
    
//    letterValueLookup.ContainsKey('A')
//    letterValueLookup.GetType()
//    let foo = letterValueLookup['B']
    0
    
