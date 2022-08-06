﻿using System.Collections.Generic;
using System.Linq;

namespace Generator.Renderer.Public;

internal static class Parameters
{
    public static string Render(IEnumerable<GirModel.Parameter> parameters)
    {
        return parameters
            .Select(RenderableParameterFactory.Create)
            .Select(Render)
            .Join(", ");
    }

    private static string Render(RenderableParameter parameter)
        => $@"{parameter.Direction}{parameter.NullableTypeName} {parameter.Name}";
}