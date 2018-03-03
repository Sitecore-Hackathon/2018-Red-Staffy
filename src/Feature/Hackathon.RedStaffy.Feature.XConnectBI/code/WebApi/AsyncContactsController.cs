using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Hackathon.RedStaffy.XConnectBI.Model;
using Hackathon.RedStaffy.XConnectBI.Repositories;
using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Web.Http;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;

namespace Hackathon.RedStaffy.XConnectBI.WebApi
{
    /// <summary>
    /// api/Hackathon-RedStaffy-XConnectBI-WebApi/Contacts/GetInteractions
    /// </summary>
    [ServicesController]
    public class AsyncContactsController : ServicesApiController
    {
        /// <summary>
        /// Get interactions of given interaction. 
        /// </summary>
        /// <param name="startDateStr">Start Date</param>
        /// <param name="endDateStr">End Data</param>
        /// <param name="eventType">Event Type</param>
        /// <returns>Return List of Results.</returns>
        [HttpGet]
        public Task<List<ContactViewModel>> Get([FromUri] string startDateStr, string endDateStr,
            string eventType = "PageViewEvent")
        {
            var startDate = string.IsNullOrEmpty(startDateStr) ? DateTime.MinValue : DateTime.Parse(startDateStr);
            var endDate = string.IsNullOrEmpty(endDateStr) ? DateTime.MaxValue : DateTime.Parse(endDateStr);

            var results = GetContactsAsync(startDate, endDate, eventType);

            //TODO: push to PowerBI & DI IPowerBiRepository
            PowerBiRepository repo = new PowerBiRepository();
            var accessToken = repo.GetToken();
            repo.CreateDataset(accessToken);
            
            return results;
        }

        private static XConnectClient GetClient()
        {
            var xConnectUrl =
                Sitecore.Configuration.Settings.GetSetting(Constants.Constant.XConnectUri);

            var config = new XConnectClientConfiguration(
                new XdbRuntimeModel(CollectionModel.Model),
                new Uri(xConnectUrl),
                new Uri(xConnectUrl));
            try
            {
                config.Initialize();
            }
            catch (XdbModelConflictException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return new XConnectClient(config);
        }

        private async Task<List<ContactViewModel>> GetContactsAsync(DateTime startDate, DateTime endDate,
            string eventType)
        {
            using (var client = GetClient())
            {
                var queryable = client.Contacts
                    .WithExpandOptions(new ContactExpandOptions()
                    {
                        Interactions = new RelatedInteractionsExpandOptions()
                        {
                            StartDateTime = startDate,
                            EndDateTime = endDate,
                            Limit = int.MaxValue
                        }
                    });

                var results = await queryable.ToSearchResults();
                var contacts = await results.Results.Select(x => x.Item).ToList();

                var contactList = new List<ContactViewModel>();

                foreach (var contact in contacts)
                {
                    var events = contact.Interactions.Select(x => x.Events);

                    var interactionEvents =
                        (from interactionEvent in events
                            from xdbEvent in interactionEvent
                            where xdbEvent.XObject.XdbType.ClrType.FullName != null &&
                                  xdbEvent.XObject.XdbType.ClrType.FullName.Contains(eventType)
                            select new InteractionEvent()
                            {
                                EventType = xdbEvent.XObject.XdbType.ClrType.FullName,
                                Data = xdbEvent.Data,
                                DataKey = xdbEvent.DataKey,
                                DefinitionId = xdbEvent.DefinitionId,
                                ItemId = xdbEvent.ItemId,
                                EngagementValue = xdbEvent.EngagementValue,
                                Id = xdbEvent.Id,
                                ParentEventId = xdbEvent.ParentEventId,
                                Text = xdbEvent.Text,
                                Timestamp = xdbEvent.Timestamp,
                                Duration = xdbEvent.Duration
                            }).ToList();

                    contactList.Add(new ContactViewModel
                    {
                        ContactId = contact.Id,
                        Events = interactionEvents,
                        IsKnown = contact.IsKnown
                    });
                }

                return contactList;
            }
        }
        
    }
}