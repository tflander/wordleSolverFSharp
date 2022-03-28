module Solver.WordListTools

open System.Text.RegularExpressions
open System

let ReadFiveLetterWords(filePath: string) =
    
    let fiveLetterWordPattern = Regex("^[A-Za-z]{5}$")
    
    let isFiveLetterWord(word: string) =
        fiveLetterWordPattern.IsMatch(word)
    
    (IO.File.ReadAllText filePath).Split("\n")
        |> Array.filter isFiveLetterWord
        |> Array.map (fun word -> word.ToUpperInvariant())
