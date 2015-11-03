#I "../../../packages/FSharp.Data/lib/net40"
#r "../../../packages/Suave/lib/net40/Suave.dll"
#r "../../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data

//type Simple = JsonProvider<""" { "name":"John", "age":94 } """>
//type Simple = JsonProvider<"../../../json/test.json">
//let simple = Simple.Parse(""" { "name":"Tomas", "age":4, "address":"cave" } """)

//simple.Age
//simple.Name

//type Numbers = JsonProvider<""" [1, 2, 3, 3.14] """>
//let nums = Numbers.Parse(""" [1.2, 45.1, 98.2, 5] """)
//let total = nums |> Seq.sum

//type Mixed = JsonProvider<""" [1, 2, 2.5, "hello", "world"] """>
//let mixed = Mixed.Parse(""" [4, 5, 6.5, "hello", "world" ] """)

//mixed.Numbers |> Seq.sum
//mixed.Strings |> String.concat ", "
//mixed.JsonValue

//type Monkeys = JsonProvider<"../../../json/test.json">
//
//for item in Monkeys.GetSamples() do 
//  printf "%s " item.Name 
//  item.Age |> Option.iter (printf "(%d)")
//  item.Address |> Option.iter(printf "[%s]")
//  printfn ""
//
//type Values = JsonProvider<""" [{"value":94 }, {"value":"Tomas" }] """>
//
//for item in Values.GetSamples() do 
//  match item.Value.Number, item.Value.String with
//  | Some num, _ -> printfn "Numeric: %d" num
//  | _, Some str -> printfn "Text: %s" str
//  | _ -> printfn "Some other value!"
//
//type WorldBank = JsonProvider<"../../../json/worldbank.json">
//let doc = WorldBank.GetSample()
//
//// Print general information
//let info = doc.Record
//printfn "Showing page %d of %d. Total records %d" 
//  info.Page info.Pages info.Total
//
//// Print all data points
//for record in doc.Array do
//  record.Value |> Option.iter (fun value ->
//    printfn "%d: %f" record.Date value)

type searchRecord = {
    abs : string
}
type proquestDoc = JsonProvider<"../../../json/proquest.json">
let doc = proquestDoc.GetSample()
//printfn "Total Count %d" doc.Count
//for record in doc.Items do
//  match record.Abstract with
//    | Some str -> printfn "Abstract: %s" record.Abstract.Value
//    | _ -> printfn "No abstract"


//let sample =[Some 4; None; Some 2; None] |> List.choose id



let emptyFunction (optionalAbstract:Option<string>) (emptyText:string) =
   match optionalAbstract with
     | Some optionalAbstract -> optionalAbstract
     | _ -> emptyText

let jonFunction (optionalAbstract:Option<string>) =  emptyFunction optionalAbstract "no text"

type RawInspectionData() = 
     let allData = proquestDoc.GetSample()
     member this.allAbstracts = 
             allData.Items 
                   |> Array.map(fun rcrd  -> rcrd.Abstract |> jonFunction) 
                  //|> Array.choose id
let testData = RawInspectionData().allAbstracts


let monkey = emptyFunction (Some "") "tests"




