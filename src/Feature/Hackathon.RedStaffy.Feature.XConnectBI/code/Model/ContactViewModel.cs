using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.XConnect;

namespace Hackathon.RedStaffy.XConnectBI.Model
{
    public class ContactViewModel
    {
        public Guid? ContactId { get; set; }
        public List<InteractionEvent> Events { get; set; }

        public bool IsKnown { get; set; }
    }
}