using Domain.Common.Abstractions;

namespace Infrastructure.Common.Enums;

public class SaveChangeStatus : Enumeration
{
    public static readonly SaveChangeStatus Failure = new SaveChangeStatus(0, nameof(Failure));
    public static readonly SaveChangeStatus Succeed = new SaveChangeStatus(0, nameof(Succeed));
    public static readonly SaveChangeStatus Concurrent = new SaveChangeStatus(0, nameof(Concurrent));

    private SaveChangeStatus() { }
    private SaveChangeStatus(int value, string displayName) : base(value, displayName) { }
}
