namespace Application.DTOs;

public class ItemPageDto
{
    public int TotalSize { get; set; }
    public IEnumerable<ItemDto> Items { get; set; } = new List<ItemDto>();
}