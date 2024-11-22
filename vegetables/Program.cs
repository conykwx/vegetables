using Microsoft.Data.SqlClient;
using System;

class Program
{
    static string connectionString = "Data Source=DESKTOP-9K56BQI\\SQLEXPRESS;Initial Catalog=shop;Integrated Security=True;TrustServerCertificate=True";

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Меню:");
            Console.WriteLine("1 - Показати всі дані");
            Console.WriteLine("2 - Показати усі назви");
            Console.WriteLine("3 - Показати усі кольори");
            Console.WriteLine("4 - Показати максимальну калорійність");
            Console.WriteLine("5 - Показати мінімальну калорійність");
            Console.WriteLine("6 - Показати середню калорійність");
            Console.WriteLine("7 - Показати кількість овочів");
            Console.WriteLine("8 - Показати кількість фруктів");
            Console.WriteLine("9 - Показати кількість за кольором");
            Console.WriteLine("10 - Показати дані за калорійністю");
            Console.WriteLine("exit - Вихід з програми");
            Console.Write("\nВиберіть опцію: ");
            string? choice = Console.ReadLine()?.ToLower();

            if (choice == "exit")
                break;

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    switch (choice)
                    {
                        case "1":
                            ShowAllData(connection);
                            break;
                        case "2":
                            ShowAllNames(connection);
                            break;
                        case "3":
                            ShowAllColors(connection);
                            break;
                        case "4":
                            ShowMaxCalories(connection);
                            break;
                        case "5":
                            ShowMinCalories(connection);
                            break;
                        case "6":
                            ShowAverageCalories(connection);
                            break;
                        case "7":
                            ShowVegetableCount(connection);
                            break;
                        case "8":
                            ShowFruitCount(connection);
                            break;
                        case "9":
                            ShowCountByColor(connection);
                            break;
                        case "10":
                            ShowByCalories(connection);
                            break;
                        default:
                            Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
        }
    }

    static void ShowAllData(SqlConnection connection)
    {
        string query = "SELECT * FROM VegetablesAndFruits;";
        var command = new SqlCommand(query, connection);
        var reader = command.ExecuteReader();

        Console.WriteLine("\nВсі дані:");
        while (reader.Read())
        {
            Console.WriteLine($"Назва: {reader["name"]}, Тип: {reader["type"]}, Колір: {reader["color"]}, Калорійність: {reader["calories"]}");
        }
    }

    static void ShowAllNames(SqlConnection connection)
    {
        string query = "SELECT name FROM VegetablesAndFruits;";
        var command = new SqlCommand(query, connection);
        var reader = command.ExecuteReader();

        Console.WriteLine("\nУсі назви:");
        while (reader.Read())
        {
            Console.WriteLine(reader["name"]);
        }
    }

    static void ShowAllColors(SqlConnection connection)
    {
        string query = "SELECT DISTINCT color FROM VegetablesAndFruits;";
        var command = new SqlCommand(query, connection);
        var reader = command.ExecuteReader();

        Console.WriteLine("\nУсі кольори:");
        while (reader.Read())
        {
            Console.WriteLine(reader["color"]);
        }
    }

    static void ShowMaxCalories(SqlConnection connection)
    {
        string query = "SELECT MAX(calories) FROM VegetablesAndFruits;";
        var command = new SqlCommand(query, connection);

        Console.WriteLine($"\nМаксимальна калорійність: {command.ExecuteScalar()}");
    }

    static void ShowMinCalories(SqlConnection connection)
    {
        string query = "SELECT MIN(calories) FROM VegetablesAndFruits;";
        var command = new SqlCommand(query, connection);

        Console.WriteLine($"\nМінімальна калорійність: {command.ExecuteScalar()}");
    }

    static void ShowAverageCalories(SqlConnection connection)
    {
        string query = "SELECT AVG(calories) FROM VegetablesAndFruits;";
        var command = new SqlCommand(query, connection);

        Console.WriteLine($"\nСередня калорійність: {command.ExecuteScalar()}");
    }

    static void ShowVegetableCount(SqlConnection connection)
    {
        string query = "SELECT COUNT(*) FROM VegetablesAndFruits WHERE type = 'Овоч';";
        var command = new SqlCommand(query, connection);

        Console.WriteLine($"\nКількість овочів: {command.ExecuteScalar()}");
    }

    static void ShowFruitCount(SqlConnection connection)
    {
        string query = "SELECT COUNT(*) FROM VegetablesAndFruits WHERE type = 'Фрукт';";
        var command = new SqlCommand(query, connection);

        Console.WriteLine($"\nКількість фруктів: {command.ExecuteScalar()}");
    }

    static void ShowCountByColor(SqlConnection connection)
    {
        Console.Write("\nВведіть колір: ");
        string? color = Console.ReadLine();
        string query = "SELECT COUNT(*) FROM VegetablesAndFruits WHERE color = @color;";
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@color", color);

        Console.WriteLine($"\nКількість обраного кольору ({color}): {command.ExecuteScalar()}");
    }

    static void ShowByCalories(SqlConnection connection)
    {
        Console.Write("\nВведіть мінімальну калорійність: ");
        int min = int.Parse(Console.ReadLine());
        Console.Write("Введіть максимальну калорійність: ");
        int max = int.Parse(Console.ReadLine());

        string query = "SELECT name, calories FROM VegetablesAndFruits WHERE calories BETWEEN @min AND @max;";
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@min", min);
        command.Parameters.AddWithValue("@max", max);

        var reader = command.ExecuteReader();

        Console.WriteLine("\nПродукти в заданому діапазоні калорійності:");
        while (reader.Read())
        {
            Console.WriteLine($"{reader["name"]}: {reader["calories"]} калорій");
        }
    }
}
