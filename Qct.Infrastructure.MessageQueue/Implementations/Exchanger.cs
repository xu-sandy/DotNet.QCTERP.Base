using Qct.Infrastructure.MessageClient.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Qct.Infrastructure.MessageClient.Implementations.Repositories;
using Qct.Infrastructure.Json;

namespace Qct.Infrastructure.MessageClient.Implementations
{
    public class Exchanger : BaseEventHandler<EventJsonWrapper>, IRemoteServiceAgent
    {
        static ExchangerClient socketClient;

        public Exchanger(Guid publisherId, bool isWebSite, Action<object, EventArgs> retrySubHandler)
        {
            PublisherId = publisherId;
            IsWebSite = isWebSite;
            RetryAllSubscribes = new EventHandler(retrySubHandler);
            socketClient = new ExchangerClient(ReConnectedHandler);
            socketClient.Initialize();
        }
        Guid PublisherId { get; set; }
        bool IsWebSite { get; set; }

        public EventHandler RetryAllSubscribes { get; set; }

        private void ReConnectedHandler(object sender, EventArgs e)
        {
            MessageQueueConfigurationSection config = MessageQueueConfigurationSection.GetConfig();

            var authToken = new PublisherInformaction()
            {
                IsWebSite = IsWebSite,
                MQMToken = config.MQMPassword,
                PublisherId = PublisherId,
                WebSiteHost = config.WebSiteHost
            };
            if (socketClient.SendPublisherInformaction(authToken))
            {
                RetryAllSubscribes?.Invoke(this, new EventArgs());
                RetryFailed();
            }
        }

        public void RetryFailed()
        {
            try
            {
                var events = FindAllFailedRecord();
                foreach (var eventObject in events)
                {
                    if (socketClient.SendPublishItem(new PublishItem() { Content = eventObject.ToString(), Descriptions = eventObject.Descriptions, PublisherId = eventObject.PublisherId }))
                    {
                        RemoveFailedRecord(eventObject);
                    }
                }
            }
            catch (Exception)
            {
                //todolog
            }
        }

        public override void Handler(EventJsonWrapper eventObject)
        {
            if (eventObject == null || eventObject.IsLocalLoop)
                return;
            if (!socketClient.SendPublishItem(new PublishItem() { Content = JsonHelper.ToJson(eventObject), Descriptions = eventObject.Descriptions, PublisherId = eventObject.PublisherId }))
            {
                FailedRecord(eventObject);
            }

        }

        void FailedRecord(EventJsonWrapper _event)
        {
            var repository = new FailedMessageRepository();
            repository.CreateWithSaveChanges(_event);
            repository.Dispose();
        }
        void RemoveFailedRecord(EventJsonWrapper _event)
        {
            var repository = new FailedMessageRepository();
            repository.DeleteWithSaveChanges(_event.Id);
            repository.Dispose();

        }

        IEnumerable<EventJsonWrapper> FindAllFailedRecord()
        {
            using (var repository = new FailedMessageRepository())
            {
                return repository.GetReadOnlyEntities().ToList();
            }
        }

        public void Subscribe(SubscribeItem item)
        {
            if (item == null) return;
            socketClient.SendSubscribeItem(item);
        }
    }
}
