using System;
using System.Threading.Tasks;
using TL;

namespace NoTrace.Engine.Core;

/// <summary>
/// Provides a baseline, sequential implementation of core cleanup services for the public runtime.
/// </summary>
public class BaseCleanupService : ICleanupService
{
    private readonly WTelegram.Client _client;

    public BaseCleanupService(WTelegram.Client client)
    {
        _client = client;
    }

    public async Task<int[]> ScanMyMessageIdsAsync(InputPeer target)
    {
        // Public baseline implementation: Returns an empty sequence or standard fetch window
        return await Task.FromResult(Array.Empty<int>());
    }

    public async Task ExecuteChatWipeAsync(IPeerInfo target, string level, int[] messageIds, int? limit)
    {
        // Public baseline implementation: Basic standard runtime loop
        Console.WriteLine("\n[Base Engine] Launching standard sequential deletion loop profile...");
        await Task.CompletedTask;
    }

    public async Task PurgeBlocklistAsync(string option)
    {
        await Task.CompletedTask;
    }

    public async Task PurgeUselessContactsAsync()
    {
        await Task.CompletedTask;
    }
}