using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primitives;

namespace InputFileParser
{
    public interface IFileDecoder
    {
        string Name { get; set; }
        string FullPath { get; set; }    
        ShapeList ShapeList { get; set; }
        void SwitchBackFront();
        void Parse();
    }
}
