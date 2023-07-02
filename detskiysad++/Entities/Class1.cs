using System.Collections.Generic;
using System.Linq;

namespace detskiysad__.Entities
{
    partial class Сhildren
    {
        public string FullName
        {
            get
            {
                return $"{ChildrenSurname} {ChildrenName} {ChildrenPatronymic}";
            }
        }

        public string Extra_classAsString
        {
            get { return string.Join(", ", Extra_class.Select(ec => ec.Name)); }
        }


        public string ExtraClassName { get; set; }

        public virtual ICollection<Extra_class> ExtraClasses { get; set; }
        public int Attendance { get; set; }
    }
}

