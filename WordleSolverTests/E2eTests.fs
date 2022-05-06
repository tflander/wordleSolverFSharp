module WordleSolverTests.E2eTests

open System
open Solver
open Xunit
open Swensen.Unquote
open Solver.WordGame
open Solver.WordListTools

open WordleSolverTests.Support

type GameData = {
    wordList: string[]
    guessCount: int
}

type ``End To End Tests`` () = 

    let fiveLetterWords = ReadFiveLetterWords("words.txt")

    let randomWord (wordlist: string[]) =
        let index = Random().Next(wordlist.Length)
        wordlist.[index]

    let play(solution: string) =
        let game = Guess solution
        
        let mutable gameData: GameData = {
            wordList = fiveLetterWords
            guessCount = 1
        }
        
        let mutable guess: string = randomWord gameData.wordList        
        let mutable guessResult: Types.LetterAnswer list = game guess
    
        while(not (IsGameWon guessResult)) do
            gameData <- {
                wordList = FilterCandidateWords guessResult gameData.wordList
                guessCount = gameData.guessCount + 1
            }
            guess <- randomWord gameData.wordList
            guessResult <- game guess
            
        test <@ gameData.guessCount <= 6  @>
        
//    [<Fact(Skip = "This is spike code to delete")>]
    [<Fact>]
    member __.``Game Simulation``() =

        let game = GivenGameWithSolution("MUSIC")
        play game

