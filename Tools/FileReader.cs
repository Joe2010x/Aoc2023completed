namespace Tools;

public class FileReaderClass
{
    public static List<string> ReadFromFile(string fileName)
    {
        // var rootPath = "c:\\Users\\Joe\\Documents\\Aoc2023\\Aoc2023\\";
        // var path = "d1m/";
        var result = new List<string>();
        try
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(fileName);
            //Read the first line of text
            var line = sr.ReadLine();
            //Continue to read until you reach end of file
            while (line != null)
            {
                //write the line to console window
                result.Add(line);
                //Read the next line
                line = sr.ReadLine();
            }
            //close the file
            sr.Close();
                
            return result;
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
            return result;
        }
        finally
        {
            Console.WriteLine("Executing final block.");
        }
    }
}