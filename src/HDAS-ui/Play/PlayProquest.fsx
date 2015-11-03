#I "../../../packages/FSharp.Data/lib/net40"
#r "../../../packages/Suave/lib/net40/Suave.dll"
#r "../../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data

type searchRecord = {
    Abstract : string;
    Title : string
}
type proquestProvider = JsonProvider<"../../../json/proquest2.json">
//type proquestDoc = JsonProvider<"http://localhost:8082/proquestsearch/cancer">


//let doc = proquestDoc.GetSample()

let simpleSearchRequest (term:string) = Http.RequestString( "http://localhost:8082/proquestsearch/" + term, httpMethod = "GET",
                                               headers = ["Accept", "application/json"])



type RawInspectionData = 
    static member fromProvider (x:proquestProvider.Root) = 
                  x.Members |> Array.map(fun rcrd  -> { Abstract = rcrd.FabioAbstract; Title = rcrd.DcTitle.ToString() }) 


let testData  =  simpleSearchRequest >> proquestProvider.Parse >> RawInspectionData.fromProvider

testData "pain"