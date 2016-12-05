using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Entities
{
    public abstract class Category
    {
        public virtual string CategoryName { get; set; }

        public virtual void PrintCategoryName()
        {
            Console.WriteLine(this.CategoryName);   
        }
    }
}
