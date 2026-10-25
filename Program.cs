namespace Day_01_G03;

class CaseInsensitiveComparer : IComparer<string>
{
    public int Compare(string x, string y) => string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
}

internal class Program
{
    static void Print<T>(string title, IEnumerable<T> items)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {title} ===");
        foreach (T item in items)
            Console.WriteLine(item);
    }

    static void Main(string[] args)
    {
        List<Product> products = ListGenerator.ProductsList;
        List<Customer> customers = ListGenerator.CustomersList;

        string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        int[] arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
        string[] fruits = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };
        string[] words = File.ReadAllLines("dictionary_english.txt");

        // =================== Restriction Operators ===================
        Print("R1: out of stock", products.Where(p => p.UnitsInStock == 0));

        Print("R2: in stock and price > 3.00",
            products.Where(p => p.UnitsInStock > 0 && p.UnitPrice > 3.00M));

        Print("R3: digits whose name is shorter than their value",
            digits.Where((name, value) => name.Length < value));

        // =================== Element Operators ===================
        Product firstOut = products.FirstOrDefault(p => p.UnitsInStock == 0);
        Console.WriteLine();
        Console.WriteLine("=== E1: first product out of stock ===");
        Console.WriteLine(firstOut?.ToString() ?? "No match");

        Product firstExpensive = products.FirstOrDefault(p => p.UnitPrice > 1000);
        Console.WriteLine("=== E2: first product with price > 1000 ===");
        Console.WriteLine(firstExpensive?.ToString() ?? "null (no match)");

        Console.WriteLine("=== E3: second number greater than 5 ===");
        Console.WriteLine(arr.Where(n => n > 5).ElementAtOrDefault(1));

        // =================== Aggregate Operators ===================
        Console.WriteLine();
        Console.WriteLine("=== A1: number of odd numbers ===");
        Console.WriteLine(arr.Count(n => n % 2 != 0));

        Print("A2: customers and their orders count",
            customers.Select(c => new { c.CustomerName, OrdersCount = c.Orders.Length }));

        Print("A3: categories and products count",
            products.GroupBy(p => p.Category)
                    .Select(g => new { Category = g.Key, ProductsCount = g.Count() }));

        Console.WriteLine("=== A4: total of numbers ===");
        Console.WriteLine(arr.Sum());

        Console.WriteLine("=== A5: total characters in dictionary ===");
        Console.WriteLine(words.Sum(w => (long)w.Length));

        Console.WriteLine("=== A6: shortest word length ===");
        Console.WriteLine(words.Min(w => w.Length));

        Console.WriteLine("=== A7: longest word length ===");
        Console.WriteLine(words.Max(w => w.Length));

        Console.WriteLine("=== A8: average word length ===");
        Console.WriteLine(words.Average(w => w.Length));

        // =================== Ordering Operators ===================
        Print("O1: products by name", products.OrderBy(p => p.ProductName));

        Print("O2: case-insensitive sort (custom comparer)",
            fruits.OrderBy(w => w, new CaseInsensitiveComparer()));

        Print("O3: products by units in stock (high to low)",
            products.OrderByDescending(p => p.UnitsInStock));

        Print("O4: digits by name length then alphabetically",
            digits.OrderBy(d => d.Length).ThenBy(d => d));

        Print("O5: by length then case-insensitive",
            fruits.OrderBy(w => w.Length).ThenBy(w => w, new CaseInsensitiveComparer()));

        Print("O6: products by category then price (high to low)",
            products.OrderBy(p => p.Category).ThenByDescending(p => p.UnitPrice));

        Print("O7: by length then case-insensitive descending",
            fruits.OrderBy(w => w.Length).ThenByDescending(w => w, new CaseInsensitiveComparer()));

        Print("O8: digits with second letter 'i' (reversed)",
            digits.Where(d => d.Length > 1 && d[1] == 'i').Reverse());

        // =================== Transformation Operators ===================
        Print("T1: product names", products.Select(p => p.ProductName));

        string[] threeWords = { "aPPLE", "BlUeBeRrY", "cHeRry" };
        Print("T2: upper & lower versions",
            threeWords.Select(w => new { Upper = w.ToUpper(), Lower = w.ToLower() }));

        Print("T3: product properties (UnitPrice renamed Price)",
            products.Select(p => new { p.ProductName, p.Category, Price = p.UnitPrice }));

        Print("T4: does the number match its position?",
            arr.Select((n, index) => $"{n}: {n == index}"));

        int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
        int[] numbersB = { 1, 3, 5, 7, 8 };
        Print("T5: pairs where a < b",
            from a in numbersA
            from b in numbersB
            where a < b
            select $"{a} is less than {b}");

        Print("T6: orders with total < 500",
            customers.SelectMany(c => c.Orders).Where(o => o.Total < 500.00M));

        Print("T7: orders made in 1998 or later",
            customers.SelectMany(c => c.Orders).Where(o => o.OrderDate.Year >= 1998));
    }
}
