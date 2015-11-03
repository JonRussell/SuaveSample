
module HDAS.Program
open System
open System.Net
open Suave
open Suave.Sockets
open Suave.Sockets.Control
open Suave.Logging
open Suave.Web
open Suave.Http
open Suave.Http.EventSource
open Suave.Http.Applicatives
open Suave.Http.Writers
open Suave.Http.Files
open Suave.Http.Successful
open Suave.Types
open Suave.State.CookieStateStore
open Suave.Utils
open Suave.DotLiquid
open HDASUI.EvidenceProvider
open HDASUI.NCBIProvider
open FSharp.Data
open HDASUI.Authentication
open HDASUI.Types

let logger = Loggers.ConsoleWindowLogger LogLevel.Verbose

System.Net.ServicePointManager.DefaultConnectionLimit <- Int32.MaxValue

type SearchModel = {  
  filters: string list
  title : string
}

type searchRecord = {
    Abstract : string;
    Title : string
}

type proquestResultsModel = {
    searchTerm: string;
    searchResults: searchRecord list
}

type proquestProvider = JsonProvider<"../../json/proquest2.json">
type ebscoProvider = JsonProvider<"../../json/ebsco.json">
type ovidProvider = JsonProvider<"../../json/ovid.json">

let proquestConn = sprintf "http://%s/%s/" hdasServiceIP proquestSearchUri
let ebscoConn = sprintf "http://%s/%s/" hdasServiceIP ebscoSearchUri
let ovidConn = sprintf "http://%s/%s/" hdasServiceIP ovidSearchUri

let simpleSearchRequest conn (term:string) = Http.RequestString( conn + term, httpMethod = "GET",
                                               headers = ["Accept", "application/json"])

let handleOption (x:string option) = if x.IsSome then x.Value else String.Empty

type RawProquestData = 
    
    static member fromProvider (x:proquestProvider.Root) = 
                  x.Members |> Array.map(fun rcrd  -> { Abstract = handleOption rcrd.FabioAbstract; Title = handleOption rcrd.DcTitle }) 

type RawEbscoData = 
    static member fromProvider (x:ebscoProvider.Root) = 
                  x.Members |> Array.map(fun rcrd  -> { Abstract = handleOption rcrd.FabioAbstract
                                                        Title = handleOption rcrd.DcTitle }) 

type RawOvidData = 
    static member fromProvider (x:ovidProvider.Root) = 
                  x.Members |> Array.map(fun rcrd  -> { Abstract = handleOption rcrd.FabioAbstract
                                                        Title = handleOption rcrd.DcTitle }) 

let returnProquestData  =  simpleSearchRequest proquestConn >> proquestProvider.Parse >> RawProquestData.fromProvider
let returnEbscoData  =  simpleSearchRequest ebscoConn >> ebscoProvider.Parse >> RawEbscoData.fromProvider
let returnOvidData  =  simpleSearchRequest ovidConn >> ovidProvider.Parse >> RawOvidData.fromProvider

let emptyModel = []

let EvidenceSearchResponse term : WebPart =
  fun (ctx : HttpContext) ->
    async{
      return! OK (abstracts(term) |> String.concat "<br/>") ctx
      }

let HandleFormPostData (data:HttpRequest) name =
    (tryGetChoice1 data.formData name).Value    

let HandleProviderData (data:HttpRequest) elem provider=
    let term = (tryGetChoice1 data.formData elem).Value
    
    let results = match provider with
        | Ebsco -> returnEbscoData term |> Array.toList
        | Ovid -> returnOvidData term |> Array.toList
        | _ -> returnProquestData term |> Array.toList

    {searchTerm = term; searchResults = results}   

let hdasApp =
  choose [
    GET >>= choose
      [ path "/" >>= OK "Home"               
        path "/proquest" >>= DotLiquid.page "proquest.html" emptyModel
        path "/ebsco" >>= DotLiquid.page "ebsco.html" emptyModel
        path "/ovid" >>= DotLiquid.page "ovid.html" emptyModel
        pathScan "/evidencesearch/%s" (fun (term) -> EvidenceSearchResponse(term))        
      ]
    POST >>= choose
      [        
        path "/test2" >>= request (fun x -> OK (sprintf "POST data: %s" (HandleFormPostData x "text1")))
        path "/proquest" >>= request (fun x -> HandleProviderData x "searchtext" Proquest |> DotLiquid.page "proquestresults.html")
        path "/ebsco" >>= request (fun x -> HandleProviderData x "searchtext" Ebsco |> DotLiquid.page "ebscoresults.html")
        path "/ovid" >>= request (fun x -> HandleProviderData x "searchtext" Ovid |> DotLiquid.page "ovidresults.html")
      ]
    ]


[<EntryPoint>]
let main argv =

  startWebServer
    { bindings              = [ HttpBinding.mk' HTTP "0.0.0.0" 8085
                                //HttpBinding.mk' (HTTPS (Provider.open_ssl cert)) "127.0.0.1" 8443
                              ]
      serverKey             = Utils.Crypto.generateKey HttpRuntime.ServerKeyLength
      errorHandler          = defaultErrorHandler
      listenTimeout         = TimeSpan.FromMilliseconds 2000.
      cancellationToken     = Async.DefaultCancellationToken
      bufferSize            = 2048
      maxOps                = 100
      mimeTypesMap          = Writers.defaultMimeTypesMap
      homeFolder            = None
      compressedFilesFolder = None
      logger                = logger }
   hdasApp 
  0

