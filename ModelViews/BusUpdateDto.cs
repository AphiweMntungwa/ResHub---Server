namespace ResHub.ModelViews
{
    using System;
    using System.Collections.Generic;

    public class BusUpdateDto
    {
        public string BusNumber { get; set; }
        public string? BusDriver { get; set; }
        public string? BusDriverPhoneNumber { get; set; }
        public List<string> FromTimes { get; set; }
        public List<string> ToTimes { get; set; }

    }
}
