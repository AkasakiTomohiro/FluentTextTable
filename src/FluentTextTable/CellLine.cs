﻿using System;
using System.IO;
using EastAsianWidthDotNet;

namespace FluentTextTable
{
    internal class CellLine
    {
        internal static readonly CellLine Empty = new CellLine(string.Empty, null);
        private const int Margin = 2;

        private readonly string _value;

        internal int Width { get; }

        public CellLine(object value, string format)
        {
            _value = format is null
                ? value.ToString()
                : string.Format(format, value);
            Width = _value.GetWidth() + Margin;
        }

        internal void Write(
            TextWriter writer,
            Row row,
            Column column)
        {
            writer.Write(" ");

            int leftPadding;
            int rightPadding;
            switch (column.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    leftPadding = 0;
                    rightPadding = column.Width - Width;
                    break;
                case HorizontalAlignment.Center:
                    leftPadding = 0;
                    rightPadding = column.Width - Width;
                    break;
                case HorizontalAlignment.Right:
                    leftPadding = column.Width - Width;
                    rightPadding = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            writer.Write(new string(' ', leftPadding));
            writer.Write(_value);
            writer.Write(new string(' ', rightPadding));
            writer.Write(" |");
        }
    }
}