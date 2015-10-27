// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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
