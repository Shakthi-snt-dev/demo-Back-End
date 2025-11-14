using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Store.Entities;

public class StoreSettings
{
    [Key]
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }

    public bool EnablePOS { get; set; } = true;

    public bool EnableInventory { get; set; } = true;

    public string TimeZone { get; set; } = "UTC";

    public string? BusinessHoursJson { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

