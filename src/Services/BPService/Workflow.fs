namespace Givestack.HealthCheck.BPService.Workflow

open Suave
open Suave.Operators
open Newtonsoft.Json
open Givestack.HealthCheck.BPService.Database
open Givestack.HealthCheck.BPService.Domain
open Newtonsoft.Json.Serialization
open Suave.Successful

module Workflow = 

    let getAllBPStatuses =
        let results = Database.getStats
        results |> Seq.toList
   
    let createStat (stat:BloodPressureStat) =
        Database.createStat stat 
        sprintf "%s added blood pressure stat" stat.UserId