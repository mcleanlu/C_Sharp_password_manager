using System;
using System.Collections.Generic;
using System.IO;

class PasswordEntity
{
    public string ID { get; set; }
    public string Password { get; set; }
    public string Url { get; set; }

    public PasswordEntity(string url, string id, string password)
    {
        Url = url;
        ID = id;
        Password = password;
    }
}

struct FileOperations
{
    public string FilePath { get; set; }

    public void WriteToFile(List<PasswordEntity> entities)
    {
        using (StreamWriter sw = new StreamWriter(FilePath))
        {
            foreach (var entity in entities)
            {
                sw.WriteLine($"{entity.Url},{entity.ID},{entity.Password}");
            }
        }
    }

    public List<PasswordEntity> ReadFromFile()
    {
        var entities = new List<PasswordEntity>();
        using (StreamReader sr = new StreamReader(FilePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] details = line.Split(',');
                entities.Add(new PasswordEntity(details[0], details[1], details[2]));
            }
        }
        return entities;
    }
}

class Program
{
    static void Main(string[] args)
    {
        FileOperations fileOperations = new FileOperations { FilePath = "passwords.txt" };
        List<PasswordEntity> passwordEntities = fileOperations.ReadFromFile();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Add a website");
            Console.WriteLine("2. View a website");
            Console.WriteLine("3. Delete a website");
            Console.WriteLine("4. Exit");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Write("Enter website address: ");
                    string url = Console.ReadLine();
                    Console.Write("Enter username: ");
                    string id = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();
                    passwordEntities.Add(new PasswordEntity(url, id, password));
                    fileOperations.WriteToFile(passwordEntities);
                    break;
                case "2":
                    if (passwordEntities.Count == 0)
                    {
                        Console.WriteLine("No websites found.");
                        break;
                    }

                    Console.WriteLine("Websites:");
                    foreach (var e in passwordEntities)
                    {
                        Console.WriteLine(e.Url);
                    }

                    Console.Write("Enter website address to view: ");
                    string viewUrl = Console.ReadLine();
                    PasswordEntity entityToView = passwordEntities.Find(e => e.Url == viewUrl);
                    if (entityToView != null)
                    {
                        Console.WriteLine($"Username: {entityToView.ID}");
                        Console.WriteLine($"Password: {entityToView.Password}");
                    }
                    else
                    {
                        Console.WriteLine("Website not found.");
                    }
                    break;
                case "3":
                    Console.Write("Enter website address to delete: ");
                    string deleteUrl = Console.ReadLine();
                    PasswordEntity deleteEntity = passwordEntities.Find(e => e.Url == deleteUrl);
                    if (deleteEntity != null)
                    {
                        passwordEntities.Remove(deleteEntity);
                        fileOperations.WriteToFile(passwordEntities);
                        Console.WriteLine("Website deleted.");
                    }
                    else
                    {
                        Console.WriteLine("Website not found.");
                    }
                    break;
                case "4":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please enter a number between 1 and 4.");
                    break;
            }
        }
    }
}
