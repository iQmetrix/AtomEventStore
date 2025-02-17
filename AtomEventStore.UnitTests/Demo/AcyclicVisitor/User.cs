﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Grean.AtomEventStore.UnitTests.Demo.AcyclicVisitor
{
    public class User
    {
        private readonly Guid id;
        private readonly string name;
        private readonly string password;
        private readonly string email;
        private readonly bool emailVerified;

        public User(
            Guid id,
            string name,
            string password,
            string email,
            bool emailVerified)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.email = email;
            this.emailVerified = emailVerified;
        }

        public static User Create(UserCreated @event)
        {
            return new User(
                @event.UserId,
                @event.UserName,
                @event.Password,
                @event.Email,
                false);
        }

        public User Accept(EmailVerified @event)
        {
            return new User(
                id,
                name,
                password,
                email,
                true);
        }

        public User Accept(EmailChanged @event)
        {
            return new User(
                id,
                name,
                password,
                @event.NewEmail,
                false);
        }

        private User Accept(object @event)
        {
            var emailChanged = @event as EmailChanged;
            if (emailChanged != null)
                return Accept(emailChanged);

            var emailVerified = @event as EmailVerified;
            if (emailVerified != null)
                return Accept(emailVerified);

            throw new ArgumentException("Unexpected event type.", "@event");
        }

        public static User Fold(IEnumerable<object> events)
        {
            var first = events.First();
            var rest = events.Skip(1);

            var created = first as UserCreated;
            if (created == null)
                throw new ArgumentException("The first event must be a UserCreated instance.", "events");

            var user = Create(created);
            return rest.Aggregate(user, (u, e) => u.Accept(e));
        }

        public static User Fold(params object[] events)
        {
            return Fold(events.AsEnumerable());
        }

        public Guid Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Password
        {
            get { return password; }
        }

        public string Email
        {
            get { return email; }
        }

        public bool EmailVerified
        {
            get { return emailVerified; }
        }
    }
}
