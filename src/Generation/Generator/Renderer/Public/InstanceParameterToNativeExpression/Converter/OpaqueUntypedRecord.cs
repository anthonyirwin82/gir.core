﻿using System;

namespace Generator.Renderer.Public.InstanceParameterToNativeExpressions;

public class OpaqueUntypedRecord : InstanceParameterConverter
{
    public bool Supports(GirModel.Type type)
    {
        return type is GirModel.Record record && Model.Record.IsOpaqueUntyped(record);
    }

    public string GetExpression(GirModel.InstanceParameter instanceParameter)
    {
        return instanceParameter switch
        {
            { Transfer: GirModel.Transfer.None } => "this.Handle",
            { Transfer: GirModel.Transfer.Full } => "this.Handle.UnownedCopy()",
            _ => throw new Exception("Unknown transfer type for opaque untyped record instance parameter")
        };
    }
}
