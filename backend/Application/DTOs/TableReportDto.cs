namespace backend.Application.DTOs
{
    public class TableReportDto
    {
        public List<string> Columns { get; set; }
        public List<List<object>> Rows { get; set; }
        public string SheetName { get; set; }
    }
}