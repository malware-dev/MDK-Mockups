﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MDK_UI.MockupExtensions;

namespace IngameScript.Mockups.Base
{
    public partial class MockTerminalBlock: IMockupDataTemplateProvider
    {
        protected virtual string DataTemplateName { get; } = "dtUnsupportedBlock";
        public DataTemplate DataTemplate => (DataTemplate) new DisplayTemplateProvider()[$"dt{DataTemplateName}"];
    }
}
