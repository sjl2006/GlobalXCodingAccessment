/// <summary>
/// File name: name-sorter.cs
/// Description: sort a list of names according to the last name and given name
/// Author: Haiying Lou
/// Date: 02-11-2018
/// </summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NameSorterApp
{
    public enum RetCode
    {
        SUCCESS = 0,
        INVALID_PARAM,
        INPUT_FILE_NAME_EMPTY,
        INPUT_FILE_INEXISTING,
        NO_VALID_DATA
    };

    public class NameInfo
    {
        public String LastName { get; set; }
        public String GivenName { get; set; }
    }

    /// <summary>sort a list of names according to the last name and given name</summary>
    public class SortNames
    {
        /// <summary>read names from file and put names into the given list</summary>
        /// <param name="Path">input file path</param>
        /// <param name="OutputNameList">unsorted name list</param>
        /// <returns>INVALID_PARAM, NO_VALID_DATA, SUCCESS</returns>
        public RetCode GetNameListFromFile(string Path, ref List<NameInfo> OutputNameList)
        {
            // The given name list should be an empty list.
            if ((Path == null) || (OutputNameList == null) || (OutputNameList.Count > 0))
            {
                Console.WriteLine("Program Error: INVALID_PARAM");
                return RetCode.INVALID_PARAM;
            }

            using (StreamReader Reader = new StreamReader(Path))
            {
                // Read the file line by line. 
                while (!Reader.EndOfStream)
                {
                    string Line = Reader.ReadLine();

                    // Remove redundant whitespace.
                    Line = Line.Trim();
                    Line = Regex.Replace(Line, @"\s+", " ");

                    // Ingore the blank line.
                    if (Line == "")
                    {
                        continue;
                    }

                    // Check if it is valid name.
                    // Valid name format: 1-3 given name, 1 last name,only include letters and whitespace.             
                    if (!Regex.IsMatch(Line, @"^([a-zA-Z]+\s){1,3}[a-zA-Z]+$"))
                    {
                        Console.WriteLine("Ingore this invalid name: {0}", Line);
                        continue;
                    }

                    // Extract given name and last name from the line.
                    string GivenName = Regex.Match(Line, @"^([a-zA-Z]+\s){1,3}").Value;
                    string LastName = Regex.Match(Line, @"[a-zA-Z]+$").Value;

                    // Put the extracted name into the given list.
                    OutputNameList.Add(new NameInfo() { LastName = LastName, GivenName = GivenName });
                }

                if (OutputNameList.Count == 0)
                {
                    Console.WriteLine("There are no valid names in the file.");
                    return RetCode.NO_VALID_DATA;
                }
            }

            return RetCode.SUCCESS;
        }

        /// <summary>sort the name list, save the sorted results in the same list</summary>
        /// <param name="NameList">input unsorted list and output sorted list</param>
        /// <returns>INVALID_PARAM, SUCCESS</returns>
        public RetCode SortNameList(ref List<NameInfo> NameList)
        {
            if ((NameList == null) || (NameList.Count == 0))
            {
                Console.WriteLine("Program Error: INVALID_PARAM");
                return RetCode.INVALID_PARAM;
            }

            // Use the OrderBy method to sort the names in NameList.
            // Save the sorted names in NameList.
            NameList = NameList.OrderBy(a => a.LastName).ThenBy(a => a.GivenName).ToList();

            return RetCode.SUCCESS;
        }

        /// <summary>display sorted names and write them to a file in current work directory</summary>
        /// <param name="InputNameList">sorted name list</param>
        /// <returns>INVALID_PARAM, SUCCESS</returns>
        public RetCode DispalyAndWrite(ref List<NameInfo> InputNameList)
        {
            if ((InputNameList == null) || (InputNameList.Count == 0))
            {
                Console.WriteLine("Program Error: INVALID_PARAM");
                return RetCode.INVALID_PARAM;
            }

            Console.WriteLine("The sorted names are below:");

            using (StreamWriter Writer = new StreamWriter(@"sorted-names-list.txt", false))
            {
                // Display the sorted names on screen and write into a TXT file.
                foreach (NameInfo Name in InputNameList)
                {
                    Console.WriteLine(Name.GivenName + Name.LastName);
                    Writer.WriteLine(Name.GivenName + Name.LastName);
                }
            }

            return RetCode.SUCCESS;
        }
    }

    /// <summary>parameters validity check</summary>
    public class ValidityCheck
    {
        /// <summary>parameters validity check</summary>
        /// <param name="args">command line arguements needed to check</param>
        /// <returns>INVALID_PARAM, INPUT_FILE_NAME_EMPTY, INPUT_FILE_INEXISTING, SUCCESS</returns>
        public RetCode ParameterCheck(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine("Program Error: INVALID_PARAM");
                return RetCode.INVALID_PARAM;
            }

            // Only support single arg from command line now.
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: name-sorter <FilePath>");
                return RetCode.INPUT_FILE_NAME_EMPTY;
            }

            if (!File.Exists(@args[0]))
            {
                Console.WriteLine("The file {0} does not exist", args[0]);
                return RetCode.INPUT_FILE_INEXISTING;
            }

            return RetCode.SUCCESS;
        }
    }

    class NameSorter
    {
        /// <summary>name-sorter entry point</summary>
        /// <param name="args">command line arguments</param>
        static void Main(string[] args)
        {
            RetCode enRet;
            ValidityCheck cCheck = new ValidityCheck();

            if (RetCode.SUCCESS != cCheck.ParameterCheck(@args))
            {
                return;
            }

            SortNames sn = new SortNames();

            List<NameInfo> NameList = new List<NameInfo>();

            // Get the names from file and put them into NameList.
            enRet = sn.GetNameListFromFile(@args[0], ref NameList);
            if (enRet != RetCode.SUCCESS)
            {
                return;
            }

            // Sort the names in NameList accoring to last name, given name.
            enRet = sn.SortNameList(ref NameList);
            if (enRet != RetCode.SUCCESS)
            {
                return;
            }

            // Display the sorted name list on screen and write into a TXT file.
            enRet = sn.DispalyAndWrite(ref NameList);
            if (enRet != RetCode.SUCCESS)
            {
                return;
            }
        }
    }
}
