﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace Digipost.Api.Client.Shared.Xml
{
    public class ValidationMessages : List<string>
    {
        public bool HasErrors { get; private set; }

        public bool HasWarnings { get; private set; }

        public void Add(XmlSeverityType severity, string message) //Todo: Var internal, og bør nok være det videre
        {
            Add(message);

            HasErrors = severity == XmlSeverityType.Error;
            HasWarnings = severity == XmlSeverityType.Warning;
        }

        public override string ToString()
        {
            return Count <= 0 ? "" : this.Aggregate((current, variable) => current + Environment.NewLine + variable);
        }
    }
}