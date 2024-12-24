using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace AVFLab
{
    class AVFLog
    {
        private string logFile = "avflogfile.txt";

        public void WriteLog(string action, string details)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFile, true))
                {
                    writer.WriteLine($"[{DateTime.Now}] ACTION: {action} DETAILS: {details}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log: {ex.Message}");
            }
        }

        public void ReadLog()
        {
            try
            {
                using (StreamReader reader = new StreamReader(logFile))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading log: {ex.Message}");
            }
        }

        public void SearchLog(string keyword)
        {
            try
            {
                using (StreamReader reader = new StreamReader(logFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(keyword))
                        {
                            Console.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching log: {ex.Message}");
            }
        }
    }

    class AVFDiskInfo
    {
        public void GetDiskInfo()
        {
            try
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        Console.WriteLine($"Drive {drive.Name}");
                        Console.WriteLine($"  File system: {drive.DriveFormat}");
                        Console.WriteLine($"  Total size: {drive.TotalSize}");
                        Console.WriteLine($"  Free space: {drive.TotalFreeSpace}");
                        Console.WriteLine($"  Volume label: {drive.VolumeLabel}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting disk info: {ex.Message}");
            }
        }
    }

    class AVFFileInfo
    {
        public void GetFileInfo(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Console.WriteLine($"Full path: {fileInfo.FullName}");
                Console.WriteLine($"Size: {fileInfo.Length}");
                Console.WriteLine($"Extension: {fileInfo.Extension}");
                Console.WriteLine($"Name: {fileInfo.Name}");
                Console.WriteLine($"Created: {fileInfo.CreationTime}");
                Console.WriteLine($"Last modified: {fileInfo.LastWriteTime}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting file info: {ex.Message}");
            }
        }
    }

    class AVFDirInfo
    {
        public void GetDirInfo(string dirPath)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                Console.WriteLine($"Files count: {dirInfo.GetFiles().Length}");
                Console.WriteLine($"Creation time: {dirInfo.CreationTime}");
                Console.WriteLine($"Subdirectories count: {dirInfo.GetDirectories().Length}");

                Console.WriteLine("Parent directories:");
                DirectoryInfo parent = dirInfo.Parent;
                while (parent != null)
                {
                    Console.WriteLine($"  {parent.FullName}");
                    parent = parent.Parent;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting directory info: {ex.Message}");
            }
        }
    }

    class AVFFileManager
    {
        public void InspectDirectory(string drive)
        {
            try
            {
                string inspectDir = "AVFInspect";
                Directory.CreateDirectory(inspectDir);

                string dirInfoFile = Path.Combine(inspectDir, "avfdirinfo.txt");

                using (StreamWriter writer = new StreamWriter(dirInfoFile))
                {
                    foreach (var dir in Directory.GetDirectories(drive))
                    {
                        writer.WriteLine(dir);
                    }

                    foreach (var file in Directory.GetFiles(drive))
                    {
                        writer.WriteLine(file);
                    }
                }

                string copiedFile = Path.Combine(inspectDir, "copied_avfdirinfo.txt");
                File.Copy(dirInfoFile, copiedFile);

                File.Delete(dirInfoFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inspecting directory: {ex.Message}");
            }
        }

        public void ManageFiles(string sourceDir, string extension)
        {
            try
            {
                string filesDir = "AVFFiles";
                Directory.CreateDirectory(filesDir);

                foreach (var file in Directory.GetFiles(sourceDir, "*" + extension))
                {
                    string destFile = Path.Combine(filesDir, Path.GetFileName(file));
                    File.Copy(file, destFile);
                }

                string inspectDir = "AVFInspect";
                Directory.Move(filesDir, Path.Combine(inspectDir, filesDir));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error managing files: {ex.Message}");
            }
        }

        public void CompressAndExtract(string sourceDir, string archiveName)
        {
            try
            {
                string zipPath = archiveName + ".zip";
                ZipFile.CreateFromDirectory(sourceDir, zipPath);

                string extractDir = archiveName + "_extracted";
                ZipFile.ExtractToDirectory(zipPath, extractDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error compressing or extracting: {ex.Message}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AVFLog log = new AVFLog();
            log.WriteLog("Start", "Program started");

            // Disk Info
            AVFDiskInfo diskInfo = new AVFDiskInfo();
            diskInfo.GetDiskInfo();
            log.WriteLog("DiskInfo", "Displayed disk information");

            // File Info
            AVFFileInfo fileInfo = new AVFFileInfo();
            fileInfo.GetFileInfo("example.txt");
            log.WriteLog("FileInfo", "Displayed file information for example.txt");

            // Directory Info
            AVFDirInfo dirInfo = new AVFDirInfo();
            dirInfo.GetDirInfo(".");
            log.WriteLog("DirInfo", "Displayed directory information for current directory");

            // File Manager
            AVFFileManager fileManager = new AVFFileManager();
            fileManager.InspectDirectory("C:\\");
            log.WriteLog("FileManager", "Inspected C drive");

            fileManager.ManageFiles(".", ".txt");
            log.WriteLog("FileManager", "Managed .txt files");

            fileManager.CompressAndExtract("AVFFiles", "Archive");
            log.WriteLog("FileManager", "Compressed and extracted files");

            // Read and Search Log
            log.ReadLog();
            log.SearchLog("FileManager");

            log.WriteLog("End", "Program ended");
        }
    }
}