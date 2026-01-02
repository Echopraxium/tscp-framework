namespace TSCP.Core

open System
open System.IO
open System.Text.Json

module PersistenceUtils =
    // Helper pour extraire une propriété de manière sécurisée (Lève l'ambiguïté de surcharge)
    let private tryGetProp (el: JsonElement) (name: string) =
        let mutable prop = JsonElement()
        if el.TryGetProperty(name, &prop) then Some prop else None

    let private mapObject (el: JsonElement) : TscpObject =
        let getStr name = 
            match tryGetProp el name with
            | Some p -> p.GetString()
            | None -> ""

        // Supporte @id ou Id, @type ou Type
        let id = match tryGetProp el "@id" with | Some p -> p.GetString() | _ -> getStr "Id"
        let t  = match tryGetProp el "@type" with | Some p -> p.GetString() | _ -> getStr "Type"

        let facettes = 
            match tryGetProp el "Facettes" with
            | Some p -> [ for item in p.EnumerateArray() do yield item.GetString() ]
            | _ -> []

        let relations = 
            match tryGetProp el "Relations" with
            | Some p -> [ for pr in p.EnumerateObject() do yield pr.Name, pr.Value.GetString() ] |> Map.ofList
            | _ -> Map.empty

        { Id = id; Type = t; Layer = getStr "Layer"; Label = getStr "Label"; 
          Facettes = facettes; Relations = relations }

    let load (path: string) : Result<FrameworkModel, string> =
        try
            let json = File.ReadAllText(Path.GetFullPath(path))
            use doc = JsonDocument.Parse(json)
            let root = doc.RootElement
            
            // On cherche la liste "Objects"
            let objects = 
                match tryGetProp root "Objects" with
                | Some p -> [ for item in p.EnumerateArray() do yield mapObject item ]
                | None -> []

            // On extrait le contexte pour rester conforme au Domain.fs
            let context = 
                match tryGetProp root "@context" with
                | Some c -> [ for p in c.EnumerateObject() do yield p.Name, p.Value.ToString() ] |> Map.ofList
                | None -> Map.empty

            let version = match tryGetProp root "Version" with | Some v -> v.GetString() | _ -> "1.6.2"
            let modelType = match tryGetProp root "@type" with | Some t -> t.GetString() | _ -> "tscp:FrameworkModel"

            if objects.Length = 0 then 
                Error "Aucun objet trouve. Verifiez la cle 'Objects' dans le JSON."
            else 
                // CORRECTION : On remplit TOUS les champs definis dans Domain.fs
                Ok { 
                    Context = context
                    Type = modelType
                    Version = version
                    Objects = objects 
                }
        with ex -> Error (sprintf "Erreur : %s" ex.Message)