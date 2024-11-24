﻿using System.Text.Json;

namespace CourseWorkMSExperiments;

public class Serializer
{
    public static void SerializeDictionaryToFile<TKey, TValue>(Dictionary<TKey, TValue> dictionary, string filePath)
    {
        try
        {
            // Serialize the dictionary to a JSON string
            string json = JsonSerializer.Serialize(dictionary, new JsonSerializerOptions
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