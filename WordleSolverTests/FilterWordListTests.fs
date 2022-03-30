module FilterWordListTests

open System
open Xunit
open Swensen.Unquote
open Solver.Wordle

open WordleSolverTests.Support
                                    
type ``FilterWordListTests`` () = 

    let sampleWords = [| "MUSIC"; "TEXAN"; "TEXAS"; "GUILD"; "RANKS"; "MARES"|]
    
    let AndFilterWordsUsingResult wordList result =
        (FilterCandidateWords result wordList), result
        
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
