﻿namespace Repository.Model
{
    public record Enumeration : IType
    {
        public Namespace Namespace { get; init; }
        public string ManagedName { get; set; }
        public string NativeName { get; init; }
        
        public bool HasFlags { get; init; }
    }
}
