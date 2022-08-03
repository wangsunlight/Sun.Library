using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Http;

namespace Sun.Core.Helper
{
    /// <summary>
    /// 文件和流操作
    /// </summary>
    public static partial class FileHelper
    {

        #region 上传文件,指定文件名称
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">HttpPostedFileBase</param>
        /// <param name="path">指定的保存路径</param>
        /// <returns></returns>
        public static string Upload(IFormFile file, string path)
        {
            string rm = "";
            if (file != null)
            {
                var filenames = file.FileName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                rm = Upload(file, filenames[filenames.Length - 1], path);
            }
            return rm;
        }
        #endregion

        #region 上传文件,指定文件名称
        /// <summary>
        /// 上传文件,指定文件名称
        /// </summary>
        /// <param name="file">上传文件的控件</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="path">保存文件的目录</param>
        /// <returns></returns>
        public static string Upload(IFormFile file, string fileName, string path)
        {
            return Upload(file, fileName, path, false);
        }
        #endregion

        #region 上传文件,指定文件名称
        /// <summary>
        /// 上传文件,指定文件名称
        /// </summary>
        /// <param name="file">上传文件的控件</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="path">保存文件的目录</param>
        /// <param name="HttpPath">是否返回一个用于网页上显示的图片路径</param>
        /// <returns></returns>
        public static string Upload(IFormFile file, string fileName, string path, bool HttpPath)
        {
            string rm = "";

            try
            {
                if (file != null)
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        var filenames = file.FileName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                        fileName = filenames[filenames.Length - 1];
                    }

                    string rootPath = $"{ Directory.GetCurrentDirectory() }\\wwwroot\\";
                    string subPath = rootPath + path;
                    string filePath = subPath + "\\" + fileName;

                    if (!System.IO.Directory.Exists(subPath))
                    {
                        System.IO.Directory.CreateDirectory(subPath);
                    }

                    try
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                    catch { }

                    //保存文件
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    if (HttpPath)
                    {
                        rm = "/" + path.Replace("\\", "/") + "/" + fileName;
                    }
                    else
                    {
                        rm = filePath;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return rm;
        }
        #endregion

        #region 压缩文件
        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="fileNames">要打包的文件列表(文件完整路径)</param>   
        /// <param name="GzipFileName">目标文件名</param>   
        /// <param name="CompressionLevel">压缩品质级别（0~9）</param>   
        /// <param name="SleepTimer">休眠时间（单位毫秒）</param>        
        public static bool Compress(List<string> fileNames, string GzipFileName, int CompressionLevel, int SleepTimer)
        {
            bool rm = true;
            try
            {
                WebClient client = new WebClient();
                List<System.IO.FileInfo> fileList = new List<System.IO.FileInfo>();
                if (fileNames != null)
                {
                    foreach (string item in fileNames)
                    {
                        string fileFullPath = item;
                        if (item.ToLower().StartsWith("http://"))
                        {

                            fileFullPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + string.Format("\\temp\\{0}.jpg", Guid.NewGuid().ToString()));

                            if (!System.IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\\temp"))
                            {
                                System.IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "\\temp");
                            }

                            client.DownloadFile(item, fileFullPath);

                        }

                        if (File.Exists(fileFullPath))
                        {
                            fileList.Add(new System.IO.FileInfo(fileFullPath));
                        }
                    }
                    client.Dispose();
                    rm = Compress(fileList, GzipFileName, CompressionLevel, SleepTimer);
                }
                else
                {
                    rm = false;
                }

            }
            catch (Exception ex)
            {
                rm = false;
            }
            return rm;
        }
        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="fileNames">要打包的文件列表</param>   
        /// <param name="GzipFileName">目标文件名</param>   
        /// <param name="CompressionLevel">压缩品质级别（0~9）</param>   
        /// <param name="SleepTimer">休眠时间（单位毫秒）</param>        
        public static bool Compress(List<System.IO.FileInfo> fileNames, string GzipFileName, int CompressionLevel, int SleepTimer)
        {
            bool rm = false;
            ZipOutputStream s = new ZipOutputStream(File.Create(GzipFileName));
            try
            {
                s.SetLevel(CompressionLevel);   //0 - store only to 9 - means best compression   
                foreach (System.IO.FileInfo file in fileNames)
                {
                    FileStream fs = null;
                    try
                    {
                        fs = file.Open(FileMode.Open, FileAccess.ReadWrite);
                    }
                    catch
                    {
                        continue;
                    }
                    //  方法二，将文件分批读入缓冲区   
                    byte[] data = new byte[2048];
                    int size = 2048;
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file.Name));
                    entry.DateTime = (file.CreationTime > file.LastWriteTime ? file.LastWriteTime : file.CreationTime);
                    s.PutNextEntry(entry);
                    while (true)
                    {
                        size = fs.Read(data, 0, size);
                        if (size <= 0) break;
                        s.Write(data, 0, size);
                    }
                    fs.Close();
                    Thread.Sleep(SleepTimer);
                }
                rm = true;
            }
            catch (Exception ex)
            {
                rm = false;
            }
            finally
            {
                s.Finish();
                s.Close();
            }
            return rm;
        }
        #endregion

        #region 解压缩文件
        /// <summary>   
        /// 解压缩文件   
        /// </summary>   
        /// <param name="GzipFile">压缩包文件名</param>   
        /// <param name="targetPath">解压缩目标路径</param>          
        public static bool Decompress(string GzipFile, string targetPath)
        {
            bool rm = false;
            try
            {
                string directoryName = targetPath;
                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);//生成解压目录   
                string CurrentDirectory = directoryName;
                byte[] data = new byte[2048];
                int size = 2048;
                ZipEntry theEntry = null;
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(GzipFile)))
                {
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        if (theEntry.IsDirectory)
                        {
                            // 该结点是目录   
                            if (!Directory.Exists(CurrentDirectory + theEntry.Name))
                                Directory.CreateDirectory(CurrentDirectory + theEntry.Name);
                        }
                        else
                        {
                            if (theEntry.Name != string.Empty)
                            {
                                //解压文件到指定的目录   
                                using (FileStream streamWriter = File.Create(CurrentDirectory + theEntry.Name))
                                {
                                    while (true)
                                    {
                                        size = s.Read(data, 0, data.Length);
                                        if (size <= 0)
                                            break;

                                        streamWriter.Write(data, 0, size);
                                    }
                                    streamWriter.Close();
                                }
                            }
                        }
                    }
                    s.Close();
                }
                rm = true;
            }
            catch (Exception ex)
            {
                rm = false;
            }
            return rm;
        }
        #endregion

        #region 获取文件物理路径
        /// <summary>
        /// 获取文件物理路径
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string GetPhysicalPath(string strPath)
        {
            strPath = strPath.TrimStart('/').Replace("/", "\\").TrimStart('\\');
            if (strPath.StartsWith("\\"))
            {
                strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
            }
            else if (strPath.StartsWith("~"))
            {
                strPath = strPath.TrimStart('~').TrimStart('\\');
            }
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        }
        #endregion


        #region 检查文件目录是否存在，不存在测创建
        /// <summary>
        /// 检查文件目录是否存在，不存在测创建
        /// </summary>
        /// <param name="fileName">文件夹位置</param>
        public static void CheckDirectory(string fileName)
        {
            string path = System.IO.Path.GetDirectoryName(fileName);

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }
        #endregion

        #region 写文件
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="fileName">文件位置(包括文件名字及其后缀名，例如：C://1.txt)</param>
        /// <param name="content"></param>
        /// <param name="replace">是否覆盖</param>
        public static void WriteFile(string fileName, string content, bool replace = true)
        {
            CheckDirectory(fileName);


            if (File.Exists(fileName))
            {
                if (replace)
                {
                    File.Delete(fileName);
                    FileStream fs = File.Create(fileName);
                    byte[] contentBytes = UTF8Encoding.UTF8.GetBytes(content);
                    fs.Write(contentBytes, 0, contentBytes.Length);
                    fs.Close();
                }
                else
                {
                    File.AppendAllText(fileName, content);
                }
            }
            else
            {
                FileStream fs = File.Create(fileName);
                byte[] contentBytes = UTF8Encoding.UTF8.GetBytes(content);
                fs.Write(contentBytes, 0, contentBytes.Length);
                fs.Close();
            }

        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="fileName">文件位置(包括文件名字及其后缀名，例如：C://1.txt)</param>
        /// <param name="contentBytes"></param>
        /// <param name="replace">是否覆盖</param>
        public static void WriteFile(string filePath, byte[] contentBytes, bool replace = true)
        {
            if (File.Exists(filePath))
            {
                if (replace)
                {
                    File.Delete(filePath);
                }
            }
            FileStream fs = File.Create(filePath);
            fs.Write(contentBytes, 0, contentBytes.Length);
            fs.Close();

        }
        #endregion 

        /// <summary>
        /// 流转换为字节流
        /// </summary>
        /// <param name="stream">流</param>
        public static async Task<byte[]> ToBytesAsync(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }

        /// <summary>
        /// 流转换为字节流
        /// </summary>
        /// <param name="stream">流</param>
        public static byte[] ToBytes(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        /// <summary>
        /// 字符串转换成字节数组
        /// </summary>
        /// <param name="data">数据,默认字符编码utf-8</param>        
        public static byte[] ToBytes(string data)
        {
            return ToBytes(data, Encoding.UTF8);
        }

        /// <summary>
        /// 字符串转换成字节数组
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="encoding">字符编码</param>
        public static byte[] ToBytes(string data, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(data))
                return new byte[] { };
            return encoding.GetBytes(data);
        }

        /// <summary>
        /// 将文件读取到字节流中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static byte[] Read(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return null;
            var fileInfo = new FileInfo(filePath);
            using (var reader = new BinaryReader(fileInfo.Open(FileMode.Open)))
            {
                return reader.ReadBytes((int)fileInfo.Length);
            }
        }

        /// <summary>
        /// 流转换成字符串
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="bufferSize">缓冲区大小</param>
        /// <param name="isCloseStream">读取完成是否释放流，默认为true</param>
        public static string ToString(Stream stream, Encoding encoding = null, int bufferSize = 1024 * 2, bool isCloseStream = true)
        {
            if (stream == null)
                return string.Empty;
            if (encoding == null)
                encoding = Encoding.UTF8;
            if (!stream.CanRead)
                return string.Empty;
            using (var reader = new StreamReader(stream, encoding, true, bufferSize, !isCloseStream))
            {
                if (stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);
                var result = reader.ReadToEnd();
                if (stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);
                return result;
            }
        }

        /// <summary>
        /// 流转换成字符串
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="bufferSize">缓冲区大小</param>
        /// <param name="isCloseStream">读取完成是否释放流，默认为true</param>
        public static async Task<string> ToStringAsync(Stream stream, Encoding encoding = null, int bufferSize = 1024 * 2, bool isCloseStream = true)
        {
            if (stream == null)
                return string.Empty;
            if (encoding == null)
                encoding = Encoding.UTF8;
            if (!stream.CanRead)
                return string.Empty;
            using (var reader = new StreamReader(stream, encoding, true, bufferSize, !isCloseStream))
            {
                if (stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);
                var result = await reader.ReadToEndAsync();
                if (stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);
                return result;
            }
        }

        /// <summary>
        /// 复制流并转换成字符串
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">字符编码</param>
        public static async Task<string> CopyToStringAsync(Stream stream, Encoding encoding = null)
        {
            if (stream == null)
                return string.Empty;
            if (encoding == null)
                encoding = Encoding.UTF8;
            if (!stream.CanRead)
                return string.Empty;
            using (var memoryStream = new MemoryStream())
            {
                using (var reader = new StreamReader(memoryStream, encoding))
                {
                    if (stream.CanSeek)
                        stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(memoryStream);
                    if (memoryStream.CanSeek)
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    var result = await reader.ReadToEndAsync();
                    if (stream.CanSeek)
                        stream.Seek(0, SeekOrigin.Begin);
                    return result;
                }
            }
        }
    }
}
