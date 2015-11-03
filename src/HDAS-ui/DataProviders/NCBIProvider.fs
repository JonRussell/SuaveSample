module HDASUI.NCBIProvider

open FSharp.Data
open System.Xml.Linq


let BaseUrl = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi"
let Query (term) = "?db=pubmed&term=" + term

type NCBITypeProvider = XmlProvider<"Schemas/NCBI.xml">

let simpleSearchRequest term = Http.RequestString( BaseUrl + Query (term), httpMethod = "GET",
                                   headers = [ "Accept", "application/json" ])
 
let search = simpleSearchRequest >> NCBITypeProvider.Parse

let ncbiCount term = search(term).Count 
