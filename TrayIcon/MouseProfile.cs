using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrayIcon {
    class MouseProfile {
        public string Name { get; set; }
        public int MouseSpeed { get; set; }
        public int ScrollSpeed { get; set; }
        public int DoubleClickSpeed { get; set; }

        public MouseProfile(string Name, int MouseSpeed, int ScrollSpeed, int DoubleClickSpeed) {
            this.Name = Name;
            this.MouseSpeed = MouseSpeed;
            this.ScrollSpeed = ScrollSpeed;
            this.DoubleClickSpeed = DoubleClickSpeed;
        }
        public MouseProfile() {

        }
    }
}
