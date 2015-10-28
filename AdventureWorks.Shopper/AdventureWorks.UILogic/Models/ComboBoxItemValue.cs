namespace AdventureWorks.UILogic.Models
{
    public class ComboBoxItemValue
    {
        public string Id { get; set; }

        public string Value { get; set; }
        
        public override string ToString()
        {
            // Narrator support
            return Value;
        }
    }
}
