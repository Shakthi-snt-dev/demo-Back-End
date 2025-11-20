namespace Flowtap_Application.DtoModel.Response;

public class IntegrationResponseDto
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public bool Connected { get; set; }
    public DateTime? ConnectedAt { get; set; }
    public DateTime? LastSyncAt { get; set; }
    public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SyncResultDto
{
    public bool Success { get; set; }
    public int SyncedItems { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public DateTime Timestamp { get; set; }
}

