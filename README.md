# dotnet-channels-guide

[![build](https://github.com/pawla-homsi/dotnet-channels-guide/actions/workflows/build.yml/badge.svg)](https://github.com/pawla-homsi/dotnet-channels-guide/actions/workflows/build.yml)

A practical guide to `System.Threading.Channels` in .NET, with a working producer/consumer demo.

## Why

At work we had an API that did two jobs per request: the work that produced the response, and a second job that was required but had nothing to do with the response. Moving the second job off the request path would cut response time, but adding a message broker such as RabbitMQ was more infrastructure than the traffic justified.

An in-process channel solved it: the request handler writes to a channel and returns, and a background consumer does the second job. This repo explains how channels work and when they are the right tool. The full write-up is in [docs/article.md](docs/article.md) and [on Medium](https://medium.com/@paulahomsi_50101/net-channels-a21198e7103c).

## Quick start

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
git clone https://github.com/pawla-homsi/dotnet-channels-guide.git
cd dotnet-channels-guide
dotnet run --project DotNetChannels
```

The demo writes 1,000 messages through a bounded channel (capacity 100), reads them in a consumer, and exits once the producer completes the channel.

## What the demo shows

| Concept | Where |
|---|---|
| Bounded channel with back-pressure (`FullMode = Wait`) | `Program.cs` |
| Writing with `WriteAsync`, then `Complete()`, and `Complete(ex)` on failure | `Producer.cs` |
| Reading with `ReadAllAsync` until the writer completes | `Consumer.cs` |

## When to use a channel, and when not to

**Good fit:** in-process producer/consumer work, background jobs that may be lost on restart, smoothing bursts between two stages of one service.

**Not a fit:** work that must survive a crash or deploy, work shared across several instances, or anything that needs retries and dead-lettering. Use a durable queue for those.

## Tech

C#, .NET 10, `System.Threading.Channels`.

## Roadmap

- [ ] `BackgroundService` consumer in an ASP.NET Core minimal API, matching the original production scenario
- [ ] Graceful shutdown: drain the channel on `StopAsync`
- [ ] Unit tests for the producer and consumer

## Related

[dotnet-channels-benchmarks](https://github.com/pawla-homsi/dotnet-channels-benchmarks): throughput and allocation measurements.

## License

MIT
