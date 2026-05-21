using System;
using System.Collections.Generic;
using System.IO;

namespace NoTrace.Engine.Configuration;

/// <summary>
/// Provides thread-safe, memory-cached access to local environment configurations.
/// </summary>
public static class EnvManager
{
    private static readonly Dictionary<string, string> EnvCache = new(StringComparer.OrdinalIgnoreCase);

    static EnvManager()
    {
        try
        {
            string? currentDir = AppDomain.CurrentDomain.BaseDirectory;
            string? envPath = null;

            // Traverse upwards to locate the root environment configuration file
            while (currentDir != null)
            {
                var potentialPath = Path.Combine(currentDir, ".env");
                if (File.Exists(potentialPath))
                {
                    envPath = potentialPath;
                    break;
                }
                currentDir = Directory.GetParent(currentDir)?.FullName;
            }

            if (envPath == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Configuration Error] Critical initialization failure: .env configuration file mismatch or not found.");
                Console.ResetColor();
                return;
            }

            // In-memory tokenization of key-value assignments
            foreach (var line in File.ReadAllLines(envPath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#")) 
                    continue;

                var parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    EnvCache[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Configuration Exception] Failed to parse target runtime parameters: {ex.Message}");
        }
    }

    private static string? GetEnv(string key)
    {
        return EnvCache.TryGetValue(key, out var val) ? val : null;
    }

    public static string ConfigProvider(string what)
    {
        switch (what)
        {
            case "api_id": return GetEnv("API_ID") ?? "";
            case "api_hash": return GetEnv("API_HASH") ?? "";
            case "phone_number": return GetEnv("PHONE_NUMBER") ?? "";
            case "verification_code":
                Console.Write("Enter verification challenge code: ");
                return Console.ReadLine() ?? "";
            case "password":
                Console.Write("Enter secondary account security token (2FA): ");
                return Console.ReadLine() ?? "";
            default: return null!;
        }
    }
}