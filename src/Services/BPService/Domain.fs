namespace Givestack.HealthCheck.BPService.Domain

open System

type BloodPressureStat() =
    member val Id = 0 with get, set
    member val UserId = "" with get, set
    member val Systolic = 0 with get, set
    member val Diastolic = 0 with get, set
    member val HeartRate = 0 with get, set
    member val TakenOn = DateTime.Now with get, set
    
type BloodPressureStatList = {
    UserId : string
    BloodPressureStat : BloodPressureStat list
    }

