﻿namespace Controllers

open System
open System.Web.Http

type UpdateController<'T>(f:'T->'T) =
    inherit ApiController()

    member this.Get ([<FromUri>]x) = f x // Should be fixed soon https://github.com/domaindrivendev/Swashbuckle/pull/547
    member this.Post x = f x

type UpdateBoolController () =
    inherit UpdateController<bool>(not)

type UpdateInt32Controller () =
    inherit UpdateController<int>((+)1)

type UpdateInt64Controller () =
    inherit UpdateController<int64>((+)1L)

type UpdateFloatController () =
    inherit UpdateController<float32>((+)1.0f)

type UpdateDoubleController () =
    inherit UpdateController<float>((+)1.0)

type UpdateStringController () =
    inherit UpdateController<string>((+)"Hello, ")

type UpdateDateTimeController () =
    inherit UpdateController<DateTime>
        (fun x -> x.AddDays(1.0))

type UpdateEnumController () =
    inherit UpdateController<UriKind>(id)

type UpdateArrayIntController () =
    inherit UpdateController<int []>(Array.rev)

type UpdateArrayEnumController () =
    inherit UpdateController<UriKind []>(Array.rev)

type UpdateListIntController () =
    inherit UpdateController<int list>(List.rev)

type UpdateSeqIntController () =
    inherit UpdateController<int seq>(Seq.rev)

type UpdateObjectPointClassController () =
    inherit UpdateController<Types.PointClass>
        (fun p -> Types.PointClass(p.Y, p.X))

