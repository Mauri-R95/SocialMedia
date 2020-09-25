using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.BackgroundTasks
{
    public class RedisSubcriber : BackgroundService
    {
        private readonly IConnectionMultiplexer _connectionRedis;

        public RedisSubcriber(IConnectionMultiplexer connectionRedis)
        {
            _connectionRedis = connectionRedis;

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subcriber = _connectionRedis.GetSubscriber();
            return subcriber.SubscribeAsync("Messages", (channel, value) =>
            {
            Console.WriteLine($"The message content was: { value}"); 
            });
        }
    }
}
