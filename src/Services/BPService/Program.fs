open Suave
open Suave.Successful
open Suave.Operators
open Suave.Filters
open Givestack.HealthCheck.BPService.Workflow
open System.Net
open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Givestack.HealthCheck.BPService.Domain

type CmdArgs = { IP: System.Net.IPAddress; Port: Sockets.Port }

let fromJSON json = 
    JsonConvert.DeserializeObject(json, typeof<BloodPressureStat>) :?> 'a

let JSON v =
    let jsonSerializerSettings = new JsonSerializerSettings()
    jsonSerializerSettings.ContractResolver <- new CamelCasePropertyNamesContractResolver()

    JsonConvert.SerializeObject(v, jsonSerializerSettings)
    |> OK
    >=> Writers.setMimeType "application/json; charset=utf-8"

let getResourceFromReq(req : HttpRequest) =
        let getString rawForm =
            System.Text.Encoding.UTF8.GetString(rawForm)
        req.rawForm |> getString |> fromJSON

let getAll = 
    warbler (fun _ -> Workflow.getAllBPStatuses |> JSON)

let add =
    request (getResourceFromReq >> Workflow.createStat >> JSON)
    
let app : WebPart = 
    choose [ POST >=> 
                choose [ path "/bp" >=> (add)]
             GET >=> 
                choose [ path "/bp" >=> (getAll)] ]

[<EntryPoint>]
let main argv =
    let args =
        let parse f str = match f str with (true, i) -> Some i | _ -> None

        let (|Port|_|) = parse System.UInt16.TryParse
        let (|IPAddress|_|) = parse System.Net.IPAddress.TryParse

        //default bind to 127.0.0.1:8083
        let defaultArgs = { IP = System.Net.IPAddress.Loopback; Port = 8080us }

        let rec parseArgs b args =
            match args with
            | [] -> b
            | "--ip" :: IPAddress ip :: xs -> parseArgs { b with IP = ip } xs
            | "--port" :: Port p :: xs -> parseArgs { b with Port = p } xs
            | invalidArgs ->
                printfn "error: invalid arguments %A" invalidArgs
                printfn "Usage:"
                printfn "    --ip ADDRESS   ip address (Default: %O)" defaultArgs.IP
                printfn "    --port PORT    port (Default: %i)" defaultArgs.Port
                exit 1

        argv |> List.ofArray |> parseArgs defaultArgs

    let log x = printfn "%A" x; x

    startWebServer { defaultConfig with bindings = [ HttpBinding.create HTTP args.IP args.Port ]} app
    0 // return an integer exit code
