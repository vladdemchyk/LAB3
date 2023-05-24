// Task 1: Read CSV file and calculate total amount spent per day
public void CalculateTotalAmountSpentPerDay(string csvFilePath, string dateFormat)
{
    try
    {
        // Define delegates
        Func<string, DateTime> getDate = (string transaction) => DateTime.ParseExact(transaction.Split(',')[0], dateFormat, CultureInfo.InvariantCulture);
        Func<string, double> getAmount = (string transaction) => double.Parse(transaction.Split(',')[1]);
        Action<DateTime, double> displayTotalAmountSpentPerDay = (DateTime date, double totalAmount) => Console.WriteLine($"{date.ToString(dateFormat)}: {totalAmount}");

        // Read CSV file and group transactions by date
        var transactionsByDate = File.ReadAllLines(csvFilePath)
            .Skip(1)
            .Select(transaction => new { Date = getDate(transaction), Amount = getAmount(transaction) })
            .GroupBy(transaction => transaction.Date.Date);

        // Calculate total amount spent per day and display results
        foreach (var group in transactionsByDate)
        {
            double totalAmount = group.Sum(transaction => transaction.Amount);
            displayTotalAmountSpentPerDay(group.Key, totalAmount);
        }
    }
    catch (Exception e)
    {
        // Log the error
        Console.WriteLine($"Error: {e.Message}");
    }
}

// Task 2: Filter products from JSON files based on user-defined criteria
public void FilterProductsFromJsonFiles(string jsonFilePathFormat, List<Predicate<Product>> filters, Action<Product> displayProduct)
{
    try
    {
        // Define delegate
        Func<int, string> getJsonFilePath = (int i) => string.Format(jsonFilePathFormat, i);

        // Read JSON files and filter products
        for (int i = 1; i <= 10; i++)
        {
            string jsonFilePath = getJsonFilePath(i);
            var products = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(jsonFilePath));
            var filteredProducts = products.Where(product => filters.All(filter => filter(product)));

            // Display filtered products
            foreach (var product in filteredProducts)
            {
                displayProduct(product);
            }
        }
    }
    catch (Exception e)
    {
        // Log the error
        Console.WriteLine($"Error: {e.Message}");
    }
}

// Task 3: Apply image processing operations to a set of images
public void ApplyImageProcessingOperationsToImages(List<Bitmap> images, List<Func<Bitmap, Bitmap>> operations, Action<Bitmap> displayImage)
{
    try
    {
        // Apply operations to images and display results
        foreach (var image in images)
        {
            Bitmap processedImage = image;
            foreach (var operation in operations)
            {
                processedImage = operation(processedImage);
            }
            displayImage(processedImage);
        }
    }
    catch (Exception e)
    {
        // Log the error
        Console.WriteLine($"Error: {e.Message}");
    }
}

// Task 4: Generate report on word frequency in a set of text files
public void GenerateWordFrequencyReport(List<string> textFilePaths, Func<string, IEnumerable<string>> tokenizeText, Func<IEnumerable<string>, IDictionary<string, int>> countWordFrequency, Action<IDictionary<string, int>> displayReport)
{
    try
    {
        // Tokenize text and count word frequency for each file
        var wordFrequencyByFile = textFilePaths.ToDictionary(filePath => filePath, filePath =>
        {
            var text = File.ReadAllText(filePath);
            var tokens = tokenizeText(text);
            return countWordFrequency(tokens);
        });

        // Merge word frequency counts for all files
        var totalWordFrequency = wordFrequencyByFile.Values.SelectMany(dict => dict)
            .GroupBy(pair => pair.Key)
            .ToDictionary(group => group.Key, group => group.Sum(pair => pair.Value));

        // Display word frequency report
        displayReport(totalWordFrequency);
    }
    catch (Exception e)
    {
        // Log the error
        Console.WriteLine($"Error: {e.Message}");
    }
}

// Product class for Task 2
public class Product
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string Category { get; set; }
}