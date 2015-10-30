using System;

namespace AdventureWorks.UILogic
{
    /// <summary>
    /// The IFlyoutViewModel interface should be implemented by the Flyout view model classes, to provide actions used for opening the Flyout, closing it,
    /// and handling the back button clicks. 
    /// </summary>
    public interface IFlyoutViewModel
    {
        /// <summary>
        /// Gets or sets the action used to close the Flyout.
        /// </summary>
        /// <value>
        /// The action that will be executed to close the Flyout.
        /// </value>
        Action CloseFlyout { get; set; }
    }
}
