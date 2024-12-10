using System.Text.Json;

namespace CourseWorkMS;

public class Serializer
{
    public static void SerializeDictionaryToFile<T>(T objectToSerialize, string filePath)
    {
        try
        {
            // Serialize the dictionary to a JSON string
            string json = JsonSerializer.Serialize(objectToSerialize, new JsonSerializerOptions
            {
                WriteIndented = true // Makes the JSON output more readable
            });

            // Write the JSON string to the specified file
            File.WriteAllText(filePath, json);

            Console.WriteLine($"Dictionary successfully serialized to {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error serializing dictionary to file: {ex.Message}");
        }
    }
}