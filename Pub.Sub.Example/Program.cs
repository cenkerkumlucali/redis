using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1453");
ISubscriber subscriber = connection.GetSubscriber();