﻿using Generator.Model;
using Generator.Renderer;

namespace Generator.Generator.Internal;

internal class ClassMethods : Generator<GirModel.Class>
{
    private readonly Publisher _publisher;

    public ClassMethods(Publisher publisher)
    {
        _publisher = publisher;
    }

    public void Generate(GirModel.Class obj)
    {
        if (obj.IsFundamental)
            return;

        var source = Renderer.Internal.ClassMethods.Render(obj);
        var codeUnit = new CodeUnit(
            Project: Namespace.GetCanonicalName(obj.Namespace),
            Name: $"{obj.Name}.Methods",
            Source: source,
            IsInternal: true
        );

        _publisher.Publish(codeUnit);
    }
}