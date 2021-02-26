﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight
{
    public interface IFileSystem
    {
        /// <summary>
        /// Gets the base directory path.
        /// </summary>
        /// <returns>The base directory path.</returns>
        string GetBaseDirectory();

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="path">The file to check.</param>
        /// <returns>True if path contains the name of an existing file; otherwise, false.</returns>
        bool Exists(string path);

        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        /// <param name="path">The file to be opened for reading.</param>
        /// <returns>A read-only stream on the specified path.</returns>
        System.IO.Stream Open(string path);

        /// <summary>
        /// Creates or overwrites a file in the specified path.
        /// </summary>
        /// <param name="path">The path and name of the file to create.</param>
        /// <returns> A stream that provides read/write access to the file specified in path.</returns>
        System.IO.Stream Create(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        byte[] ReadBinary(System.IO.Stream stream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="bytes"></param>
        void WriteBinary(System.IO.Stream stream, byte[] bytes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        string ReadUtf8Text(System.IO.Stream stream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="text"></param>
        void WriteUtf8Text(System.IO.Stream stream, string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string ReadUtf8Text(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        void WriteUtf8Text(string path, string text);
    }
}
