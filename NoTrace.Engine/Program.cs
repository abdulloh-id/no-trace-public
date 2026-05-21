using System;
using System.Text;
using System.Threading.Tasks;
using WTelegram;
using NoTrace.Engine.Configuration;
using NoTrace.Engine.Core;
using NoTrace.Engine.UI;

try
{
    // Configure runtime encoding schemas to support trilingual character sets natively
    Console.OutputEncoding = Encoding.UTF8;
    Console.InputEncoding = Encoding.UTF8;

    Console.WriteLine(LocaleManager.T(TextKey.EngineInitializing));

    // Initialize core protocol handshake using the memory-cached environment state
    using var client = new Client(EnvManager.ConfigProvider);
    var user = await client.LoginUserIfNeeded();
    
    Console.WriteLine(LocaleManager.T(TextKey.LoginSuccess, user.first_name, user.id));

    // Resolve public dependencies and execute primary UI interaction loop
    ICleanupService cleanupService = new BaseCleanupService(client);
    var menuController = new MenuController(client, cleanupService);

    await menuController.StartEngineAsync();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n[Fatal Runtime Exception] Engine initialization aborted: {ex.Message}");
    Console.ResetColor();
}
finally
{
    Console.WriteLine(LocaleManager.T(TextKey.EngineShutdown));
}