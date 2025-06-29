using MediatR;

namespace backend.Application.DTOs
{
    public class ExportExcelDto : IRequest<byte[]>
    {
        public string SheetName { get; set; }
        public List<string> Columns { get; set; } = new();
        public List<List<object>> Rows { get; set; } = new();
    }

}
