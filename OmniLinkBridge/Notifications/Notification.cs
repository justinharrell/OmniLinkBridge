﻿using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace OmniLinkBridge.Notifications
{
    public static class Notification
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<INotification> providers = new List<INotification>()
        {
            new EmailNotification(),
            new ProwlNotification(),
            new PushoverNotification()
        };

        public static void Notify(string source, string description, NotificationPriority priority = NotificationPriority.Normal)
        {
            log.Debug($"{source} {description} {priority}");
            Parallel.ForEach(providers, (provider) =>
            {
                try
                {
                    provider.Notify(source, description, priority);
                }
                catch (Exception ex)
                {
                    log.Error("Failed to send notification", ex);
                }
            });
        }
    }
}
