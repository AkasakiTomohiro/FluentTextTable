﻿using System;
using System.IO;
using Xunit;
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable NotAccessedField.Local

namespace FluentTextTable.Test
{
    public class PlainTextTest
    {
        [Fact]
        public void WhenBasic()
        {
            var table = Build.TextTable<User>(builder =>
            {
                builder
                    .AddColumn(x => x.Id).NameAs("ID").HorizontalAlignmentAs(HorizontalAlignment.Right)
                    .AddColumn(x => x.Name).NameAs("氏名")
                    .AddColumn(x => x.Birthday).FormatAs("{0:yyyy/MM/dd}");
            });
            var text = table.ToString(new[]
            {
                new User {Id = 1, Name = "ビル ゲイツ", Birthday = DateTime.Parse("1955/10/28")},
                new User {Id = 2, Name = "Steven Jobs", Birthday = DateTime.Parse("1955/2/24")}
            });

            Assert.Equal(
                @"
 +----+-------------+------------+
 | ID | 氏名        | Birthday   |
 +----+-------------+------------+
 |  1 | ビル ゲイツ | 1955/10/28 |
 +----+-------------+------------+
 |  2 | Steven Jobs | 1955/02/24 |
 +----+-------------+------------+
", $"{Environment.NewLine}{text}");
        }

        [Fact]
        public void WhenAutoFormat()
        {
            using var writer = new StringWriter();
            var table = Build.TextTable<User>();
            table.Write(writer, new[]
            {
                new User {Id = 1, Name = "ビル ゲイツ", Birthday = DateTime.Parse("1955/10/28")},
                new User {Id = 2, Name = "Steven Jobs", Birthday = DateTime.Parse("1955/2/24")}
            });

            Assert.Equal(
                @"
 +----+-------------+---------+-------------+--------------------+
 | Id | Name        | Parents | Occupations | Birthday           |
 +----+-------------+---------+-------------+--------------------+
 | 1  | ビル ゲイツ |         |             | 1955/10/28 0:00:00 |
 +----+-------------+---------+-------------+--------------------+
 | 2  | Steven Jobs |         |             | 1955/02/24 0:00:00 |
 +----+-------------+---------+-------------+--------------------+
", $"{Environment.NewLine}{writer}");
        }

        [Fact]
        public void WhenAutoGenerateColumnsIsTrue()
        {

            var table = Build.TextTable<User>(builder =>
            {
            });
            var text = table.ToString(new[]
            {
                new User {Id = 1, Name = "ビル ゲイツ", Birthday = DateTime.Parse("1955/10/28")},
                new User {Id = 2, Name = "Steven Jobs", Birthday = DateTime.Parse("1955/2/24")}
            });

            Assert.Equal(
                @"
 +----+-------------+---------+-------------+--------------------+
 | Id | Name        | Parents | Occupations | Birthday           |
 +----+-------------+---------+-------------+--------------------+
 | 1  | ビル ゲイツ |         |             | 1955/10/28 0:00:00 |
 +----+-------------+---------+-------------+--------------------+
 | 2  | Steven Jobs |         |             | 1955/02/24 0:00:00 |
 +----+-------------+---------+-------------+--------------------+
", $"{Environment.NewLine}{text}");
        }

        [Fact]
        public void WhenSpecifyPadding()
        {
            var table = Build.TextTable<User>(builder =>
            {
                builder
                    .PaddingAs(2)
                    .AddColumn(x => x.Id).NameAs("ID").HorizontalAlignmentAs(HorizontalAlignment.Right)
                    .AddColumn(x => x.Name).NameAs("氏名")
                    .AddColumn(x => x.Birthday).FormatAs("{0:yyyy/MM/dd}");
            });
            var text = table.ToString(new[]
            {
                new User {Id = 1, Name = "ビル ゲイツ", Birthday = DateTime.Parse("1955/10/28")},
                new User {Id = 2, Name = "Steven Jobs", Birthday = DateTime.Parse("1955/2/24")}
            });

            Assert.Equal(
                @"
 +------+---------------+--------------+
 |  ID  |  氏名         |  Birthday    |
 +------+---------------+--------------+
 |   1  |  ビル ゲイツ  |  1955/10/28  |
 +------+---------------+--------------+
 |   2  |  Steven Jobs  |  1955/02/24  |
 +------+---------------+--------------+
", $"{Environment.NewLine}{text}");
        }

        [Fact]
        public void WhenMultipleLines()
        {

            var table = Build.TextTable<User>(builder =>
            {
                builder
                    .AddColumn(x => x.Id).NameAs("ID").HorizontalAlignmentAs(HorizontalAlignment.Right)
                    .AddColumn(x => x.Name).VerticalAlignmentAs(VerticalAlignment.Center)
                    .AddColumn(x => x.Birthday).VerticalAlignmentAs(VerticalAlignment.Bottom).FormatAs("{0:yyyy/MM/dd}")
                    .AddColumn(x => x.Parents).VerticalAlignmentAs(VerticalAlignment.Center).FormatAs("- {0}")
                    .AddColumn(x => x.Occupations).HorizontalAlignmentAs(HorizontalAlignment.Center);
            });
            var text = table.ToString(new[]
            {
                new User
                {
                    Id = 1, 
                    Name = "Bill Gates", 
                    Birthday = DateTime.Parse("1955/10/28"),
                    Parents = $"Bill Gates Sr.{Environment.NewLine}Mary Maxwell Gates",
                    Occupations = new []{"Software developer", "Investor", "Entrepreneur", "Philanthropist"}
                }
            });

            Assert.Equal(
                @"
 +----+------------+------------+----------------------+--------------------+
 | ID | Name       | Birthday   | Parents              | Occupations        |
 +----+------------+------------+----------------------+--------------------+
 |  1 |            |            |                      | Software developer |
 |    | Bill Gates |            | - Bill Gates Sr.     |      Investor      |
 |    |            |            | - Mary Maxwell Gates |    Entrepreneur    |
 |    |            | 1955/10/28 |                      |   Philanthropist   |
 +----+------------+------------+----------------------+--------------------+
", $"{Environment.NewLine}{text}");
        }

        [Fact]
        public void WithAttribute()
        {

            var table = Build.TextTable<UserWithAttribute>();
            var text = table.ToString(new[]
            {
                new UserWithAttribute
                {
                    Id = 1,
                    Name = "Bill Gates",
                    Birthday = DateTime.Parse("1955/10/28"),
                    Parents = $"Bill Gates Sr.{Environment.NewLine}Mary Maxwell Gates",
                    Occupations = new []{"Software developer", "Investor", "Entrepreneur", "Philanthropist"}
                }
            });

            Assert.Equal(
                @"
 +----+------------+------------+----------------------+--------------------+
 | ID | Name       | Birthday   | Parents              | Occupations        |
 +----+------------+------------+----------------------+--------------------+
 |  1 |            |            |                      | Software developer |
 |    | Bill Gates |            | - Bill Gates Sr.     |      Investor      |
 |    |            |            | - Mary Maxwell Gates |    Entrepreneur    |
 |    |            | 1955/10/28 |                      |   Philanthropist   |
 +----+------------+------------+----------------------+--------------------+
", $"{Environment.NewLine}{text}");
        }

        private class UserWithAttribute
        {
            [Column(Index = 1, Header = "ID", HorizontalAlignment = HorizontalAlignment.Right)]
            public int Id { get; set; }

            [Column(Index = 2, VerticalAlignment = VerticalAlignment.Center)]
            public string Name { get; set; }

            [Column(Index = 3, VerticalAlignment = VerticalAlignment.Bottom, Format = "{0:yyyy/MM/dd}")]
            public DateTime Birthday;

            [Column(Index = 4, VerticalAlignment = VerticalAlignment.Center, Format = "- {0}")]
            public string Parents { get; set; }

            [Column(Index = 5, HorizontalAlignment = HorizontalAlignment.Center)]
            public string[] Occupations { get; set; }

        }

        private class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime Birthday;
            public string Parents { get; set; }
            public string[] Occupations { get; set; }

        }
    }
}
