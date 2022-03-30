module EvaluateGuessTests

open System
open Solver.Domain
open Xunit
open Swensen.Unquote

open WordleSolverTests.Support
    
type ``Evaluate guess tests`` () = 
                    
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
                                
                                
    [<Fact>]
    member __.``Double Letter Guessed Correct Position``() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("GASES")
            |> ExpectResult([Miss; Miss; Hit; Miss; Miss])
                                
