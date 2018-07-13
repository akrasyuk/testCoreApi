using System;
using System.ComponentModel.DataAnnotations;

namespace TestCoreApp.Entities
{
    public class Resource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public int UpdatedBy { get; set; }

        [Range(0, 100, ErrorMessage = "The value must be between 0 and 100")]
        public int CustomProperty1 { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "The value must be between 6 and 12")]
        public string CustomProperty2 { get; set; }

        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "01/01/2018", "01/12/9999", ErrorMessage = "Valid date range is 01/01/2018 - 12/31/9999")]
        public DateTime CustomProperty3 { get; set; }

        public decimal CustomProperty4 { get; set; }

        public int? CustomProperty5 { get; set; }
        public string CustomProperty6 { get; set; }
        public DateTime CustomProperty7 { get; set; }
    }
}
