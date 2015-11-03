#I "../../packages/FSharp.Data/lib/net40"
#r "../../packages/Suave/lib/net40/Suave.dll"
#r "../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"

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



let defaultConfig = { defaultConfig with
                                    bindings = [ HttpBinding.mk' HTTP "0.0.0.0" 8083 ]
                                    homeFolder = Some (__SOURCE_DIRECTORY__ + "/web")}




let EvidenceSearchResponse term : WebPart =
  fun (ctx : HttpContext) ->
    async{
      return! OK ("tem is : " + term) ctx
      }


let hdasApp =
  choose [
    GET >>= choose
      [ path "/" >>= OK "Home"
        pathScan "/evidencesearch/%s" (fun (term) -> EvidenceSearchResponse(term))
      ]
    ]
