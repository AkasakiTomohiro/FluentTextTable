﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using EastAsianWidthDotNet;

namespace FluentTextTable
{
    public class ColumnConfig : IColumnConfig
    {
        internal string Header { get; private set; }
        internal int HeaderWidth { get; private set; }
        internal HorizontalAlignment HorizontalAlignment { get; private set; } = HorizontalAlignment.Default;
        internal VerticalAlignment VerticalAlignment { get; private set; } = VerticalAlignment.Top;
        internal string Format { get; private set; }
        internal int Width { get; private set; }

        internal ColumnConfig(string header)
        {
            HeaderIs(header);
        }

        public IColumnConfig HeaderIs(string header)
        {
            Header = header;
            HeaderWidth = header.GetWidth() + 2;
            return this;
        }

        public IColumnConfig AlignHorizontalTo(HorizontalAlignment horizontalAlignment)
        {
            HorizontalAlignment = horizontalAlignment;
            return this;
        }

        public IColumnConfig AlignVerticalTo(VerticalAlignment verticalAlignment)
        {
            VerticalAlignment = verticalAlignment;
            return this;
        }

        public IColumnConfig FormatTo(string format)
        {
            Format = format;
            return this;
        }
        internal void UpdateWidth(IEnumerable<Row> rows)
        {
            Width = Math.Max(HeaderWidth, rows.Select(x => x.GetCell(this).Width).Max());
        }

        internal void WriteHeader(TextWriter writer)
        {
            writer.Write(" ");
            writer.Write(Header);
            writer.Write(new string(' ', Width - HeaderWidth));
        }
    }
}