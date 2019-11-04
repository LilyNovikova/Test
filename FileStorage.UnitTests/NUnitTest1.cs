using System;
using NUnit.Framework;
using ConsoleApp1;
using System.Reflection;
using ConsoleApp1.UnitTests;
using FileSystem.exception;

namespace ConsoleApp1
{

    namespace ConsoleApp1.UnitTests
    {
        [TestFixture]
        public class FileTests
        {
            [TestCase("file.txt", "lalala", 3.0, "txt", true)]
            [TestCase("longTextFileName.txt", "test string", 5.5, "txt", true)]
            [TestCase("file", "", 0.0, null, false)]
            [TestCase(null, "lalala", 3.0, null, typeof(NullReferenceException))]
            [TestCase("file.txt", null, 0.0, "txt", typeof(NullReferenceException))]
            [TestCase("file.txt", "", 0.0, "txt", true)]
            [TestCase("c:\file.txt", "lalala", 3.0, "txt", false)]
            [TestCase("", "lalala", 3.0, null, typeof(NullReferenceException))]
            public void TestFile_Constructor(String filename, String content, double size, String extension, Object expected)
            {
                try
                {
                    File file = new File(filename, content);
                    Assert.AreEqual(TestUtils.GetClassField(file, "filename"), filename);

                    Assert.AreEqual(TestUtils.GetClassField(file, "content"), content);
                    Assert.AreEqual((double)TestUtils.GetClassField(file, "size"), size);
                    Assert.AreEqual(TestUtils.GetClassField(file, "extension"), extension);
                    Assert.AreEqual(true, expected);
                }
                catch (Exception e)
                {
                    Assert.AreEqual(e.GetType(), expected);
                }
            }

            [TestCase("lalala", 3.0, true)]
            [TestCase("test string", 5.5, true)]
            [TestCase("", 0.0, true)]
            [TestCase(null, 0.0, typeof(NullReferenceException))]
            public void TestFile_GetSize(String content, double size, Object expected)
            {
                try
                {
                    Assert.AreEqual((new File("filename", content)).getSize() == size, expected);
                }
                catch (Exception e)
                {
                    Assert.AreEqual(e.GetType(), expected);
                }
            }

            [TestCase("filename", true)]
            [TestCase("", typeof(NullReferenceException))]
            [TestCase(null, typeof(NullReferenceException))]
            public void TestFile_GetFilename(String filename, Object expected)
            {
                try
                {
                    Assert.AreEqual((new File(filename, "content")).getFilename().Equals(filename), expected);
                }
                catch (Exception e)
                {
                    Assert.AreEqual(e.GetType(), expected);
                }
            }

        }

        [TestFixture]
        public class FileStorageConstructorsTests
        {
            [Test]
            public void TestFileStorage_DefaultConstructor()
            {
                FileStorage storage = new FileStorage();
                Assert.IsTrue(((double)TestUtils.GetClassField(storage, "availableSize")) == 100.0 &&
                                ((double)TestUtils.GetClassField(storage, "maxSize")) == 100.0);
            }

            [TestCase(int.MaxValue)]
            [TestCase(-110)]
            [TestCase(100)]
            public void TestFileStorage_Constructor(int MaxSize)
            {
                FileStorage storage = new FileStorage(MaxSize);
                Assert.IsTrue((double)TestUtils.GetClassField(storage, "availableSize") == MaxSize &&
                    (double)TestUtils.GetClassField(storage, "maxSize") == MaxSize);
            }
        }

        [TestFixture]
        public class FileStorageWriteTests
        {
            public enum TestResults
            {
                True,
                False,
                FileNameAlreadyExistsException,
                NullReferenceException
            }
            private FileStorage storage;
            [SetUp]
            public void TestSetUp()
            {
                storage = new FileStorage();
                try
                {
                    storage.write(new File("file.txt.txt", ""));
                }
                catch
                {

                }
            }

            [TestCase("file.txt", "content", ExpectedResult = TestResults.True)]
            [TestCase("File.txt", "content", ExpectedResult = TestResults.True)]
            [TestCase("File.txt", "", ExpectedResult = TestResults.True, Description = "Trying to add empty file")]
            [TestCase(null, "content", ExpectedResult = TestResults.NullReferenceException, Description = "Trying to add invalid file")]
            [TestCase("file.txt.txt", "content", ExpectedResult = TestResults.FileNameAlreadyExistsException, Description = "Trying to add file with already existing name")]
            public TestResults TestFileStorage_WriteSmallFiles(String filename, String content)
            {
                try
                {
                    return storage.write(new File(filename, content)) ? TestResults.True : TestResults.False;
                }
                catch (FileNameAlreadyExistsException)
                {
                    return TestResults.FileNameAlreadyExistsException;
                }
                catch (NullReferenceException)
                {
                    return TestResults.NullReferenceException;
                }
            }

