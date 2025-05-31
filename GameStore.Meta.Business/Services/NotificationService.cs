using Core.Entities.DTO.Pagination;
using Core.Utilities.ResultTool;
using GameStore.Meta.DataAccess.Repositories;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Message;
using GameStore.Meta.Models.Rest.Notification;
using GameStore.Meta.Models.SignalR;
using GameStore.Meta.SignalR.Hubs;
using GameStore.Meta.SignalR.Utilities;
using MeArch.Module.Security.Entities.Master;
using MeArch.Module.Security.Model.Dto;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver.Core.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Business.Services
{
    public class NotificationService
    {
        readonly HubConnectionManager<NotificationHub> HubConnectionManager;
        readonly NotificationRepository NotificationRepository;
        readonly CurrentUser CurrentUser;
        readonly IHubContext<NotificationHub> Hub;
        public NotificationService(NotificationRepository notifications, CurrentUser currentUser, IHubContext<NotificationHub> hub, HubConnectionManager<NotificationHub> hubConnectionManager)
        {
            NotificationRepository = notifications;
            CurrentUser = currentUser;
            Hub = hub;
            HubConnectionManager = hubConnectionManager;
        }

        public async Task<IDataResult<GetNotificationDetailModel>> GetDetailAsync(Guid id)
        {
            var userId = Guid.Parse(CurrentUser.Id.ToString());
            var notification = await NotificationRepository.GetSingleAsync(f => f.Id == id);

            if (notification.ReadBy == null)
            {
                notification.ReadBy = new List<Guid> { userId };
                await NotificationRepository.UpdateAsync(notification);
            }
            else if (!notification.ReadBy.Contains(userId))
            {
                notification.ReadBy.Add(userId);
                await NotificationRepository.UpdateAsync(notification);
            }

            var result = new GetNotificationDetailModel
            {
                Id = notification.Id,
                Type = notification.Type,
                ContentType = notification.ContentType,
                Sender = notification.Sender,
                Title = notification.Title,
                Content = notification.Content,
                IsRead = notification.ReadBy.Contains(userId),
                CreateDate = notification.CreateDate
            };

            return new SuccessDataResult<GetNotificationDetailModel>(result);
        }

        public async Task<IDataResult<GetNotificationsModel>> GetListAsync()
        {
            var userId = Guid.Parse(CurrentUser.Id.ToString());

            var notifications = await NotificationRepository.GetListAsync(f => f.Sender == userId ||
                                                                                        (f.TargetUsers == null ||
                                                                                         f.TargetUsers.Contains(userId)));

            var result = new GetNotificationsModel
            {
                Notifications = notifications.Select(s => new GetNotifications_Item
                {
                    Id = s.Id,
                    Type = s.Type,
                    ContentType = s.ContentType,
                    Content = s.ContentType == "TEXT" ? s.Content : null,
                    Sender = s.Sender,
                    Title = s.Title,
                    CreateDate = s.CreateDate,
                    IsRead = s.ReadBy != null ? s.ReadBy.Contains(userId)
                                              : false
                }).ToList()
            };

            return new SuccessDataResult<GetNotificationsModel>(result);
        }

        public async Task<IResult> SendAsync(CreateNotificationModel req)
        {
            var senderId = CurrentUser.Id.ToString();
            var newNotification = new Notification
            {
                Id = Guid.NewGuid(),
                Type = req.Type?.ToLower() ?? "info",
                Sender = Guid.Parse(senderId),
                Title = req.Title,
                Content = req.Content,
                ContentType = req.ContentType ?? "TEXT",
                TargetUsers = req.TargetUsers,
                CreateDate = DateTime.Now
            };
            await NotificationRepository.AddAsync(newNotification);

            await PushToClientsAsync(newNotification);

            return new SuccessResult();
        }

        public async Task<IResult> PushAsync(PushNotificationModel req)
        {
            var newNotification = new Notification
            {
                Id = Guid.NewGuid(),
                Type = req.Type?.ToLower() ?? "info",
                Sender = req.Sender,
                Title = req.Title,
                Content = req.Content,
                ContentType = req.ContentType ?? "TEXT",
                TargetUsers = req.TargetUsers,
                CreateDate = DateTime.Now
            };
            await NotificationRepository.AddAsync(newNotification);

            await PushToClientsAsync(newNotification);

            return new SuccessResult();
        }

        private async Task PushToClientsAsync(Notification notification)
        {
            var connections = HubConnectionManager.GetConnections();
            var targets = notification.TargetUsers?.Select(s => s.ToString()).ToList();
            if (targets != null)
                connections = connections.Where(f => targets.Contains(f.Key) || f.Key == notification.Sender.ToString());

            var connectionIdList = connections.SelectMany(s => s.Value)
                                              .Select(s => s)
                                              .ToList();

            foreach (var connection in connectionIdList)
            {
                await Hub.Clients.Client(connection).SendAsync("ReceiveNotification", new ReceiveNotificationModel
                {
                    Id = notification.Id,
                    Type = notification.Type,
                    ContentType = notification.ContentType,
                    Content = notification.Content,
                    Title = notification.Title,
                    Sender = notification.Sender,
                    IsRead = false,
                    CreateDate = notification.CreateDate
                });
            }
        }
    }
}
