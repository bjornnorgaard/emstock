namespace Models.Enums
{
    public enum ComponentStatus
    {
        Available = 1,
        ReservedLoaner,
        ReservedAdmin,
        Loaned,
        Defect,
        Trashed,
        Lost,
        NeverReturned
    }
}
