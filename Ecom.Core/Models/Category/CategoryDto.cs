using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Models.Category
{
    public record AddCategoryDto
        (string Name, string Description);

    public record UpdateCategoryDto
        (string Name, string Description, int id);

}
