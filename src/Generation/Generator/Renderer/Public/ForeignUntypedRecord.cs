﻿using System;
using System.Linq;
using Generator.Model;

namespace Generator.Renderer.Public;

internal static class ForeignUntypedRecord
{
    public static string Render(GirModel.Record record)
    {
        var name = Model.ForeignUntypedRecord.GetPublicClassName(record);
        var internalHandleName = Model.ForeignUntypedRecord.GetFullyQuallifiedOwnedHandle(record);

        return $@"
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace {Namespace.GetPublicName(record.Namespace)};

// AUTOGENERATED FILE - DO NOT MODIFY

{PlatformSupportAttribute.Render(record as GirModel.PlatformDependent)}
public partial class {name}
{{
    public {internalHandleName} Handle {{ get; }}

    public {name}({internalHandleName} handle)
    {{
        Handle = handle;
    }}
}}";
    }
}