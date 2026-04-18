namespace ReadingWritingCSV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Path:
            // C:\\Users\\User\\Desktop\\EnverSoft\\Data.csv

            StreamReader? reader = null;
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

                // Valid path found

                validPathEntered = true;
                break;
            }

            if (!validPathEntered)
            {
                Console.WriteLine("Maximun attempts reached. Program shutting down");
                return;
            }

            // Use the stream reader to open the file
            reader = new StreamReader(File.OpenRead(path));


            // Loop to make sure we go through each line
            while (!reader.EndOfStream)
            {

                int count = 0;
                // Here is where we can access each line as it's read
                var line = reader.ReadLine();
                var values = line.SplitAny(',');
                Console.WriteLine("Line" + count++ + ": " + line);
                //Console.WriteLine("Values" + count++ + ": " + string.Join(", ", values));

            }
        }
    }
}