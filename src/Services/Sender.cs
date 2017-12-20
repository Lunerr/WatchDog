﻿using Discord;
using System;
using System.Threading;
using System.Threading.Tasks;
using WatchDog.Extensions;

namespace WatchDog.Services
{
    public sealed class Sender
    {
        private readonly ThreadLocal<Random> _random;
        private readonly Configuration _configuration;

        public Sender(ThreadLocal<Random> random, Configuration configuration)
        {
            _random = random;
            _configuration = configuration;
        }

        public Task SendAsync(IMessageChannel channel, string description, string title = null, Color? color = null)
        {
            var builder = new EmbedBuilder
            {
                Color = color ?? new Color(_random.Value.ArrayElement(_configuration.Colors)),
                Description = description,
                Title = title
            };

            return channel.SendMessageAsync("", false, builder.Build());
        }

        public Task ReplyAsync(IUser user, IMessageChannel channel, string description, string title = null, Color? color = null)
        {
            return SendAsync(channel, user.Tag() + ", " + description, title, color);
        }

        public Task ReplyErrorAsync(IUser user, IMessageChannel channel, string description)
        {
            return ReplyAsync(user, channel, description, null, new Color(_configuration.ErrorColor));
        }
    }
}
