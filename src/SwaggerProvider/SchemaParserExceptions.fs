﻿namespace SwaggerProvider.Internal.Schema.Parsers

open FSharp.Data
open System

type SwaggerSchemaParseException(message) =
    inherit Exception(message)

/// Schema object does not contain the `field` that is Required in Swagger specification.
type FieldNotFoundException(obj:JsonValue, field:string, specLink:string) =
    inherit SwaggerSchemaParseException(
        sprintf "Object MUST contain field `%s` (See %s for more details).\nObject:%A"
            field specLink obj)

/// The `field` value is not specified in Swagger specification
type UnknownFieldValueException(obj:JsonValue, value:string, field:string, specLink:string) =
    inherit SwaggerSchemaParseException(
        sprintf "Value `%s` is not allowed for field `%s`(See %s for more details).\nObject:%A"
            value field specLink obj)

/// Unsupported Swagger version
type UnsupportedSwaggerVersionException(version) =
    inherit SwaggerSchemaParseException(
        sprintf "SwaggerProviders does not Swagger Specification %s"
            version)

/// Unknown reference
type UnknownSwaggerReferenceException(ref:string) =
    inherit SwaggerSchemaParseException(
        sprintf "SwaggerProvider could not resolve `$ref`: %s" ref)