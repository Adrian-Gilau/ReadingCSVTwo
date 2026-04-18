using System.Security.Cryptography.X509Certificates;

namespace ReadingWritingCSV
{
    public class Program // or should I use internal
    {
        public class PersonalInformation
        {
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string Address { get; set; } = "";
            public string Phone { get; set; } = "";

        }
        static string GetNextAvailableFileName(string folder, string baseName)
        {
            string filePath = Path.Combine(folder, baseName + ".txt");

            if (!File.Exists(filePath))
                return filePath;

            int counter = 1;

            while (true)
            {
                filePath = Path.Combine(folder, $"{baseName}_{counter}.txt");

                if (!File.Exists(filePath))
                    return filePath;

                counter++;
            }
        }
        static void Main(string[] args)
        {
            // Path:
            // C:\\Users\\User\\Desktop\\EnverSoft\\Data.csv

            //StreamReader? reader = null;
            string? path = null;

            int attempts = 0;
            bool validPathEntered = false;

            // Give the user 10 attempts to enter a valid path

            while (attempts < 10)
            {
                Console.WriteLine($"Attempt {attempts + 1} of 10");
                Console.Write("Please enter the vaild path of your csv file: ");

                path = Console.ReadLine();

                // Check if empty 

                if (string.IsNullOrWhiteSpace(path))
                {
                    Console.WriteLine("No file path entered. \n");
                    attempts++;
                    continue;
                }

                // Check for illegal characters

                if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    char[] invalidChars = Path.GetInvalidFileNameChars();

                    var InvalidCharsEntered = path
                        .Where(x => invalidChars.Contains(x))
                        .Distinct()
                        .ToArray();

                    Console.WriteLine("The file path entered contains illegal characters.");
                    Console.WriteLine("Illegal characters found: " + string.Join(" ", InvalidCharsEntered));
                    Console.WriteLine("\nExamples of invalid path characters:");
                    Console.WriteLine(string.Join(" ", invalidChars));

                    attempts++;
                    continue;
                }

                // Check if the file exists

                if (!File.Exists(path))
                {
                    Console.WriteLine("The file path doesn't exist. \n");
                    attempts++;
                    continue;
                }

                // Check if file is locked / open elsewhere

                try
                {
                    using FileStream testStream = new FileStream(
                        path,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.None);

                    validPathEntered = true;
                    break;
                }
                catch (IOException)
                {
                    Console.WriteLine("The file is currently open in another program.");
                    Console.WriteLine("Please close the file and try again.\n");
                    attempts++;
                    continue;
                }
            }

            if (!validPathEntered)
            {
                Console.WriteLine("Maximun attempts reached. Program shutting down");
                return;
            }

            string[]? headers = null;
            List<PersonalInformation> people = new();

            // Read and display CSV contents
            try
            {
                using StreamReader reader = new StreamReader(path);

                int lineNumber = 0;

                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(',');

                    if (lineNumber == 0)
                    {
                        headers = parts;
                        lineNumber++;
                        continue;
                    }

                    PersonalInformation personalInformation = new PersonalInformation
                    {
                        FirstName = parts[0],
                        LastName = parts[1],
                        Address = parts[2],
                        Phone = parts[3]
                    };

                    people.Add(personalInformation);
                    lineNumber++;


                }

                string directory = Path.GetDirectoryName(path) ?? "";
                string nameFile = GetNextAvailableFileName(directory, "NamesOutput");
                string addressFile = GetNextAvailableFileName(directory, "AddressOutput");

                var nameFrequency = people
    .SelectMany(p => new[] { p.FirstName, p.LastName })
    .GroupBy(name => name)
    .Select(g => new
    {
        Name = g.Key,
        Count = g.Count()
    })
    .OrderByDescending(x => x.Count)
    .ThenBy(x => x.Name)
    .ToList();

                //Console.WriteLine("\n=== Name Frequency (Descending, then Alphabetical) ===");

                //foreach (var item in nameFrequency)
                //{
                //    Console.WriteLine($"{item.FirstName} {item.LastName} - {item.Count}");
                //}

                using (StreamWriter writer = new StreamWriter(nameFile))
                {
                    writer.WriteLine("=== Name Frequency ===");

                    foreach (var item in nameFrequency)
                    {
                        writer.WriteLine($"{item.Name} - {item.Count}");
                    }
                }

                var sortedAddresses = people
    .OrderBy(p =>
    {
        string[] parts = p.Address.Split(' ');
        return parts.Length > 1 ? parts[1] : p.Address;
    })
    .ToList();

                //Console.WriteLine("\n=== Addresses Sorted by Street Name ===");

                //foreach (var p in sortedAddresses)
                //{
                //    Console.WriteLine(p.Address);
                //}

                using (StreamWriter writer = new StreamWriter(addressFile))
                {
                    writer.WriteLine("=== Addresses Sorted by Street Name ===");

                    foreach (var person in sortedAddresses)
                    {
                        writer.WriteLine(person.Address);
                    }
                }

                Console.WriteLine("\nFile read successfully.");
                Console.WriteLine("\nProcessing complete.");
                Console.WriteLine($"Names file saved to: {nameFile}");
                Console.WriteLine($"Addresses file saved to: {addressFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred:");
                Console.WriteLine(ex.Message);
            }

        }
    }
}