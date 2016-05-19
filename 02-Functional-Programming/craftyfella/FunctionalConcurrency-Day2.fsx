module ``Day 2 of Functional Parallelism``

open System

let pages = 
    [| for a in 1..1000000 do
           yield "one potato two potato three potato four"
           yield "five potato six potato sevon potato more" |]

(* creating an equivilant frequencies function in fsharp *)
let frequencies a = Array.countBy id a |> Map.ofArray
let getWords (text : string) = text.Split([| ' '; '.' |], StringSplitOptions.RemoveEmptyEntries)

(* Translation Direct from page 61 *)
Array.Parallel.map (fun page -> frequencies (getWords page)) pages
(* More ideomatic *)
pages |> Array.Parallel.map (getWords >> frequencies)

(* an attempt at merge with *)
let mergeWith updater a b = 
    let addOrUpdate key value updater map = 
        match Map.tryFind key map with
        | Some v -> Map.add key (updater v value) map
        | None -> Map.add key value map
    Map.fold (fun acc key value -> addOrUpdate key value updater acc) a b

let mapA = 
    Map [ ("one", 1)
          ("potato", 3)
          ("three", 2) ]

let mapB = 
    Map [ ("two", 1)
          ("potato", 3) ]

let merged = mergeWith (+) mapB mapA

(* Understanding Map fold *)
let map1 = 
    Map.ofList [ (1, "one")
                 (2, "two")
                 (3, "three") ]

Map.foldBack (fun key value state -> 
    printfn "Key: %O State: %O Value:%O State+Key: %O" key state value (state + key)
    state + key) map1 0

let map2 = 
    Map.ofList [ (1, "one")
                 (2, "two")
                 (3, "three") ]

Map.fold (fun state key value -> 
    printfn "Key: %O State: %O Value:%O State+Key: %O" key state value (state + key)
    state + key) 0 map2
(* Understanding reduce *)
Array.reduce (fun a b -> 
    printfn "A: %O B: %O A+B:%O" a b (a + b)
    a + b) [| 0..10 |]
(******** putting it all together **********)
(* Reduce over the Array of maps and merge each one together *)
Array.reduce (mergeWith (+)) (Array.Parallel.map (fun page -> frequencies (getWords page)) pages)
(* More FSharpy *)
pages
|> Array.Parallel.map (getWords >> frequencies)
|> Array.reduce (mergeWith (+))

(* TODO: Mark and Jons take on the above *)

(* Batching word counting *)
let countWordsSequential pages = 
    let mapCat f = (Array.map f) >> (Array.collect id)
    pages
    |> mapCat getWords
    |> frequencies

#time 

pages
|> Array.chunkBySize 100
|> Array.Parallel.map countWordsSequential
|> Array.reduce (mergeWith (+))
