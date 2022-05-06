module Solver.Program

open System

open Solver.WordListTools
 
[<EntryPoint>]
let main argv =
    printfn "Reading words file..."
    let fiveLetterWords = ReadFiveLetterWords("words.txt")
    printfn $"%i{fiveLetterWords.Length}"
            
    let letterValueLookup = BuildLetterValueLookup(fiveLetterWords)
    
    let score = ScoreWord("MUSIC", letterValueLookup)
    
//    let scoredWords = Array.map (fun s -> ScoreWord(s, letterValueLookup), s) fiveLetterWords
//    let foo = Seq.sortBy (fst >> (~-)) scoredWords
    
    0
    
