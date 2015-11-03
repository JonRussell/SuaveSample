#I "../../../packages/FSharp.Data/lib/net40"
#r "../../../packages/Suave/lib/net40/Suave.dll"
#r "../../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open Suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Types
open FSharp.Data
open Suave.Web
open Suave.Types

//Set pwd to the directory containing this script
System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__



let BaseUrl = "http://alpha.api.nice.org.uk/services/search"
let Query (term) = "/results?q=" + term + "&pa=1&ps=&s="
let ApiKey = "3a0d112f-f924-47cb-b04a-b8f7c86bad6d"


type EvidenceTypeProvider = JsonProvider<"../../HDAS/Schemas/EvidenceSearch.json">


let simpleSearchRequest term = Http.RequestString( BaseUrl + Query (term), httpMethod = "GET",
                                   query   = [ "api-key", ApiKey],
                                   headers = [ "Accept", "application/json" ])



let search = simpleSearchRequest >> EvidenceTypeProvider.Parse 


let abstracts = search("pain").SearchResult.Documents |> Array.map (fun d -> d.Abstract) 

printfn "%s" (abstracts |> String.concat "<br/>")

//------ working ------

//need to combine JsonProvider and request, then pipe into function which returns
//a list of abstracts

//dont func(finc(func)) need to write funcs that can be |>
let abstracts (x:EvidenceTypeProvider) = [
    for x in x.SearchResult.Documents -> x.Abstract
  ]


//do this 

EvidenceTypeProvider.Parse(SimpleSearchRequest (term))
|> abstracts

