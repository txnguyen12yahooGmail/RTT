using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_CommonBusinessLayer
{
    public class CompleteDateTime
    {
        public DateTime? FullDateTime { get; set; }

        public string DateTimeText
        {
            get
            {
                if (FullDateTime != null)
                {
                    string T = FullDateTime.ToString();
                    return T;
                }
                else
                {
                    return "";
                }
            }
        }

        public string DateText
        {
            get
            {
                if (FullDateTime != null)
                {
                    string value = FullDateTime.Value.ToString("MM/dd/yyyy");
                    return value;
                }
                else
                {
                    return "";
                }
            }

        }

        public string TimeText
        {
            get
            {
                if (FullDateTime != null)
                {
                    string value = FullDateTime.Value.ToString("hh:mm ttt");
                    return value;
                }
                else
                {
                    return "";
                }
            }
        }


    }
}
