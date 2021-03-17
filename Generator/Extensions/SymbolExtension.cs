﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.Model;

namespace Generator
{
    internal static class SymbolExtension
    {
        public static bool IsForeignTo(this Symbol symbol, Namespace ns)
            => symbol.Namespace is not null && ns != symbol.Namespace;

        internal static string Write(this Symbol symbol, Target target,  Namespace currentNamespace)
        {
            var name = GetName(symbol, target);

            if (!symbol.IsForeignTo(currentNamespace))
                return name;

            if (symbol.Namespace is null)
                throw new Exception($"Can not write {nameof(Symbol)}, because namespace is missing");

            return symbol.Namespace.Name + "." + name;
        }
        
        private static string GetName(Symbol symbol, Target target) => target switch
        {
            Target.Managed => symbol.ManagedName,
            Target.Native => symbol.NativeName,
            _ => throw new Exception($"Unknown {nameof(Target)}")
        };
    }
}
