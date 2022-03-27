using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HomeWork_L_Loggin_Collection
{

    internal interface ILogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception ex);
    }
    internal class LocalFileLogger<T> : ILogger
    {
        private readonly string path;
        private string GenericTypeName => typeof(T).Name;

        public LocalFileLogger(string path)
        {
            this.path = path;
        }

        public void LogError(string message, Exception ex)
        {
            Write($"[Info]: [{GenericTypeName}] : {message}");
        }

        public void LogInfo(string message)
        {
            Write($"[Info]: [{GenericTypeName}] : {message}");
        }

        public void LogWarning(string message)
        {
            Write($"[Warning] : [{GenericTypeName}] : {message}");
        }

        private void Write(string message)
        {
            using (StreamWriter w = new StreamWriter(path, true))
            {
                w.WriteLine(message);
            }

        }
    }

    public class Entity
        {
            public int id { get; set; }
            public int ParentId { get; set; }
            public string Name { get; set; }

            public static Dictionary<int, List<Entity>> Converting(List<Entity> entity)
            {
                if (entity.Count == 0) throw new Exception("Список пуст!Заполни его:)");
                if (entity.Select(x => x.id).Distinct().Count() != entity.Count()) throw new Exception("id не уникален!");
                Dictionary<int, List<Entity>> output =
                    entity.GroupBy(x => x.ParentId)
                        .ToDictionary(key => key.Key, vals => vals.ToList());
                foreach (KeyValuePair<int, List<Entity>> pair in output)
                {
                    Console.WriteLine($"Key = {pair.Key}, Value = List{{{string.Join(", ", pair.Value)}}}"); 
                }

                return output;
            }

            public override string ToString()
            {
                return $"Entity{{Id = {id}}}";
            }

        }

        class Program
        {
            public static void Main()
            {
            Console.WriteLine("1 Задание: ");
            LocalFileLogger<string> test1 = new LocalFileLogger<string>("C:/Users/12/Documents/loggingtest.txt");
            test1.LogInfo("User1 is writing name");
            test1.LogWarning("Sorry, We're working as fast as we can ಥ﹏ಥ");
            test1.LogError("Everything is very bad!!!!", new Exception("Help"));

            LocalFileLogger<int> test2 = new LocalFileLogger<int>("C:/Users/12/Documents/loggingtest.txt");
            test2.LogInfo("User1 is writing name");
            test2.LogWarning("Sorry, We're working as fast as we can ಥ﹏ಥ");
            test2.LogError("Everything is very bad!!!!", new Exception("Help"));
            Console.WriteLine("2 Задание:");
            List <Entity> entity = new List<Entity>
            {
                new Entity { id= 1,  ParentId = 0,Name = "Root entity"},
                new Entity { id= 2,  ParentId = 1,Name = "Child of 1 entity" },
                new Entity { id= 3,  ParentId = 1,Name = "Child of 1 entity" },
                new Entity { id= 4,  ParentId = 2,Name = "Child of 2 entity" },
                new Entity { id= 5,  ParentId = 4,Name = "Child of 4 entity" }
            };
                Entity.Converting(entity);
            }
        }
    }
