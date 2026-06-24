namespace ERP.Business.Features.CRM.Dtos;

public sealed class CustomerNoteDto
{
    public int Id { get; set; }
    public string Note { get; set; } = string.Empty;
    public DateTime NoteDate { get; set; }
    public string? CreatedByUserId { get; set; }
}
