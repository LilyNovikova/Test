using System.Collections.Generic;
using FileSystem.exception;

namespace ConsoleApp1
{
    public class FileStorage
    {
        private List<File> files = new List<File>();
        private double availableSize = 100;
        private double maxSize = 100;

        /**
         * Construct object and set max storage size and available size according passed values
         * @param size FileStorage size
         */
        public FileStorage(int size)
        {
            maxSize = size;
            availableSize += maxSize;
        }

        /**
         * Construct object and set max storage size and available size based on default value=100
         */
        public FileStorage()
        {
        }


        /**
         * Write file in storage if filename is unique and size is not more than available size
         * @param file to save in file storage
         * @return result of file saving
         * @throws FileNameAlreadyExistsException in case of already existent filename
         */
        public bool write(File file) 
        {
            if (isExists(file.getFilename())) {
                throw new FileNameAlreadyExistsException();
            }
            if (file.getSize() >= availableSize) {
                return false;
            }
            files.Add(file);
            availableSize -= file.getSize();

            return true;
        }

        /**
         * Check is file exist in storage
         * @param fileName to search
         * @return result of checking
         */
        public bool isExists(string fileName)
        {
            foreach (File file in files)
            {
                if (file.getFilename().Contains(fileName))
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * Delete file from storage
         * @param fileName of file to delete
         * @return result of file deleting
         */
        public bool delete(string fileName)
        {
            return files.Remove(getFile(fileName));
        }

        /**
         * Get all Files saved in the storage
         * @return list of files
         */
        public List<File> getFiles()
        {
            return files;
        }

        /**
         * Get file by filename
         * @param fileName of file to get
         * @return file
         */
        public File getFile(string fileName)
        {
            if (isExists(fileName))
            {
                foreach (File file in files)
                {
                    if (file.getFilename().Equals(fileName))
                    {
                        return file;
                    }
                }
            }
            return null;
        }

    }
}