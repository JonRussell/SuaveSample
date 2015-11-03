namespace HDASUI

module Types = 
    type OvidAuth = {
        Authentication : string
        }

    type proquestConnection = {
        Server : string
        }

    type Credentials = {
        username: string; 
        password: string
        }

    type SearchProvider =  Proquest | Ebsco | Ovid 

    