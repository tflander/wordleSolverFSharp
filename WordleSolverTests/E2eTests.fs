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

