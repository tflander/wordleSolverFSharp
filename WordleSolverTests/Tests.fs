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
    
    let ExpectResultForGuess(guess: string, expectedStates: LetterState list) =
        let chars = guess.ToCharArray()
        chars
            |> Array.mapi (fun i c -> {Letter = c; State = expectedStates.[i]})
            |> Array.toList
                
    [<Fact>]
    member __.AllHits() =
        let game = Wordle("MUSIC")        
        test <@ game.Guess("MUSIC") = ExpectResultForGuess("MUSIC", [Hit; Hit; Hit; Hit; Hit]) @>
                
    [<Fact>]
    member __.AllMisses() =
        let game = Wordle("MUSIC")
        test <@ game.Guess("TEXAN") = ExpectResultForGuess("TEXAN", [Miss; Miss; Miss; Miss; Miss]) @>
                
    [<Fact>]
    member __.OneNearMiss() =
        let game = Wordle("MUSIC")
        test <@ game.Guess("TEXAS") = ExpectResultForGuess("TEXAS", [Miss; Miss; Miss; Miss; NearMiss]) @>
                