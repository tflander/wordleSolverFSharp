module Tests

open System
open Xunit
open Program
open Swensen.Unquote
    
type ``Evauluate guess tests`` () = 
//    static member IsLetterStateValidTestData 
//        with get() : obj[] list = 
//        [
//            [| {IsInWord = false; IsInCorrectPosition = false}; true |]
//            [| {IsInWord = false; IsInCorrectPosition = true}; false |]
//            [| {IsInWord = true; IsInCorrectPosition = false}; true |]
//            [| {IsInWord = true; IsInCorrectPosition = true}; true |]
//        ]
    
    let GivenGameWithSolution(solution: string) =
        Wordle(solution)
        
    let WhenGuess(guess: string) (game: Wordle) =
        game.Guess(guess)
        
    let ExpectResult(expectedStates: LetterState list) (actualResult: LetterAnswer list) =
        let actualStates = List.map (fun answer -> answer.State) actualResult
        test <@ actualStates = expectedStates @>

    [<Fact>]
    member __.AllHits2() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("MUSIC")
            |> ExpectResult([Hit; Hit; Hit; Hit; Hit])
                                    
    [<Fact>]
    member __.AllMisses() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("TEXAN")
            |> ExpectResult([Miss; Miss; Miss; Miss; Miss])
                
    [<Fact>]
    member __.OneNearMiss() =
        GivenGameWithSolution("MUSIC")
            |> WhenGuess("TEXAS")
            |> ExpectResult([Miss; Miss; Miss; Miss; NearMiss])
                