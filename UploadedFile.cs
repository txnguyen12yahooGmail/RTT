using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_CommonBusinessLayer
{
    public class UploadedFile
    {
        #region Fields
        private Guid _FileID = Guid.Empty;
        private string _FileName = "";
        private string _FileExtension = "";
        private string _FileDescription = "";
        private long _FileSize = 0;
        private string _FileContentType = "";
        private byte[] _FileContents = null;
        private string _FileUploadedBy = null;
        private string _FileUploadedByDisplay = "";
        private CompleteDateTime _FileUploadedDate = null;
        #endregion

        #region Properties
        public Guid FileID
        {
            get { return _FileID; }
            set { _FileID = value; }
        }
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }
        public string FileExtension
        {
            get { return _FileExtension; }
            set { _FileExtension = value; }
        }
        public string FileDescription
        {
            get { return _FileDescription; }
            set { _FileDescription = value; }
        }
        public long FileSize
        {
            get { return _FileSize; }
            set { _FileSize = value; }
        }
        public string FileContentType
        {
            get { return _FileContentType; }
            set { _FileContentType = value; }
        }
        public byte[] FileContents
        {
            get { return _FileContents; }
            set {
                _FileContents = value;
                this._FileSize = _FileContents.Length;
            }
        }
        public string FileUploadedBy
        {
            get { return _FileUploadedBy; }
            set { _FileUploadedBy = value; }
        }
        public CompleteDateTime FileUploadedDate
        {
            get { return _FileUploadedDate; }
            set { _FileUploadedDate = value; }
        }
        public string DisplayFileSize
        {
            get
            {
                long absValue = (_FileSize < 0 ? -_FileSize : _FileSize);
            
                string suffix;
                double displayValue;
                if (absValue >= 0x40000000) // Gigabyte
                {
                    suffix = "GB";
                    displayValue = (_FileSize >> 20);
                }
                else if (absValue >= 0x100000) // Megabyte
                {
                    suffix = "MB";
                    displayValue = (_FileSize >> 10);
                }
                else if (absValue >= 0x400) // Kilobyte
                {
                    suffix = "KB";
                    displayValue = _FileSize;
                }
                else
                {
                    return _FileSize.ToString("0 B"); // Byte
                }
                
                displayValue = (displayValue / 1024);
                
                return displayValue.ToString("0.# ") + suffix;
        
            }
        }
        public string FileUploadedByDisplay
        {
            get { return _FileUploadedByDisplay; }
            set { _FileUploadedByDisplay = value; }
        }
        #endregion
    }
}
