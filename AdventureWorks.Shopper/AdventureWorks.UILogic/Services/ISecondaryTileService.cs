using System;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace AdventureWorks.UILogic.Services
{
    public interface ISecondaryTileService
    {
        bool SecondaryTileExists(string tileId);

        Task<bool> PinSquareSecondaryTile(string tileId, string displayName, string arguments);

        Task<bool> PinWideSecondaryTile(string tileId, string displayName, string arguments);

        Task<bool> UnpinTile(string tileId);

        void ActivateTileNotifications(string tileId, Uri tileContentUri, PeriodicUpdateRecurrence recurrence);
    }
}
