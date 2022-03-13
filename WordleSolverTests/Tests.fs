module Tests

open System
open Xunit
open Program
open Swensen.Unquote
    
type ``IsLetterStateValid tests`` () = 
//    static member IsLetterStateValidTestData 
//        with get() : obj[] list = 
//        [
//            [| {IsInWord = false; IsInCorrectPosition = false}; true |]
//            [| {IsInWord = false; IsInCorrectPosition = true}; false |]
//            [| {IsInWord = true; IsInCorrectPosition = false}; true |]
//            [| {IsInWord = true; IsInCorrectPosition = true}; true |]
//        ]
                
    [<Fact>]
    member __.AllHits() =
        let game = Wordle("MUSIC")
        let m = {Letter = 'M'; State = Hit}
        let u = {Letter = 'U'; State = Hit}
        let s = {Letter = 'S'; State = Hit}
        let i = {Letter = 'I'; State = Hit}
        let c = {Letter = 'C'; State = Hit}
        test <@ game.Guess("MUSIC") = [m; u; s; i; c] @>
                
    [<Fact>]
    member __.AllMisses() =
        let game = Wordle("MUSIC")
        let t = {Letter = 'T'; State = Miss}
        let e = {Letter = 'E'; State = Miss}
        let x = {Letter = 'X'; State = Miss}
        let a = {Letter = 'A'; State = Miss}
        let n = {Letter = 'N'; State = Miss}
        test <@ game.Guess("TEXAN") = [t; e; x; a; n] @>
                
    [<Fact>]
    member __.OneNearMiss() =
        let game = Wordle("MUSIC")
        let t = {Letter = 'T'; State = Miss}
        let e = {Letter = 'E'; State = Miss}
        let x = {Letter = 'X'; State = Miss}
        let a = {Letter = 'A'; State = Miss}
        let s = {Letter = 'S'; State = NearMiss}
        test <@ game.Guess("TEXAS") = [t; e; x; a; s] @>
                