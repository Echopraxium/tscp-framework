namespace TSCP.Core

// --- DÉFINITION DE L'ONTOLOGIE ---
[<CLIMutable>]
type TscpObject = {
    Id: string
    Type: string
    Layer: string
    Label: string
    Facettes: string list
    Relations: Map<string, string>
}

[<CLIMutable>]
type FrameworkModel = {
    Context: Map<string, string>
    Type: string
    Version: string
    Objects: TscpObject list
}