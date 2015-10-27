// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace AdventureWorks.UILogic.Models
{
    /// Documentation on validating user input is at http://go.microsoft.com/fwlink/?LinkID=288817&clcid=0x409
    /// 
    public class ModelValidationResult
    {
        public ModelValidationResult()
        {
            ModelState = new Dictionary<string, List<string>>();
        }

        public Dictionary<string, List<string>> ModelState { get; private set; }
    }
}
