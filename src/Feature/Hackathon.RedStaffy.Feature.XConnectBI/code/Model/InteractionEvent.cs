using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.RedStaffy.XConnectBI.Model
{
    public class InteractionEvent
    {
        public string EventType { get; set; }

        public string Data { get; set; }

        public string DataKey { get; set; }

        public Guid DefinitionId { get; internal set; }

        public Guid ItemId { get; set; }

        public int EngagementValue { get; set; }

        public Guid Id { get; set; }

        public Guid? ParentEventId { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; internal set; }

        public TimeSpan Duration { get; set; }
    }
}