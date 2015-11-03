#I "../../packages/FSharp.Data/lib/net40"
#r "../../packages/Suave/lib/net40/Suave.dll"
#r "../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "System.Xml.Linq.dll"

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




let BaseUrl = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi"
let Query (term) = "?db=pubmed&term=" + term

type NCBITypeProvider = XmlProvider<"../HDAS/Schemas/NCBI.xml">

let sample = NCBITypeProvider.GetSample()

let count = sample.``<Note>``

let simpleSearchRequest term = Http.RequestString( BaseUrl + Query (term), httpMethod = "GET",
                                   query   = [ "api-key", ApiKey],
                                   headers = [ "Accept", "application/json" ])


let search = simpleSearchRequest >> NCBITypeProvider.Parse 

let abstracts = search("pain").SearchResult.Documents |> Array.map (fun d -> d.Abstract) 

printfn "%s" (abstracts |> String.concat "<br/>")

