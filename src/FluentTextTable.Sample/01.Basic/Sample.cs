﻿using System;

namespace FluentTextTable.Sample._01.Basic
{
    public class User
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string JapaneseName { get; set; }
        public DateTime Birthday;
    }

    public class Sample
    {
        public static void WriteConsole()
        {
            var users = new[]
            {
                new User {Id = 1, EnglishName = "Bill Gates", JapaneseName = "ビル・ゲイツ", Birthday = DateTime.Parse("1955/10/28")},
                new User {Id = 2, EnglishName = "Steven Jobs", JapaneseName = "スティーブ・ジョブズ", Birthday = DateTime.Parse("1955/2/24")}
            };
            Build
                .TextTable<User>()
                .WriteLine(users);
        }
    }


}