open System
open System.Collections.Generic
open System.Text.RegularExpressions

open Solver.Wordle
open Solver.WordListTools
 
module WordListTools = 

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
            
    let ScoreWord(word: string, letterValueLookup: IDictionary<char, int>) =
        Array.fold (fun a c -> a + letterValueLookup.[c]) 0 (word.ToCharArray())

open WordListTools

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
    
