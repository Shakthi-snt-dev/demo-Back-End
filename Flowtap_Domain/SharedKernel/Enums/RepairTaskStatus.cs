namespace Flowtap_Domain.SharedKernel.Enums;

public enum RepairTaskStatus
{
    New,
    WaitingForInspection,
    InProgress,
    WaitingForParts,
    ReadyForPickup,
    Completed,
    Returned,
    Cancelled
}

