﻿//using System;
//using System.Collections.Generic;
//using System.Text;
//using Primitives;
//using System.Xml;
//using System.IO;
//using Utility;
//using System.Numerics;

//namespace InputFileParser
//{
//    /// <summary>
//    /// Represents an X3D file to be displayed and converted into an ArcFile
//    /// </summary>
//    public class AsciiSTLFile
//    {
//        /// <summary>
//        /// Gets or sets the name of the file
//        /// </summary>
//        /// <value>The name.</value>
//        public string Name { get; set; }
//        /// <summary>
//        /// Gets or sets the full path to the file
//        /// </summary>
//        /// <value>The full path.</value>
//        public string FullPath { get; set; }

//        /// <summary>
//        /// Gets or sets the shape list populated by the items in the X3D file
//        /// </summary>
//        /// <value>The shape list.</value>
//        public ShapeList ShapeList { get; set; }

//        public AsciiSTLFile(string fullPath)
//        {
//            Name = Path.GetFileNameWithoutExtension(fullPath);
//            FullPath = fullPath;
//            Clean();
//        }

//        /// <summary>Edits the base file to make it XML compliant by removing any opening angle brackets inside quotes.</summary>
//        private void Clean()
//        {
//            string text = File.ReadAllText(FullPath);
//            text = text.Replace("\"<", "\"");
//            text = text.Replace(">\"", "\"");
//            File.WriteAllText(FullPath, text);
//        }

//        /// <summary>Parses the STL file that this AsciiSTLFile represents and extracts the IndexedFaceSets within.</summary>
//        public void Parse()
//        {
//            ShapeList = new ShapeList();
//            double scale = 1;

//            StreamReader sr = new StreamReader(FullPath);
//            string s = "";
//            string name = "";
//            string coordIndices = "";
//            string points = "";
//            //Issues sometimes exist if reading before the head element due to format issues in files generated by Blender. So skip that section of the file.
//            while (!sr.EndOfStream)
//            {
//                s = sr.ReadLine();
//                string[] words = s.Trim().Split(' ');
//                string firstWord = words[0];
//                switch (firstWord)
//                {
//                    case "facet":
//                        Vector4 normal = new Vector4(DecimalParser.ParseToDouble(words[2]), words[3], words[4], 0);
//                        break;
//                    case "outer":
//                        break;
//                    case "endloop":
//                        break;
//                    case "endfacet":
//                        break;
//                        if (reader.Name == "Transform")
//                        {
//                            name = reader["DEF"];
//                        }
//                        else if (reader.Name == "IndexedFaceSet")
//                        {
//                            coordIndices = reader["coordIndex"];
//                        }
//                        else if (reader.Name == "Coordinate")
//                        {
//                            points = reader["point"];
//                        }
//                        else if (reader.Name == "Viewpoint")
//                        {
//                            string[] camera = reader["position"].Split(' ');
//                        }
//                        break;
//                }

//                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "IndexedFaceSet")
//                {
//                    IndexedFaceSet ifs = new IndexedFaceSet(name, coordIndices, points, scale);
//                    ShapeList.IndexedFaceSets.Add(ifs);
//                }
//                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Scene")
//                {
//                    break;
//                }
//            }

//            sr.Close();
//        }

//        /// <summary>Alters the file, switching the definition of the faces such that their vertices will defined in the opposite direction (clockwise if counterclockwise and vice versa). This turns Back-Facing faces into Front-Facing and vice versa, essentially turning the objects inside-out. This is used to correct inconsistancies in the way way x3d files are created by different programs.</summary>
//        public void SwitchBackFront()
//        {
//            XmlDocument doc = new XmlDocument();
//            doc.Load(FullPath);
//            XmlNodeList nodeList = doc.SelectNodes("//IndexedFaceSet");

//            foreach (XmlNode node in nodeList)
//            {
//                StringBuilder newValue = new StringBuilder();

//                XmlNode coordNode = node.Attributes.GetNamedItem("coordIndex");
//                string[] faces = coordNode.Value.Split(',');
//                for (int faceIndex = 0; faceIndex < faces.Length - 1; faceIndex++)
//                {
//                    string[] values = faces[faceIndex].TrimStart().Split(' ');
//                    for (int i = values.Length - 2; i >= 0; i--)
//                    {
//                        newValue.Append(values[i]).Append(" ");
//                    }
//                    newValue.Append(values[values.Length - 1]).Append(", ");
//                }

//                coordNode.Value = newValue.ToString();
//            }

//            doc.Save(FullPath);
//        }

//        /// <summary>
//        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
//        /// </summary>
//        /// <returns>
//        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
//        /// </returns>
//        public override string ToString()
//        {
//            return Name;
//        }

//    }
//}
