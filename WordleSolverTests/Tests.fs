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
                
    [<Fact>]
    member __.CreateGame() =
        let game = Wordle("MUSIC")
        let m = {Letter = 'M'; State = Hit}
        let u = {Letter = 'U'; State = Hit}
        let s = {Letter = 'S'; State = Hit}
        let i = {Letter = 'I'; State = Hit}
        let c = {Letter = 'C'; State = Hit}
        test <@ game.Guess("MUSIC") = [m; u; s; i; c] @>        