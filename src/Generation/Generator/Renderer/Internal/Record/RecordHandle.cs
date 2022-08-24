﻿using Generator.Model;

namespace Generator.Renderer.Internal;

internal static class RecordHandle
{
    public static string Render(GirModel.Record record)
    {
        var internalHandleName = Record.GetInternalHandleName(record);
        var nullHandleName = Record.GetInternalNullHandleName(record);
        var unownedHandleName = Record.GetInternalUnownedHandleName(record);
        var ns = Namespace.GetInternalName(record.Namespace);

        return $@"
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace {ns}
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    public abstract class {internalHandleName} : SafeHandle
    {{
        protected {internalHandleName}(bool ownsHandle) : base(IntPtr.Zero, ownsHandle) {{ }}

        public sealed override bool IsInvalid => handle == IntPtr.Zero;
    }}

    {PlatformSupportAttribute.Render(record as GirModel.PlatformDependent)}
    public class {nullHandleName} : {internalHandleName}
    {{
        public static {nullHandleName} Instance = new {nullHandleName}();
    
        private {nullHandleName}() : base(true) {{ }}

        protected override bool ReleaseHandle()
        {{
            throw new System.Exception(""It is not allowed to free a \""{ns}.{nullHandleName}\""."");
        }}
    }}

     {PlatformSupportAttribute.Render(record as GirModel.PlatformDependent)}
    public partial class {unownedHandleName} : {internalHandleName}
    {{
        private {unownedHandleName}() : base(false) {{ }}

        public {unownedHandleName}(IntPtr handle) : base(false)
        {{
            SetHandle(handle);
        }}    

        protected override bool ReleaseHandle()
        {{
            throw new System.Exception(""It is not allowed to free a \""{ns}.{unownedHandleName}\""."");
        }}
    }}
}}";
    }
}
