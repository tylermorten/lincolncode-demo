create table if not exists "BloodPressureStat"( 
    "Id" integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    "UserId" nvarchar(128) NOT NULL,
    "Systolic" integer,
    "Diastolic" integer,
    "HeartRate" integer,
    "TakenOn" datetime);