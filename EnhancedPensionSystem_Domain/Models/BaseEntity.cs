﻿namespace EnhancedPensionSystem_Domain.Models;

public class BaseEntity : IBaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public bool IsDeleted { get; set; } = false;
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; } 
    public string? LastAction { get; set; }
}
