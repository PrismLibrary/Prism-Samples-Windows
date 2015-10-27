// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;

namespace AdventureWorks.UILogic.Services
{
    public class DialogCommand
    {
        public object Id { get; set; }

        public string Label { get; set; }

        public Action Invoked { get; set; }
    }
}
