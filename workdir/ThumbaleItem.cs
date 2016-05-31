using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace workdir
{
    class ThumbableItem
    {
        Image _ThumbImage = null;

        public ThumbableItem(int ThumbListIndex, String FileFullName, string FileExtension)
        {
            Index = ThumbListIndex;
            _FileFullName = FileFullName;
            _FileExtension = FileExtension;
        }
        private int Index;

        public int ThumbListIndex
        {
            get { return Index; }
            set { Index = value; }
        }
        private string _FileFullName;

        public string FileFullName
        {
            get { return _FileFullName; }
            set { _FileFullName = value; }
        }
        private string _FileExtension;

        public string FileExtension
        {
            get { return _FileExtension; }
            set { _FileExtension = value; }
        }
        public Image ThumbImage 
        {
            get 
            {
                return _ThumbImage;
            }
            set 
            { 
                _ThumbImage = value; 
            } 
        }

    }
}
