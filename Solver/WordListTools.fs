module Solver.WordListTools

open System.Collections.Generic
open System.Text.RegularExpressions
open System

let ReadFiveLetterWords(filePath: string) =
    
    let fiveLetterWordPattern = Regex("^[A-Za-z]{5}$")
    
    let isFiveLetterWord(word: string) =
        fiveLetterWordPattern.IsMatch(word)
    
    (IO.File.ReadAllText filePath).Split("\n")
        |> Array.filter isFiveLetterWord
        |> Array.map (fun word -> word.ToUpperInvariant())

let BuildLetterValueLookup(wordList: string[]) = 
    let letterCounts = Dictionary<char, int>()
    
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
        
let ScoreWord(word: string, letterValueLookup: IDictionary<char, int>) =
    Array.fold (fun a c -> a + letterValueLookup.[c]) 0 (word.ToCharArray())
