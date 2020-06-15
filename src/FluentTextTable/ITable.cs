﻿using System.Collections.Generic;
using System.IO;

namespace FluentTextTable
{
    public interface ITable<in TItem>
    {
        int Padding { get; }
        IReadOnlyList<IColumn<TItem>> Columns { get; }
        string ToString(IEnumerable<TItem> items);
        void Write(TextWriter writer, IEnumerable<TItem> items);
    }
}