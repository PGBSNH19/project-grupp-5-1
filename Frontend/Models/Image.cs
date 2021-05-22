﻿using BlazorInputFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageDataURL { get; set; }
        public IFileListEntry ImageFile { get; set; }
        public string ImageURL { get; set; }
        public bool IsDefault { get; set; }
        public int ProductId { get; set; }

    }
}
