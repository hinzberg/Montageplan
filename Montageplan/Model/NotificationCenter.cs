using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class NotificationCenter
    {
        public abstract class NotificationBase
        {
            public NotificationBase(Action callback)
            {
                this.Callback = callback;
            }

            public abstract int GetHash();

            public Action Callback { get; set; }
        }
        public class ObjectNotification : NotificationBase
        {
            public ObjectNotification(int identifier, object objectRef, Action callback)
                : base(callback)
            {
                this.Identifier = identifier;
                this.ObjectRef = objectRef;
            }

            public int Identifier { get; set; }
            public object ObjectRef { get; set; }

            // NotificationBase
            public override int GetHash()
            {
                int hash;
                string hashString = this.Identifier.ToString() + this.ObjectRef.GetHashCode().ToString();
                hash = hashString.GetHashCode();
                return hash;
            }
        }
        public class IdentifierNotification : NotificationBase
        {
            public IdentifierNotification(int identifier, Action callback)
                : base(callback)
            {
                this.Identifier = identifier;
            }

            public int Identifier { get; set; }

            // NotificationBase
            public override int GetHash()
            {
                int hash;
                string hashString = this.Identifier.ToString();
                hash = hashString.GetHashCode();
                return hash;
            }
        }

        private static NotificationCenter center = null;
        public static NotificationCenter GetInstance()
        {
            if (center == null)
                center = new NotificationCenter();
            return center;
        }

        private readonly List<NotificationBase> notifications;

        private NotificationCenter()
        {
            this.notifications = new List<NotificationBase>();
        }

        public void Add(NotificationBase notification)
        {
            this.notifications.Add(notification);
        }

        public void Remove(int identifier, object objectRef)
        {
            this.Remove(new ObjectNotification(identifier, objectRef, null));
        }
        public void Remove(int identifier)
        {
            this.Remove(new IdentifierNotification(identifier, null));
        }

        public void ClearAll()
        {
            this.notifications.Clear();
        }

        public void Notify(int identifier, object objectRef)
        {
            this.Notify(new ObjectNotification(identifier, objectRef, null));
        }
        public void Notify(int identifier)
        {
            this.Notify(new IdentifierNotification(identifier, null));
        }

        private void Remove(NotificationBase notification)
        {
            int hash = notification.GetHash();
            List<NotificationBase> notifications = new List<NotificationBase>(this.GetNotifications(notification.GetHash()));
            foreach (var notiItem in notifications)
                this.notifications.Remove(notiItem);
        }

        private void Notify(NotificationBase notification)
        {
            List<NotificationBase> notifications = new List<NotificationBase>(this.GetNotifications(notification.GetHash()));
            foreach (var notiItem in notifications)
                notiItem.Callback();
        }

        private List<NotificationBase> GetNotifications(int hash)
        {
            List<NotificationBase> notifications = new List<NotificationBase>();
            foreach (var notiItem in this.notifications)
            {
                int itemHash = notiItem.GetHash();
                if (itemHash == hash)
                    notifications.Add(notiItem);
            }
            return notifications;
        }

    }
}
