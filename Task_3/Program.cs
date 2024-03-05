using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string connectionString = ConfigurationManager.ConnectionStrings["CoffeeDB"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Coffee JOIN Country ON Coffee.id_country = Country.id_country", connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Coffee");

            // Task 3
            DataTable coffeeTable = dataSet.Tables["Coffee"];

            var coffeeByCountry = coffeeTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, Count = g.Count() });

            Console.WriteLine("Назва країни, та кількість сортів кави:");
            foreach (var item in coffeeByCountry)
                Console.WriteLine(
                    $"Країна: {item.Country,-10}" +
                    $"Кількість сортів кави: {item.Count,-10}"
                );

            var averageWeightByCountry = coffeeTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, AverageWeight = g.Average(r => r.Field<double>("weight")) });

            Console.WriteLine("\nСередня кількість грамів кави по кожній країні:");
            foreach (var item in averageWeightByCountry)
                Console.WriteLine(
                    $"Країна: {item.Country,-10}" +
                    $"Середня кількість грамів: {Math.Round(item.AverageWeight,2),-10}"
                );

            var cheapestCoffeeByCountry = coffeeTable.AsEnumerable()
                .Where(r => r.Field<string>("country_name") == "Бразилия")
                .OrderBy(r => r.Field<decimal>("cost"))
                .Take(3);

            Console.WriteLine("\nТри найдешевші сорти кави (Бразилія):");
            foreach (var row in cheapestCoffeeByCountry)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-28}" +
                    $"Ціна: {row.Field<decimal>("cost"),-10}"
                );

            var mostExpensiveCoffeeByCountry = coffeeTable.AsEnumerable()
                .Where(r => r.Field<string>("country_name") == "Бразилия")
                .OrderByDescending(r => r.Field<decimal>("cost"))
                .Take(3);

            Console.WriteLine("\nТри найдорожчі сорти кави (Бразилія):");
            foreach (var row in mostExpensiveCoffeeByCountry)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-28}" +
                    $"Ціна: {row.Field<decimal>("cost"),-10}"
                );

            var cheapestCoffee = coffeeTable.AsEnumerable()
                .OrderBy(r => r.Field<decimal>("cost"))
                .Take(3);

            Console.WriteLine("\nТри найдешевші сорти кави (всі країни):");
            foreach (var row in cheapestCoffee)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-28}" +
                    $"Ціна: {row.Field<decimal>("cost"),-10}"
                );

            var mostExpensiveCoffee = coffeeTable.AsEnumerable()
                .OrderByDescending(r => r.Field<decimal>("cost"))
                .Take(3);

            Console.WriteLine("\nТри найдорожчі сорти кави (всі країни):");
            foreach (var row in mostExpensiveCoffee)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-28}" +
                    $"Ціна: {row.Field<decimal>("cost"),-10}"
                );
        }
    }
}
