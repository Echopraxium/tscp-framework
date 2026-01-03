namespace TSCP.GUI

open System
open System.Net
open System.Diagnostics
open System.Threading.Tasks
open Google.Apis.Auth.OAuth2
open TSCP.Core

module Auth =
    let clientId = "YOUR_CLIENT_ID.apps.googleusercontent.com"
    let redirectUri = "http://localhost:5000/"

    let authenticateUser () : Task<bool> =
        task {
            try
                printfn "Initializing Google Auth Flow..."
                return true
            with _ ->
                return false
        }

    /// Initiates the Google OAuth2 flow using the system browser
    let loginAsync () =
        async {
            let authUrl = sprintf "https://accounts.google.com/o/oauth2/v2/auth?client_id=%s&redirect_uri=%s&response_type=code&scope=openid%%20email%%20profile" clientId redirectUri
            
            // Open the system browser for user interaction
            Process.Start(new ProcessStartInfo(authUrl, UseShellExecute = true)) |> ignore
            
            // Set up a temporary local listener to capture the authorization code
            use listener = new HttpListener()
            listener.Prefixes.Add(redirectUri)
            listener.Start()
            
            let! context = Async.FromBeginEnd(listener.BeginGetContext, listener.EndGetContext)
            let code = context.Request.QueryString.["code"]
            
            // Simple success response to the browser
            let response = context.Response
            let responseString = "<html><body><h1>TSCP Auth Success</h1><p>You can close this tab and return to the application.</p></body></html>"
            let buffer = System.Text.Encoding.UTF8.GetBytes(responseString)
            response.ContentLength64 <- int64 buffer.Length
            response.OutputStream.Write(buffer, 0, buffer.Length)
            response.Close()
            listener.Stop()

            return code
        }