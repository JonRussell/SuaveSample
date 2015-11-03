#I "../../../packages/FSharp.Data/lib/net40"
#I "../../../packages/FSharp.Configuration/lib/net40"
#r "../../../packages/Suave/lib/net40/Suave.dll"
#r "../../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "../../../packages/FSharp.Configuration/lib/net40/FSharp.Configuration.dll"
#r "System"
#r "System.Configuration"
#r "System.Xml.Linq.dll"

open Suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Types
open FSharp.Data
open Suave.Web
open Suave.Types
open FSharp.Configuration
open System
open System.Configuration

//Set pwd to the directory containing this script
System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__


#load "../Types.fs"
#load "../Configuration/Authentication.fs"


open HDAS.Authentication

printfn "ovid auth is %s " ovidAuth.Authentication


let BaseUrl = "https://ovidsp.tx.ovid.com/ovidws/"


type OvidRootTypeProvider = XmlProvider<"../../HDAS/Schemas/Ovid/RootResource.xml", Global=true>



let simpleSearchRequest () = Http.RequestString( BaseUrl, httpMethod = "GET",
                                               headers = ["Accept", "text/html";
                                                          "Authorization", ovidAuth.Authentication])



let search = simpleSearchRequest >> OvidRootTypeProvider.Parse

let lis = search().Body.Ul.Lis


let optionalItems = lis |> Array.map (fun x -> x.A) |> Array.choose id


