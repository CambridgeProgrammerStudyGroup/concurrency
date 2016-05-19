module ``Day 1 of Functional Parallelism``

open System

(* Dictionaries in FSharp *)
let counts = 
    Map [ ("apple", 2)
          ("orange", 1) ]

(* Dictionaries in FSharp other way *)
let countsOther = [ ("apple", 2); ("orange", 1) ] |> Map.ofList

(* getting values from a dictionary *)
counts |> Map.tryFind "apple"
counts |> Map.tryFind "banana"

(* adding values to a dictionary *)
counts |> Map.add "banana" 1

(* creating an equivilant frequencies function in fsharp *)
let frequencies = Seq.countBy id

let getWords (text : string) =
    text.Split([|' ';'.'|], StringSplitOptions.RemoveEmptyEntries) |> Seq.ofArray

(* creating an equivilant mapCat function in fsharp *)
let mapCat f = (Seq.map f) >> (Seq.collect id)

["Hello World"; "Hello World"; "Hello"; "World" ]
 |> mapCat getWords

let countWordsSequential pages =
    pages |> mapCat getWords |> frequencies 
#time
["Hello World"; "Hello World"; "Hello"; "World" ]
    |> countWordsSequential

(* Sequences in FSharp with partial application of multiply *)
seq {0..10000000} |> Seq.take 10 |> Seq.map ((*) 2)

(* Infinite Sequences with partial application *)
Seq.initInfinite ((+) 1)
