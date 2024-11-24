using GameSync.Domain.GameSync.Interfaces;

namespace GameSync.Domain.GameSync.Entities;

public class LogEntity : ILogEntity
{
    public long Id { get; set; }

    public string Severity { get; set; }

    public string Data { get; set; }

    public DateTime Date { get; set; }
}