            [TestCase("Text.txt", 20000, ExpectedResult = TestResults.True)]
            [TestCase("Text.txt", 99, ExpectedResult = TestResults.True)]
            [TestCase("Text.txt", 100, ExpectedResult = TestResults.True)]
            public TestResults TestFileStorage_WriteBigFiles(String filename, int ContentLength)
            {
                try
                {
                    return storage.write(new File(filename, TestUtils.GenerateSymbols(ContentLength))) ? TestResults.True : TestResults.False;
                }
                catch (FileNameAlreadyExistsException)
                {
                    return TestResults.FileNameAlreadyExistsException;
                }
                catch
                {
                    //If another type of exception catched
                    return TestResults.NullReferenceException;
                }
            }


            [TestCase("")]
            [TestCase(";")]
            [TestCase(";;")]
            [TestCase("letter")]
            [Test, Timeout(20000)]
            public void TestFileStorage_WriteUntilStorageIsFull(String content)
            {
                int i = 0;
                bool next = true;
                storage = new FileStorage();
                while (next)
                {
                    try
                    {
                        next = storage.write(new File(String.Format("{0}.txt", (i++).ToString()), content));
                    }
                    catch { }
                }
                Assert.IsTrue((double)content.Length / 2 > (double)TestUtils.GetClassField(storage, "availableSize"));
            }
        }

        [TestFixture]
        public class FileStorageOtherMethodsTests
        {
            public enum TestResults
            {
                True,
                False,
                FileNameAlreadyExistsException,
                NullReferenceException
            }

            private FileStorage EmptyStorage = new FileStorage();
            private FileStorage StorageWithAFile = new FileStorage();
            [SetUp]
            public void TestSetUp()
            {
                try
                {
                    StorageWithAFile.write(new File("file.txt.txt", ""));
                }
                catch
                {

                }
            }

            [TestCase("file.txt", false, false)]
            [TestCase("Text.txt", false, false)]
            [TestCase("file.txt.txt", true, false)]
            [TestCase(null, false, false)]
            [TestCase("", false, false)]
            public void TestFileStorage_IsExists(String filename, bool ExpectedForStorageWithAFile, bool ExpectedForEmptyStorage)
            {
                Assert.AreEqual(StorageWithAFile.isExists(filename), ExpectedForStorageWithAFile);
                Assert.AreEqual(EmptyStorage.isExists(filename), ExpectedForEmptyStorage);
            }

            [TestCase("file.txt", 0, false)]
            [TestCase("file.txt", 1, false)]
            [TestCase("file.txt.txt", 1, true)]
            [TestCase("file.txt.txt", 10, true)]
            [TestCase(null, 0, false)]
            [TestCase(null, 1, false)]
            [TestCase(null, 10, false)]
            [TestCase("Text.txt", 1, false)]
            [TestCase("Text.txt", 10, false)]
            public void TestFileStorage_Delete(String filename, int NumberOfFilesInStorage, Object expected)
            {
                try
                {
                    if (NumberOfFilesInStorage == 0)
                    {
                        Assert.AreEqual(EmptyStorage.delete(filename), expected);
                    }
                    else
                    {
                        for (int i = 0; i < NumberOfFilesInStorage - 1; i++)
                        {
                            StorageWithAFile.write(new File(String.Format("{0}.txt", i), "content"));
                        }
                        Assert.AreEqual(StorageWithAFile.delete(filename), expected);
                    }
                }
                catch (Exception e)
                {
                    Assert.AreEqual(e.GetType(), expected);
                }

            }

            [Test]
            public void TestFileStorage_GetFiles()
            {
                Assert.AreSame(StorageWithAFile.getFiles(), TestUtils.GetClassField(StorageWithAFile, "files"));
                Assert.AreSame(EmptyStorage.getFiles(), TestUtils.GetClassField(EmptyStorage, "files"));
            }


            [TestCase("file.txt.txt", true)]
            [TestCase("file.txt", false)]
            [TestCase("NotInStorage", false)]
            public void TestFileStorage_GetFile(String filename, bool expected)
            {
                Assert.AreEqual(expected, StorageWithAFile.getFile(filename) != null &&
                    StorageWithAFile.getFile(filename).getFilename().Equals(filename));
            }
        }
    }
}
