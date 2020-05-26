﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace FluentTextTable
{
    internal class Row
    {
        private readonly Dictionary<IColumn, Cell> _cells;
        private readonly int _height;

        public Row(Dictionary<IColumn, Cell> cells, int height)
        {
            _cells = cells;
            _height = height;
        }

        internal static Row Create<TItem>(TItem item, IReadOnlyDictionary<Column, MemberAccessor<TItem>> memberAccessors)
        {
            var cells = new Dictionary<IColumn, Cell>();
            foreach (var keyValue in memberAccessors)
            {
                cells[keyValue.Key] = new Cell(keyValue.Value.GetValue(item), keyValue.Key.Format);
            }

            var height = cells.Values.Max(x => x.Height);

            return new Row(cells, height);
        }

        internal Cell GetCell(Column column) => _cells[column];

        internal void Write(TextWriter writer, IList<Column> columns)
        {
            // Write line in row.
            for (var lineNumber = 0; lineNumber < _height; lineNumber++)
            {
                writer.Write("|");
                foreach (var column in columns)
                {
                    _cells[column].Write(writer, column, lineNumber);
                }
                writer.WriteLine();
            }

        }
    }
}