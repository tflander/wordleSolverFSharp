module Tests

open System
open Xunit
open Program
open Swensen.Unquote
    
type ``IsLetterStateValid tests`` () = 
    static member IsLetterStateValidTestData 
        with get() : obj[] list = 
        [
            [| {IsInWord = false; IsInCorrectPosition = false}; true |]
            [| {IsInWord = false; IsInCorrectPosition = true}; false |]
            [| {IsInWord = true; IsInCorrectPosition = false}; true |]
            [| {IsInWord = true; IsInCorrectPosition = true}; true |]
        ]
        
    [<Theory>]
    [<MemberData("IsLetterStateValidTestData")>]
    member __.IsLetterStateValid (state: LetterState) (expectedIsValid: bool) =
        test <@ IsLetterStateValid(state) = expectedIsValid @>
        
    [<Fact>]
    member __.CreateGame() =
        let game = Wordle("MUSIC")
        let m = {Letter = 'M'; State = {IsInWord = true; IsInCorrectPosition = true}}
        let u = {Letter = 'U'; State = {IsInWord = true; IsInCorrectPosition = true}}
        let s = {Letter = 'S'; State = {IsInWord = true; IsInCorrectPosition = true}}
        let i = {Letter = 'I'; State = {IsInWord = true; IsInCorrectPosition = true}}
        let c = {Letter = 'C'; State = {IsInWord = true; IsInCorrectPosition = true}}
        test <@ game.Guess("MUSIC") = [m; u; s; i; c] @>        