using System.Threading.Tasks;
using TL;

namespace NoTrace.Engine.Core;

/// <summary>
/// Defines architectural contracts for message-wiping, blocklist routines, and metadata cleanup.
/// </summary>
public interface ICleanupService
{
    Task<int[]> ScanMyMessageIdsAsync(InputPeer target);
    Task ExecuteChatWipeAsync(IPeerInfo target, string level, int[] messageIds, int? limit);
    Task PurgeBlocklistAsync(string option);
    Task PurgeUselessContactsAsync();
}