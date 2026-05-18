using TaskTrackerApi.Application.Services;

namespace TaskTrackerApi.Endpoints;

public static class StatsEndpoints
{
    public static void MapStatsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/stats", async (TaskService svc) =>
        {
            var stats = await svc.GetStatsAsync();
            return Results.Ok(stats);
        });
    }
}
