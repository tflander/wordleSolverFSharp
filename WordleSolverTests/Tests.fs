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
    
    let sampleWords = [| "MUSIC"; "TEXAN"; "TEXAS"; "GUILD"; "RANKS"; "MARES"|]

    [<Fact>]
    member __.``No Letter Matches``() =
        let game = GivenGameWithSolution("MUSIC")
        let response = game.Guess("TEXAN")
        let updatedWordList = FilterWords response sampleWords
        test <@ updatedWordList = [| "MUSIC"; "GUILD" |] @>
        
    [<Fact>]
    member __.``One Hit``() =
        let game = GivenGameWithSolution("MUSIC")
        let response = game.Guess("MERRY")
        let updatedWordList = FilterWords response sampleWords
        test <@ updatedWordList = [| "MUSIC" |] @>
        
    [<Fact(Skip = "This is spike code to delete")>]
    member __.``Spike``() =
        let game = GivenGameWithSolution("MUSIC")
        let response = game.Guess("TEXAN")
        let updatedWordList = FilterWords response fiveLetterWords // 2768 words
        
        let secondResponse = game.Guess "BYPRO"
        let secondUpdate = FilterWords secondResponse updatedWordList // 388 words
        test <@ 1 = 2 @>
                                