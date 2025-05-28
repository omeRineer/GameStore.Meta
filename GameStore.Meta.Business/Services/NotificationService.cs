using Core.Utilities.ResultTool;
using GameStore.Meta.Business.Hubs;
using GameStore.Meta.DataAccess.Repositories;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Notification;
using MeArch.Module.Security.Model.Dto;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Business.Services
{
    public class NotificationService
    {
        readonly NotificationRepository NotificationRepository;
        readonly CurrentUser CurrentUser;
        readonly IHubContext<NotificationHub> Hub;
        public NotificationService(NotificationRepository notifications, CurrentUser currentUser, IHubContext<NotificationHub> hub)
        {
            NotificationRepository = notifications;
            CurrentUser = currentUser;
            Hub = hub;
        }

        public async Task<IDataResult<GetNotificationsModel>> GetNotificationsAsync()
        {
            var userId = Guid.Parse(CurrentUser.Id.ToString());
            var notifications = await NotificationRepository.GetListAsync(f => f.TargetUsers == null ||
                                                                            (f.TargetUsers.Contains(userId)));

            var result = new GetNotificationsModel
            {
                Notifications = notifications.Select(s => new GetNotifications_Item
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    CreateDate = s.CreateDate,
                    IsRead = s.ReadBy != null ? s.ReadBy.Contains(userId) 
                                              : false
                }).ToList()
            };

            return new SuccessDataResult<GetNotificationsModel>(result);
        }

        public async Task<IResult> SendAsync(CreateNotificationModel req)
        {
            var createResult = await CreateAsync(new Notification
            {
                Title = req.Title,
                Description = req.Description,
                ReadBy = req.ReadBy,
                TargetUsers = req.TargetUsers
            });

            if (createResult.Success)
                await Hub.Clients.All.SendAsync("ReceiveNotification", new PushNotificationModel
                {
                    Title = req.Title,
                    Description = req.Description,
                    CreateDate = DateTime.Now
                });
            else
                return createResult;

            return new SuccessResult();
        }

        private async Task<IResult> CreateAsync(Notification notification)
        {
            await NotificationRepository.AddAsync(new Notification
            {
                Title = notification.Title,
                Description = notification.Description,
                CreateDate = DateTime.Now,
                EditDate = DateTime.Now
            });

            return new SuccessResult();
        }
    }
}
