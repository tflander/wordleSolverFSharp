module Tests

open System
open Xunit
open Swensen.Unquote
open Solver.Wordle
open Solver.WordListTools

let GivenGameWithSolution(solution: string) =
    Wordle(solution)
    
let WhenGuess(guess: string) (game: Wordle) =
    game.Guess(guess)
    
type ``Evauluate guess tests`` () = 
                    
    let ExpectResult(expectedStates: LetterState list) (actualResult: LetterAnswer list) =
        let actualStates = List.map (fun answer -> answer.State) actualResult
        test <@ actualStates = expectedStates @>

    [<Fact>]
    member __.``All Hits``() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("MUSIC")
            |> ExpectResult([Hit; Hit; Hit; Hit; Hit])
                                    
    [<Fact>]
    member __.``All Misses``() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("TEXAN")
            |> ExpectResult([Miss; Miss; Miss; Miss; Miss])
                
    [<Fact>]
    member __.``One Near Miss``() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("TEXAS")
            |> ExpectResult([Miss; Miss; Miss; Miss; NearMiss])
                
    [<Fact>]
    member __.``Double Letter Guessed Wrong Position``() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("GUESS")
            |> ExpectResult([Miss; Hit; Miss; NearMiss; Miss])
                                
                                
type ``FilterWordListTests`` () = 

    let fiveLetterWords = ReadFiveLetterWords("words.txt")
    
    let sampleWords = [| "MUSIC"; "TEXAN"; "TEXAS"; "GUILD"; "RANKS"; "MARES"|]
    
    let AndFilterWordsUsingResult wordList result =
        (FilterWords result wordList), result
        
    let ExpectFilteredWords (expected: string[]) (actual: string[], guessResult) =
        test <@ expected = actual @>
        guessResult
        
    let AndGameIsNotWon(guessResult) = 
        test <@ not (IsGameWon guessResult) @>
        
    let AndGameIsWon(guessResult) = 
        test <@ IsGameWon guessResult @>
        
    [<Fact>]
    member __.``No Letter Matches``() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("TEXAN")
            |>   AndFilterWordsUsingResult sampleWords
            |> ExpectFilteredWords [| "MUSIC"; "GUILD" |]
            |>   AndGameIsNotWon
        
        
    [<Fact>]
    member __.``One Hit``() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("MERRY")
            |>   AndFilterWordsUsingResult sampleWords
            |> ExpectFilteredWords [| "MUSIC" |]
            |>   AndGameIsNotWon
        
    [<Fact>]
    member __.``One Near Miss``() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("TEXAS")
            |>   AndFilterWordsUsingResult sampleWords
            |> ExpectFilteredWords [| "MUSIC" |]
            |>   AndGameIsNotWon

    [<Fact>]
    member __.``Double S``() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("GUESS")
            |>   AndFilterWordsUsingResult sampleWords
            |> ExpectFilteredWords [| "MUSIC" |]
            |>   AndGameIsNotWon
            
    [<Fact>]
    member __.``Double C``() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("LUCIC")
            |>   AndFilterWordsUsingResult sampleWords
            |> ExpectFilteredWords [| "MUSIC" |]
            |>   AndGameIsNotWon

    [<Fact>]
    member __.``Exact Match``() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("MUSIC")
            |>   AndFilterWordsUsingResult sampleWords
            |> ExpectFilteredWords [| "MUSIC" |]
            |>   AndGameIsWon

//    [<Fact(Skip = "This is spike code to delete")>]
    [<Fact>]
    member __.``Game Simulation``() =
        let randomWord (wordlist: string[]) =
            let index = (new System.Random()).Next(wordlist.Length)
            wordlist.[index]

        let mutable wordList = fiveLetterWords
        let game = GivenGameWithSolution("MUSIC")
        
        let foo() = 
            let mutable guessCount = 1
            let mutable guess = randomWord wordList        
            let mutable guessResult = game.Guess guess
        
            while(not (IsGameWon guessResult)) do
                guessCount <- guessCount + 1
                wordList <- FilterWords guessResult wordList
                guess <- randomWord wordList
                guessResult <- game.Guess guess
                
            test <@ guessCount <= 6  @>
            
        foo()
