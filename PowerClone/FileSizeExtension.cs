using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PowerClone
{
    public static class FileSizeExtensions
    {
        public static long GetRealSize(this FileInfo info)
        {

            var result = GetDiskFreeSpaceW(info.Directory.Root.FullName, out var sectorsPerCluster, out var bytesPerSector, out _, out _);
            if (result == 0) throw new Win32Exception();
            var clusterSize = sectorsPerCluster * bytesPerSector;
            var lSize = GetCompressedFileSizeW(info.FullName, out var hSize);
            var realSize = (long)hSize << 32 | lSize;
            return (realSize + clusterSize - 1) / clusterSize * clusterSize;
        }

        [DllImport("kernel32.dll")]
        private static extern uint GetCompressedFileSizeW([In, MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
            [Out, MarshalAs(UnmanagedType.U4)] out uint lpFileSizeHigh);

        [DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
        private static extern int GetDiskFreeSpaceW([In, MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName,
            out uint lpSectorsPerCluster, out uint lpBytesPerSector, out uint lpNumberOfFreeClusters,
            out uint lpTotalNumberOfClusters);
    }
}
