using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameSorterApp;

namespace NameSorterApp.MSUnitTest
{
    [TestClass]
    public class SortNames_UnitTest
    {
        private readonly SortNames InstSortNames;

        public SortNames_UnitTest() => InstSortNames = new SortNames();

        /// <summary>
        /// check if the data and order is identical
        /// </summary>
        /// <param name="ListA">List A</param>
        /// <param name="ListB">List B</param>
        /// <returns>true, false</returns>
        private bool ListCompare(ref List<NameInfo> ListA, ref List<NameInfo> ListB)
        {
            bool Result = true;

            if (ListA.Count == ListB.Count)
            {
                for (int i = 0; i < ListA.Count; i++)
                {
                    if ((ListA[i].LastName != ListB[i].LastName)
                    || (ListA[i].GivenName != ListB[i].GivenName))
                    {
                        Result = false;
                        break;
                    }
                }
            }
            else
            {
                Result = false;
            }
            return Result;
        }

        [TestMethod]
        /// <summary> Test when input file path string is null</summary>
        public void TestGetNameListFromFile_NullInputPath()
        {
            List<NameInfo> NameList = new List<NameInfo>();
            string InputPath = null;
            RetCode retCode;

            retCode = InstSortNames.GetNameListFromFile(InputPath, ref NameList);

            Assert.IsTrue(retCode == RetCode.INVALID_PARAM);
        }

        [TestMethod]
        /// <summary> Test when input list is null</summary>
        public void TestGetNameListFromFile_NullList()
        {
            List<NameInfo> NameList = null;
            string InputPath = @"../../../TestData/NormalData.txt";
            RetCode retCode;

            retCode = InstSortNames.GetNameListFromFile(InputPath, ref NameList);

            Assert.IsTrue(retCode == RetCode.INVALID_PARAM);
        }

        [TestMethod]
        /// <summary> Test when input list is not empty</summary>
        public void TestGetNameListFromFile_NonEmptyList()
        {
            List<NameInfo> NameList = new List<NameInfo>();
            string InputPath = @"../../../TestData/NormalData.txt";
            RetCode retCode;

            NameList.Add(new NameInfo() { LastName = "Archer", GivenName = "Adonis Julius" });
            retCode = InstSortNames.GetNameListFromFile(InputPath, ref NameList);

            Assert.IsTrue(retCode == RetCode.INVALID_PARAM);
        }

        [TestMethod]
        /// <summary> Test when the file contains normal data</summary>
        public void TestGetNameListFromFile_NormalData()
        {
            List<NameInfo> NameList = new List<NameInfo>();
            List<NameInfo> ExpectedList = new List<NameInfo>();

            bool Result = false;
            RetCode retCode;

            string InputPath = @"../../../TestData/NormalData.txt";

            ExpectedList.Add(new NameInfo() { LastName = "Parsons", GivenName = "Janet " });
            ExpectedList.Add(new NameInfo() { LastName = "Lewis", GivenName = "Vaughn " });
            ExpectedList.Add(new NameInfo() { LastName = "Archer", GivenName = "Adonis Julius " });

            retCode = InstSortNames.GetNameListFromFile(InputPath, ref NameList);

            //compare output list with expected list
            Result = ListCompare(ref ExpectedList, ref NameList);
            Assert.IsTrue((retCode == RetCode.SUCCESS) && (NameList.Count > 0) && Result);
        }

        [TestMethod]
        /// <summary> Test when the file contains invalid data</summary>
        public void TestGetNameListFromFile_InvalidData()
        {
            List<NameInfo> NameList = new List<NameInfo>();
            List<NameInfo> ExpectedList = new List<NameInfo>();

            bool Result = false;
            RetCode retCode;

            string InputPath = @"../../../TestData/InvalidData.txt";

            ExpectedList.Add(new NameInfo() { LastName = "Lewis", GivenName = "Vaughn " });
            ExpectedList.Add(new NameInfo() { LastName = "Archer", GivenName = "Adonis Julius " });
            ExpectedList.Add(new NameInfo() { LastName = "Yoder", GivenName = "Shelby Nathan " });

            retCode = InstSortNames.GetNameListFromFile(InputPath, ref NameList);

            //compare output list with expected list
            Result = ListCompare(ref ExpectedList, ref NameList);
            Assert.IsTrue((retCode == RetCode.SUCCESS) && (NameList.Count > 0) && Result);
        }

        [TestMethod]
        /// <summary> Test when the file is empty</summary>
        public void TestGetNameListFromFile_EmptyFile_1()
        {
            List<NameInfo> NameList = new List<NameInfo>();
            string InputPath = @"../../../TestData/EmptyData.txt";
            RetCode retCode;

            retCode = InstSortNames.GetNameListFromFile(InputPath, ref NameList);

            Assert.IsTrue((retCode == RetCode.NO_VALID_DATA) && (NameList.Count == 0));
        }

        [TestMethod]
        /// <summary> Test when the file contains only blank lines</summary>
        public void TestGetNameListFromFile_EmptyFile_2()
        {
            List<NameInfo> NameList = new List<NameInfo>();
            string InputPath = @"../../../TestData/EmptyDataOnlyBlank.txt";
            RetCode retCode;

            retCode = InstSortNames.GetNameListFromFile(InputPath, ref NameList);

            Assert.IsTrue((retCode == RetCode.NO_VALID_DATA) && (NameList.Count == 0));
        }

