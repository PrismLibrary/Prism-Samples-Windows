using System.Globalization;

namespace AdventureWorks.UILogic.Models
{
    public class ShippingMethod
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string EstimatedTime { get; set; }

        public double Cost { get; set; }

        public override string ToString()
        {
            // For Accessibility purposes
            return string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", Description, EstimatedTime, Cost);
        }
    }
}
