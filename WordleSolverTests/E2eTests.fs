module WordleSolverTests.E2eTests

open System
open Solver
open Xunit
open Swensen.Unquote
open Solver.Wordle
open Solver.WordListTools

open WordleSolverTests.Support

type ``End To End Tests`` () = 

    let fiveLetterWords = ReadFiveLetterWords("words.txt")

    let randomWord (wordlist: string[]) =
        let index = Random().Next(wordlist.Length)
        wordlist.[index]

    let play(solution: string) =
        let game = Guess solution
        let mutable wordList = fiveLetterWords
        let mutable guessCount = 1
        let mutable guess = randomWord wordList        
        let mutable guessResult = game guess
    
        while(not (IsGameWon guessResult)) do
            guessCount <- guessCount + 1
            wordList <- FilterWords guessResult wordList
            guess <- randomWord wordList
            guessResult <- game guess
            
        test <@ guessCount <= 6  @>
        
//    [<Fact(Skip = "This is spike code to delete")>]
    [<Fact>]
    member __.``Game Simulation``() =

        let game = GivenGameWithSolution("MUSIC")
        play game

