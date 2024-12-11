using System.Collections.Generic;
using System.IO;
using System.Text;
using Primitives;
using QuantumConcepts.Formats.StereoLithography;
using System.Xml;

namespace InputFileParser
{
    public class STLFile: IFileDecoder
    {
        /// <summary>
        /// Gets or sets the name of the file
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the full path to the file
        /// </summary>
        /// <value>The full path.</value>
        public string FullPath { get; set; }

        /// <summary>
        /// Gets or sets the shape list populated by the items in the X3D file
        /// </summary>
        /// <value>The shape list.</value>
        public ShapeList ShapeList { get; set; }

        public STLFile(string fullPath)
        {
            Name = Path.GetFileNameWithoutExtension(fullPath);
            FullPath = fullPath;
            Clean();
        }
        
        /// <summary>Edits the base file to make it XML compliant by removing any opening angle brackets inside quotes.</summary>
        private void Clean()
        {
        }
        public void Parse()
        {
            ShapeList = new ShapeList();
            STLDocument stlBinary;
            bool exists = File.Exists(FullPath);
            if (!exists)
            {
                return;
            }
            using (FileStream fs = File.OpenRead(FullPath))
            {
                byte[] buffer = { };
                stlBinary = STLDocument.Read(fs, true);
                IList<Facet> facets = stlBinary.Facets;

                IndexedFaceSet ifs = new IndexedFaceSet(facets);
                ShapeList.IndexedFaceSets.Add(ifs);
            }
        }

        /// <summary>Alters the file, switching the definition of the faces such that their vertices will defined in the opposite direction (clockwise if counterclockwise and vice versa). This turns Back-Facing faces into Front-Facing and vice versa, essentially turning the objects inside-out. This is used to correct inconsistancies in the way way x3d files are created by different programs.</summary>
        public void SwitchBackFront()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FullPath);
            XmlNodeList nodeList = doc.SelectNodes("//IndexedFaceSet");

            foreach (XmlNode node in nodeList)
            {
                StringBuilder newValue = new StringBuilder();

                XmlNode coordNode = node.Attributes.GetNamedItem("coordIndex");
                string[] faces = coordNode.Value.Split(',');
                for (int faceIndex = 0; faceIndex < faces.Length - 1; faceIndex++)
                {
                    string[] values = faces[faceIndex].TrimStart().Split(' ');
                    for (int i = values.Length - 2; i >= 0; i--)
                    {
                        newValue.Append(values[i]).Append(" ");
                    }
                    newValue.Append(values[values.Length - 1]).Append(", ");
                }

                coordNode.Value = newValue.ToString();
            }

            doc.Save(FullPath);
        }
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
