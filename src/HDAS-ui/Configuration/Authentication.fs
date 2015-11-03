namespace HDASUI

module Authentication = 

  open HDASUI.Types
  open FSharp.Configuration

  type Settings = AppSettings<"app.config">

  let inline (|?) (a: 'a option) b = if a.IsSome then a.Value else b

  let getEnvProfile ukey pkey =
    let p = System.Environment.GetEnvironmentVariable(pkey)
    let u = System.Environment.GetEnvironmentVariable(ukey)
    if (p = null || u = null) then None
    else Some {username = u; password = p}


  let getEnvBasic key =
      let x = System.Environment.GetEnvironmentVariable(key)
      if x = null then None else Some x
    
  let ovidAuth = {
    Authentication = "Basic b3ZpZGNvbm5lY3Q6cHJlbTFlcg=="
  }

  //let ebscoAuth = (getEnvProfile "EBSCO_USERNAME" "EBSCO_PASSWORD") |? {username = Settings.EBSCO_USERNAME; password = Settings.EBSCO_PASSWORD}

  let hdasServiceIP = getEnvBasic("HDAS_SERVICE_IP") |? Settings.HDAS_SERVICE_IP

  let proquestSearchUri = getEnvBasic("PROQUEST_SEARCH_URI") |? Settings.PROQUEST_SEARCH_URI

  let ebscoSearchUri = getEnvBasic("EBSCO_SEARCH_URI") |? Settings.EBSCO_SEARCH_URI

  let ovidSearchUri = getEnvBasic("OVID_SEARCH_URI") |? Settings.OVID_SEARCH_URI