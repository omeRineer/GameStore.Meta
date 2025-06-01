using Configuration;
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
using System.Globalization;
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
            var userId = CurrentUser.Id.ToString();
            var notification = await NotificationRepository.GetSingleAsync(f => f.Id == id);

            if (notification.ReadBy == null)
            {
                notification.ReadBy = new List<string> { userId };
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
                Level = notification.Level,
                ContentType = notification.ContentType,
                Sender = notification.Sender,
                Title = notification.Title,
                Content = notification.Content,
                IsRead = notification.ReadBy.Contains(userId),
                CreateDate = notification.CreateDate,
                Custom = notification.Custom
            };

            return new SuccessDataResult<GetNotificationDetailModel>(result);
        }

        public async Task<IDataResult<GetNotificationsModel>> GetListAsync()
        {
            var userId = CurrentUser.Id.ToString();

            var notifications = await NotificationRepository.GetListAsync(f => f.Sender == userId ||
                                                                                        (f.Targets == null ||
                                                                                         f.Targets.Contains(userId)));

            var result = new GetNotificationsModel
            {
                Notifications = notifications.Select(s => new GetNotifications_Item
                {
                    Id = s.Id,
                    Type = s.Type,
                    Level = s.Level,
                    ContentType = s.ContentType,
                    Content = s.ContentType == "text" ? s.Content : null,
                    Sender = s.Sender,
                    Title = s.Title,
                    CreateDate = s.CreateDate,
                    IsRead = s.ReadBy != null ? s.ReadBy.Contains(userId)
                                              : false,
                    Custom = s.Custom
                }).ToList()
            };

            return new SuccessDataResult<GetNotificationsModel>(result);
        }

        public async Task<IResult> SendAsync(CreateNotificationModel req)
        {
            var checkResult = CheckNotification(req.ContentType, req.Level);
            if (!checkResult.Success)
                return checkResult;

            var senderId = CurrentUser.Id.ToString();
            var newNotification = new Notification
            {
                Id = Guid.NewGuid(),
                Level = req.Level.ToLower(new CultureInfo("en-US")),
                Sender = senderId,
                Title = req.Title,
                Content = req.Content,
                ContentType = req.ContentType.ToLower(new CultureInfo("en-US")),
                Targets = req.TargetUsers,
                CreateDate = DateTime.Now,
                Custom = req.Custom
            };
            await NotificationRepository.AddAsync(newNotification);

            await PushToClientsAsync(newNotification);

            return new SuccessResult();
        }

        public async Task<IResult> PushAsync(PushNotificationModel req)
        {
            var checkResult = CheckNotification(req.ContentType, req.Level);
            if (!checkResult.Success)
                return checkResult;

            var newNotification = new Notification
            {
                Id = Guid.NewGuid(),
                Level = req.Level.ToLower(new CultureInfo("en-US")),
                Type = req.Type,
                Sender = req.Sender,
                Title = req.Title,
                Content = req.Content,
                ContentType = req.ContentType.ToLower(new CultureInfo("en-US")),
                Targets = req.Targets,
                CreateDate = DateTime.Now,
                Custom = req.Custom
            };
            await NotificationRepository.AddAsync(newNotification);

            await PushToClientsAsync(newNotification);

            return new SuccessResult();
        }

        private async Task PushToClientsAsync(Notification notification)
        {
            var connections = HubConnectionManager.GetConnections();
            var targets = notification.Targets?.Select(s => s.ToString()).ToList();
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
                    Level = notification.Level,
                    Type = notification.Type,
                    ContentType = notification.ContentType,
                    Content = notification.Content,
                    Title = notification.Title,
                    Sender = notification.Sender,
                    IsRead = false,
                    CreateDate = notification.CreateDate,
                    Custom = notification.Custom
                });
            }
        }

        private IResult CheckNotification(string contentType, string level)
        {
            if (string.IsNullOrEmpty(contentType) || string.IsNullOrEmpty(level))
                return new ErrorResult($"contentType or level cannot null");

            if (!MetaConfiguration.NotificationOptions.ContentTypes.Contains(contentType.ToLower(new CultureInfo("en-US"))))
                return new ErrorResult($"{contentType} is not supported");

            if (!MetaConfiguration.NotificationOptions.Levels.Contains(level.ToLower(new CultureInfo("en-US"))))
                return new ErrorResult($"{level} is not supported");

            return new SuccessResult();
        }
    }
}
