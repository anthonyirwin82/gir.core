﻿using System;
using System.Linq;
using Generator.Model;

namespace Generator.Renderer.Public;

internal static class FundamentalClassMethods
{
    public static string Render(GirModel.Class cls)
    {
        return $@"
using System;
using GObject;
using System.Runtime.InteropServices;

#nullable enable

namespace {Namespace.GetPublicName(cls.Namespace)}
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    public partial class {cls.Name}
    {{
        {cls.Methods
            .Where(Method.IsEnabled)
            .Where(method => !method.IsFree())//Freeing is handled by the framework via a IDisposable implementation.
            .Select(x => MethodRenderer.Render(cls, x))
            .Join(Environment.NewLine)}
    }}
}}";
    }
}
