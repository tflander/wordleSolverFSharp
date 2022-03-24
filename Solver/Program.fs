open System
open System.Collections.Generic
open System.Text.RegularExpressions

type LetterState =
    | Hit
    | NearMiss
    | Miss


type LetterAnswer = {
    Letter: char
    State: LetterState
}

let FilterWords (guessResult: LetterAnswer list) (wordList: string[]) =
    let FilterForResult (result: LetterAnswer) =
        match result.State with
        | Miss -> (fun (word: string) -> not (word.Contains(result.Letter)))
        
//    wordList
//        |> Array.filter (FilterForResult(guessResult.[0]))

    let foo = List.map(fun (result: LetterAnswer) -> FilterForResult(result)) guessResult
    
    List.fold (fun (currentWordList: string[]) (filter: string->bool) -> Array.filter filter currentWordList) wordList foo
    
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
 
module WordListTools = 

    let ReadFiveLetterWords(filePath: string) =
        
        let fiveLetterWordPattern = Regex("^[A-Za-z]{5}$")
        
        let isFiveLetterWord(word: string) =
            fiveLetterWordPattern.IsMatch(word)
        
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
    
