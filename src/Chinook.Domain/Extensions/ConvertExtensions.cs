﻿using Chinook.Domain.Converters;

namespace Chinook.Domain.Extensions;

public static class ConvertExtensions
{
    public static List<TTarget> ConvertAll<TTarget>(
        this IEnumerable<IConvertModel<TTarget>> values)
        => values.Select(value => value.Convert()).ToList();
}