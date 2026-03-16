namespace Application.DTOs;

public class CategoryPageDto
{
    public int TotalSize { get; set; }
    public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

}