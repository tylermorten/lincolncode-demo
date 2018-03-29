module Givestack.HealthCheck.BPService.Database

open Givestack.HealthCheck.BPService.Domain
open System.IO
open Microsoft.Data.Sqlite
open NPoco
open System


module Database =     
    let private connString = "Filename=" + Path.Combine(Directory.GetCurrentDirectory(), "bpservice.db")
    
    let getStats = 
        use conn = new SqliteConnection(connString)
        conn.Open()

        let query = "select * from BloodPressureStat"
        
        use db = new Database(conn)
        db.Fetch<BloodPressureStat>(query)
        
    let createStat (stat: BloodPressureStat) = 
        use conn = new SqliteConnection(connString)
        conn.Open()

        let txn: SqliteTransaction = conn.BeginTransaction()

        let cmd = conn.CreateCommand()

        printfn "%s user adding bp stat" stat.UserId

        cmd.Transaction <- txn
        cmd.CommandText <- @"
        insert into BloodPressureStat(UserId, Systolic, Diastolic, HeartRate, TakenOn) values 
        ($UserId, $Systolic, $Diastolic, $HeartRate, $TakenOn)"

        cmd.Parameters.AddWithValue("$UserId", stat.UserId) |> ignore
        cmd.Parameters.AddWithValue("$Systolic", stat.Systolic) |> ignore
        cmd.Parameters.AddWithValue("$Diastolic", stat.Diastolic) |> ignore
        cmd.Parameters.AddWithValue("$HeartRate", stat.HeartRate) |> ignore
        cmd.Parameters.AddWithValue("$TakenOn", DateTime.Now) |> ignore

        cmd.ExecuteNonQuery() |> ignore

        txn.Commit()
 