        [TestMethod]
        /// <summary> Test when the list is null</summary>
        public void TestSortNameList_NullList()
        {
            List<NameInfo> NameList = null;
            RetCode retCode;

            retCode = InstSortNames.SortNameList(ref NameList);

            Assert.IsTrue(retCode == RetCode.INVALID_PARAM);
        }

        [TestMethod]
        /// <summary> Test when the list contains normal data</summary>
        public void TestSortNameList_Normal()
        {
            List<NameInfo> NameList = new List<NameInfo>();
            List<NameInfo> ExpectedList = new List<NameInfo>();

            RetCode retCode;
            bool Result;

            ExpectedList.Add(new NameInfo() { LastName = "Archer", GivenName = "Adonis Julius" });
            ExpectedList.Add(new NameInfo() { LastName = "Clarke", GivenName = "Hunter Uriah Mathew" });
            ExpectedList.Add(new NameInfo() { LastName = "Lopez", GivenName = "Mikayla" });

            NameList.Add(new NameInfo() { LastName = "Lopez", GivenName = "Mikayla" });
            NameList.Add(new NameInfo() { LastName = "Archer", GivenName = "Adonis Julius" });
            NameList.Add(new NameInfo() { LastName = "Clarke", GivenName = "Hunter Uriah Mathew" });
            retCode = InstSortNames.SortNameList(ref NameList);

            Result = ListCompare(ref ExpectedList, ref NameList);
            Assert.IsTrue((retCode == RetCode.SUCCESS) && Result);
        }

        [TestMethod]
        /// <summary> Test when the list is null</summary>
        public void TestDispalyAndWrite_NullList()
        {
            List<NameInfo> NameList = null;
            RetCode retCode;

            retCode = InstSortNames.DispalyAndWrite(ref NameList);

            Assert.IsTrue(retCode == RetCode.INVALID_PARAM);
        }

        [TestMethod]
        /// <summary> Test when the list is empty</summary>
        public void TestDispalyAndWrite_EmptyList()
        {
            List<NameInfo> NameList = new List<NameInfo>();
            RetCode retCode;

            retCode = InstSortNames.DispalyAndWrite(ref NameList);

            Assert.IsTrue(retCode == RetCode.INVALID_PARAM);
        }

        [TestMethod]
        /// <summary> Test when the list contains normal data</summary>
        public void TestDispalyAndWrite_Normal()
        {
            List<NameInfo> NameList = new List<NameInfo>();
            RetCode retCode;

            NameList.Add(new NameInfo() { LastName = "Lopez", GivenName = "Mikayla" });
            NameList.Add(new NameInfo() { LastName = "Archer", GivenName = "Adonis Julius" });
            NameList.Add(new NameInfo() { LastName = "Clarke", GivenName = "Hunter Uriah Mathew" });
            retCode = InstSortNames.DispalyAndWrite(ref NameList);

            Assert.IsTrue(retCode == RetCode.SUCCESS);
        }
    }

    [TestClass]
    public class ValidityCheck_UnitTest
    {
        private readonly ValidityCheck instValidityCheck;

        public ValidityCheck_UnitTest() => instValidityCheck = new ValidityCheck();

        [TestMethod]
        /// <summary> Test when the command line args is null</summary>
        public void TestValidityCheck_InvalidArgs()
        {
            RetCode retCode;
            string[] astArgs = null;

            retCode = instValidityCheck.ParameterCheck(astArgs);

            Assert.IsTrue(retCode == RetCode.INVALID_PARAM);
        }

        [TestMethod]
        /// <summary> Test when no command line args is given</summary>
        public void TestValidityCheck_NoInputFile()
        {
            RetCode retCode;
            string[] astArgs = { };

            retCode = instValidityCheck.ParameterCheck(astArgs);

            Assert.IsTrue(retCode == RetCode.INPUT_FILE_NAME_EMPTY);
        }

        [TestMethod]
        /// <summary> Test when the data file does not exist</summary>
        public void TestValidityCheck_InexistingInputFile()
        {
            RetCode retCode;
            string[] astArgs = { @"C:\test.txt" };

            retCode = instValidityCheck.ParameterCheck(astArgs);

            Assert.IsTrue(retCode == RetCode.INPUT_FILE_INEXISTING);
        }

        [TestMethod]
        /// <summary> Test when the file path does not exist</summary>
        public void TestValidityCheck_InexistingPath()
        {
            RetCode retCode;
            string[] astArgs = { @"A:\Users\lhy\source\repos\ConsoleApp1\ConsoleApp1\test.txt" };

            retCode = instValidityCheck.ParameterCheck(astArgs);

            Assert.IsTrue(retCode == RetCode.INPUT_FILE_INEXISTING);
        }

        [TestMethod]
        /// <summary> Test when the file name is illegal</summary>
        public void TestValidityCheck_IllegalFileName()
        {
            RetCode retCode;
            string[] astArgs = { "/|\\~!@#$%^<>?" };

            retCode = instValidityCheck.ParameterCheck(astArgs);

            Assert.IsTrue(retCode == RetCode.INPUT_FILE_INEXISTING);
        }

        [TestMethod]
        /// <summary> Test with normal data in relative path</summary>
        public void TestValidityCheck_NormalRelativeFilePath()
        {
            RetCode retCode;
            string[] astArgs = { @"../../../TestData/NormalData.txt" };

            retCode = instValidityCheck.ParameterCheck(astArgs);

            Assert.IsTrue(retCode == RetCode.SUCCESS);
        }
    }
}
