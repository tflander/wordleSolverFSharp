module Tests

open System
open Xunit
open Program
open Swensen.Unquote

let GivenGameWithSolution(solution: string) =
    Wordle(solution)
    
type ``Evauluate guess tests`` () = 
    
    let ExpectResultForGuess(guess: string, expectedStates: LetterState list) =
        let chars = guess.ToCharArray()
        chars
            |> Array.mapi (fun i c -> {Letter = c; State = expectedStates.[i]})
            |> Array.toList
        
    let WhenGuess(guess: string) (game: Wordle) =
        game.Guess(guess)
        
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
                                

open WordListTools
                                
type ``FilterWordListTests`` () = 

    let fiveLetterWords = ReadFiveLetterWords("words.txt")
    
    [<Fact>]
    member __.``Foo``() =
        let game = GivenGameWithSolution("MUSIC")
        let response = game.Guess("TEXAN")
        let updatedWordList = FilterWords response fiveLetterWords
        test <@ 1 = 2 @>
                                