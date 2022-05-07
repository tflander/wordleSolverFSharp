module WordleSolverTests.E2eTests

open System
open Solver
open Solver.WordGameRunner
open Xunit
open Swensen.Unquote
open Solver.WordGame
open Solver.WordListTools

open WordleSolverTests.Support

type ``End To End Tests`` () = 

    let fiveLetterWords = ReadFiveLetterWords("words.txt")

    let randomWord (wordlist: string[]) =
        let index = Random().Next(wordlist.Length)
        wordlist.[index]

    let solveUsingAi(guessAi: string[] -> string) (solution: string) =
        let game = Guess solution
        
        let mutable gameData: GameData = {
            wordList = fiveLetterWords
            guessCount = 1
        }
        
        let mutable guess: string = guessAi gameData.wordList        
        let mutable guessResult: Types.LetterAnswer list = game guess
    
        while(not (IsGameWon guessResult)) do
            gameData <- {
                wordList = FilterCandidateWords guessResult gameData.wordList
                guessCount = gameData.guessCount + 1
            }
            guess <- randomWord gameData.wordList
            guessResult <- game guess
            
        gameData.guessCount
        
//    [<Fact(Skip = "This is spike code to delete")>]
    [<Fact>]
    member __.``Game Simulation``() =

        let game = GivenGameWithSolution("MUSIC")
        let play = solveUsingAi randomWord
        let turnsToSolve = (play game)
        test <@ turnsToSolve <= 6  @>

