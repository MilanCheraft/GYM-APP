using MdePe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Models
{
    public class SelectableItem
    {
        public Exercise Exercise { get; set; }
        public bool IsSelected { get; set; }
    }
}
